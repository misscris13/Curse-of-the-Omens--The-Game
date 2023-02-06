using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CombatManager : MonoBehaviour
{
    [SerializeField]
    private Entity player;
    [SerializeField]
    private Entity[] enemies;

    private List<string> _turnOrder;

    // Start is called before the first frame update
    void Start()
    {
        _turnOrder = new List<string>();

        
        
        RollEnemyInitiative();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DealDamage(Entity dealer, Entity target, int dmg)
    {
        
    }

    private void RollEnemyInitiative()
    {
        int roll;
        foreach (Entity enemy in enemies)
        {
            // Roll the d20
            roll = Random.Range(1, 20);
            // Add initiative
            roll += enemy._stats["initiativeBonus"];
            
            _turnOrder.Add(roll + ";" + enemy.name);
        }
        
        _turnOrder.Sort();
    }
}
