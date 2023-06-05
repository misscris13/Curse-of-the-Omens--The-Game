using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using static System.Linq.Enumerable;
using Random = UnityEngine.Random;

public class CombatManagerOld : MonoBehaviour
{
    [SerializeField]
    private Entity player;      // Player object
    [SerializeField]
    private Entity[] enemies;   // List of enemies

    private List<Tuple<int, Entity>> _turnOrderList;    // Ordered list of <roll, Entity>

    private bool _canStart;     // Marks completion of initiative rolls
    private bool _turnEnd;
    private int _currentTurn;   // Current turn (index in turn list)

    [SerializeField] 
    private UICombatManager cmUI;   // 

    // Start is called before the first frame update.
    void Start()
    {
        _turnOrderList = new List<Tuple<int, Entity>>();
        _canStart = false;
        _turnEnd = true;
        cmUI.RollDice();
    }

    // Update is called once per frame.
    void Update()
    {
        cmUI.SetHP(player._stats["hitPoints"]);
        
        if (_canStart && _turnEnd)
        {
            _turnEnd = false;
            
            if (_turnOrderList[_currentTurn].Item2.isPlayer)
            {
                // Player actions
                Debug.Log("Player's turn - " + _turnOrderList[_currentTurn].Item1);
                // make UI visible/enabled
                // call EndTurn on UI
            }
            else
            {
                Debug.Log(_turnOrderList[_currentTurn].Item2.name + "'s turn - " + _turnOrderList[_currentTurn].Item1);
                StartCoroutine(_turnOrderList[_currentTurn].Item2.DecideNextAction());
            }
        }
    }

    // Deals damage to the dealer's target.
    // Receives a tuple containing <Dealer, damage>-.
    public void DealDamage(Tuple<Entity, int> tuple)
    {
        var dealer = tuple.Item1;
        var target = dealer.target;
        var dmg = tuple.Item2;
        
        target._stats["hitPoints"] -= dmg;

        if (target._stats["hitPoints"] <= 0)
        {
            // target.Die();
        }
        
        Debug.Log(dealer.name + " dealing " + dmg + " damage to " + target.name);
    }

    // Rolls the initiative dice for every enemy, then sorts the roll array and sets
    // everything so the combat can start.
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
        
        _turnOrderList.Sort((a,b) => b.Item1.CompareTo(a.Item1));
        cmUI.ShowOrder(_turnOrderList);
        
        _canStart = true;
        _currentTurn = 0;
    }

    // Allows other hierarchy elements to set _turnEnd to true and update _currentTurn.
    public void EndTurn()
    {
        _turnEnd = true;
        
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
