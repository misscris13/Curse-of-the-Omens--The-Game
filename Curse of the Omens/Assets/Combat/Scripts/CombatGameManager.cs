using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using static System.Linq.Enumerable;
using Random = UnityEngine.Random;

public class CombatGameManager : MonoBehaviour
{
    // ---------- Turn Management ---------- //
    private List<Tuple<int, Entity>> _turnOrderList;    // List of <roll, Entity>
    private bool _initiativeRollsFinished;              // True if everyone rolled
    private bool _turnEnded;                            // True if the current turn ended
    private int _currentTurn;                           // Current turn, index in _turnOrder
    
    // ---------- Entities ---------- //
    [SerializeField] private Entity playerEntity;       // Player entity
    [SerializeField] private Entity[] allyList;         // List of ally entities
    [SerializeField] private Entity[] enemyList;        // List of enemy entities

    // ---------- Canvas ---------- //
    [SerializeField] private TMP_Text messageTMP;       // TMP for messages
    [SerializeField] private TMP_Text playerRollTMP;    // TMP for the player's roll
    [SerializeField] private TMP_Text playerHealthTMP;  // TMP for the player's health
    
    void Start()
    {
        // ---------- Initialize variables ---------- //
        _turnOrderList = new List<Tuple<int, Entity>>();
        _initiativeRollsFinished = false;
        _turnEnded = true;

        // ---------- Start combat ---------- //
        ChangeMessage("Tira iniciativa");   // Change message
        RollInitiative();                       // Start rolling initiative
        
    }
    
    void Update()
    {
        
    }

    // Receives a string and changes the on screen message, then disappears in 3 seconds
    private void ChangeMessage(string msg)
    {
        messageTMP.text = "msg";
        
        Invoke("HideMessage", 3.0f);
    }

    // Clears message's text
    private void HideMessage()
    {
        messageTMP.text = "";
    }
    
    // Manages player roll and rolls for every entity in the battle, assigns targets
    private void RollInitiative()
    {
        bool playerRolling = true;   // True if rolling for player's initiative
        int playerRoll = 1;          // Player's initiative roll
        
        // Loop for player roll
        while (playerRolling)
        {
            playerRoll = Random.Range(1, 20);      // Keeps generating random numbers
            playerRollTMP.text = "" + playerRoll;  // Shows current number on screen

            if (Input.GetButtonDown("Fire1"))      // When player fires, the current roll stays
            {
                playerRolling = false;
            }
        }
        
        // Add player initiative bonus to roll
        playerRoll += playerEntity._stats["initiativeBonus"];
        
        // Add player roll to list
        _turnOrderList.Add(new Tuple<int, Entity>(playerRoll, playerEntity));
        
        // Set target to first enemy, by default
        playerEntity.target = enemyList[0];
        
        // Rolling for allies
        for (int i = 0; i < allyList.Length; i++)
        {
            Entity allyEntity = allyList[i];    // Get ally

            int roll = Random.Range(1, 20);     // Roll the d20
            // TODO: Add ally initiative bonus
                // roll += allyEntity._stats["initiativeBonus"];

            _turnOrderList.Add(new Tuple<int, Entity>(roll, allyEntity));

            // Set target to random enemy
            allyEntity.target = enemyList[Random.Range(0, (enemyList.Length - 1))];
        }
        
        // Rolling for enemies
        for (int i = 0; i < enemyList.Length; i++)
        {
            Entity enemyEntity = enemyList[i];  // Get enemy
            
            int roll = Random.Range(1, 20);     // Roll the d20
            // TODO: Add enemy initiative bonus
                // roll += enemyEntity._stats["initiativeBonus"];

            _turnOrderList.Add(new Tuple<int, Entity>(roll, enemyEntity));

            // Setting target
            if (allyList.Length != 0)   // If there are allies
            {
                // Choose one at random
                int targetIndex = Random.Range(0, allyList.Length);
                
                // If it's out of bounds, choose the player
                if (targetIndex == allyList.Length)
                {
                    enemyEntity.target = playerEntity;
                }
                else
                {
                    enemyEntity.target = allyList[targetIndex];
                }
            }
            else    // If it's only the player
            {
                enemyEntity.target = playerEntity;
            }
        }
        
        // Sort list by roll
        _turnOrderList.Sort((a, b) => b.Item1.CompareTo(a.Item1));

        // Show final order
        ShowInitiativeOrder();

        _currentTurn = 0;
        _initiativeRollsFinished = true;
    }

    // Shows on-screen the current turn order, also showing the current turn
    private void ShowInitiativeOrder()
    {
        // TODO: Implement
    }
}
