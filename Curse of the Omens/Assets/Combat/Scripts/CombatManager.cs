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
    private Entity player;
    [SerializeField]
    private Entity[] enemies;

    private List<Tuple<int, Entity>> _turnOrderList;
    
    private List<Entity> _entityOrder;
    private List<int> _rollOrder;

    [SerializeField] 
    private UICombatManager cmUI;

    // Start is called before the first frame update
    void Start()
    {
        _turnOrderList = new List<Tuple<int, Entity>>();
        _entityOrder = new List<Entity>();
        _rollOrder = new List<int>();
        cmUI.RollDice();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DealDamage(Entity dealer, Entity target, int dmg)
    {
        
    }

    public void RollInitiative(int playerRoll)
    {
        foreach (Entity enemy in enemies)
        {
            // Roll the d20
            var roll = Random.Range(1, 20);
            // Add initiative - not working due to enemy dictionary not existing
            //roll += enemy._stats["initiativeBonus"];
            
            _turnOrderList.Add(new Tuple<int, Entity>(roll, enemy));
        }

        playerRoll += player._stats["initiativeBonus"];
        _turnOrderList.Add(new Tuple<int, Entity>(playerRoll, player));
        
        _turnOrderList.Sort((a,b) => a.Item1.CompareTo(b.Item1));
        
        for (int i = _turnOrderList.Count - 1; i >= 0; i--)
        {
            var entry = _turnOrderList[i];
            _rollOrder.Add(entry.Item1);
            _entityOrder.Add(entry.Item2);
        }
    }
}
