using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using static System.Linq.Enumerable;
using Random = UnityEngine.Random;

public class CombatManager : MonoBehaviour
{
    [SerializeField]
    private Entity player;      // Player object
    [SerializeField]
    private Entity[] enemies;   // List of enemies

    private List<Tuple<int, Entity>> _turnOrderList;    // Ordered list of <roll, Entity>

    private bool _canStart;     // Marks completion of initiative rolls
    private int _currentTurn;   // Current turn (index in turn list)

    [SerializeField] 
    private UICombatManager cmUI;

    // Start is called before the first frame update
    void Start()
    {
        _turnOrderList = new List<Tuple<int, Entity>>();
        _canStart = false;
        cmUI.RollDice();
    }

    // Update is called once per frame
    void Update()
    {
        if (_canStart)
        {
            if (_turnOrderList[_currentTurn].Item2.isPlayer)
            {
                // Player actions
                Debug.Log("Player's turn");
            }
            else
            {
                Debug.Log(_turnOrderList[_currentTurn].Item2.name + "'s turn");
                _turnOrderList[_currentTurn].Item2.DecideNextAction();
            }

            if (_currentTurn == _turnOrderList.Count - 1)
            {
                _currentTurn = 0;
            }
            else
            {
                _currentTurn++;
            }
        }
    }

    public void DealDamage(Tuple<Entity, int> tuple)
    {
        var dealer = tuple.Item1;
        var target = dealer.target;
        var dmg = tuple.Item2;
        Debug.Log(dealer.name + " dealing " + dmg + " damage to " + target.name);
    }

    public void RollInitiative(int playerRoll)
    {
        foreach (Entity enemy in enemies)
        {
            // Roll the d20
            var roll = Random.Range(1, 20);
            // Add initiative - not working due to enemy dictionary not existing
            //roll += enemy._stats["initiativeBonus"];

            enemy.target = player;
            _turnOrderList.Add(new Tuple<int, Entity>(roll, enemy));
        }

        player.target = enemies[0];    // Sets player target to first enemy by default
        
        playerRoll += player._stats["initiativeBonus"];
        _turnOrderList.Add(new Tuple<int, Entity>(playerRoll, player));
        
        _turnOrderList.Sort((a,b) => a.Item1.CompareTo(b.Item1));

        _canStart = true;
        _currentTurn = 0;
    }
}
