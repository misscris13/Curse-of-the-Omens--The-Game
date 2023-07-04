using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
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
    private bool _playerRollingInitiative;              // True if player is rolling initiative
    private int _playerRoll;                            // Player's roll
    [SerializeField] private DiceRoll _dice;            // Dice roll script
    
    // --------- Attack Management --------- //
    private bool _playerRollingAttack;                  // True if player is rolling an attack
    private bool _activeQTE;                            // True if player is in a QTE
    private string _attackType;                         // Type of player attack (basic, skill...)
    
    // ---------- Entities ---------- //
    [SerializeField] private Entity playerEntity;       // Player entity
    [SerializeField] private Entity[] allyList;         // List of ally entities
    [SerializeField] private Entity[] enemyList;        // List of enemy entities
    private Tuple<int, Entity> currentEntity;   // debug
    // private Entity currentEntity;

    // ---------- Canvas ---------- //
    [SerializeField] private TMP_Text messageTMP;       // TMP for messages
    [SerializeField] private TMP_Text playerRollTMP;    // TMP for the player's roll
    [SerializeField] private TMP_Text playerHealthTMP;  // TMP for the player's health
    [SerializeField] private Animator messageAnimator;  // Animator for messages
    
    void Start()
    {
        Debug.Log("Starting combat...");
        
        // ---------- Initialize variables ---------- //
        _turnOrderList = new List<Tuple<int, Entity>>();
        _initiativeRollsFinished = false;
        _turnEnded = true;

        // ---------- Start combat ---------- //
        ChangeMessage("Tira iniciativa");   // Change message
        _playerRollingInitiative = true;        // Player starts rolling    
        _playerRollingAttack = false;  
        _dice.StartRolling();                   // Dice starts rolling
        playerHealthTMP.text = "" + playerEntity._stats["totalHitPoints"]
                                  + "/"+ playerEntity._stats["hitPoints"];   // Show player HP
        Debug.Log("Tirando iniciativa del jugador...");
    }
    
    void Update()
    {
        // If the player has to roll initiative
        if (_playerRollingInitiative)
        {
            _playerRoll = Random.Range(1, 20);      // Keeps generating random numbers

            if (Input.GetButtonDown("Fire1"))       // When player fires, the current roll stays
            {
                Debug.Log("Jugador ha sacado " + _playerRoll + "...");
                
                _playerRollingInitiative = false;
                playerRollTMP.text = "" + _playerRoll;  // Show roll
                _dice.StopRolling();            // Stop dice from rolling
                Invoke("HideRoll", 2.0f);   // Hide dice and text in 2s
                
                RollInitiative();                   // Start rolling other initiatives
            }
        }
        
        // When we've finished rolling initiative
        if (_initiativeRollsFinished && _turnEnded)
        {
            // Starting turn
            _turnEnded = false;

            currentEntity = _turnOrderList[_currentTurn];
            
            if (currentEntity.Item2.isPlayer)
            {
                ChangeMessage("Tu turno");   // Change message
                Debug.Log("Player's turn - " + currentEntity.Item1);
            }
            else
            {
                Debug.Log("Enemy " + currentEntity.Item2.name + " turn - "
                    + _turnOrderList[_currentTurn].Item1);
                // Invoke("NpcAction", 2.0f);
                NpcAction();
            }
        }

        if (_playerRollingAttack)
        {
            _playerRoll = Random.Range(1, 20);      // Keeps generating random numbers
            
            _dice.StartRolling();

            if (Input.GetButtonDown("Fire1"))       // When player fires, the current roll stays
            {
                Debug.Log("Jugador ha sacado " + _playerRoll + "...");
                
                _playerRollingAttack = false;

                playerRollTMP.text = "" + _playerRoll;  // Show roll
                _dice.StopRolling();            // Stop dice from rolling
                Invoke("HideRoll", 2.0f);   // Hide dice and text in 2s
                
                _activeQTE = true;
            }
        }

        if (_activeQTE)
        {
            // show qte
            
            // animation

            if (Input.GetButtonDown("Fire1")) // When player fires, the current roll stays
            {
                Debug.Log("this is a qte");
                playerEntity.KayAttack(_attackType, 10);
                
                _turnEnded = true;
                _currentTurn++;
            }

        }
    }

    // Receives a string and changes the on screen message, then disappears in 3 seconds
    private void ChangeMessage(string msg)
    {
        messageTMP.text = msg;
        messageAnimator.enabled = true;
        messageAnimator.Play("ZoomIn");
        Invoke("HideMessage", 2.0f);
    }

    // Clears message's text
    private void HideMessage()
    {
        messageTMP.text = "";
        messageAnimator.enabled = false;
    }
    
    // Manages rolls for every entity in the battle and assigns targets
    private void RollInitiative()
    {
        // Add player initiative bonus to roll
        _playerRoll += playerEntity._stats["initiativeBonus"];
        
        // Add player roll to list
        _turnOrderList.Add(new Tuple<int, Entity>(_playerRoll, playerEntity));
        
        // Set target to first enemy, by default
        playerEntity.target = enemyList[0];

        Debug.Log("Tirando iniciativa de aliados...");
        
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

        Debug.Log("Tirando iniciativa de enemigos...");
        
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

        Debug.Log("Ordenando lista de iniciativas...");
        
        // Sort list by roll
        _turnOrderList.Sort((a, b) => b.Item1.CompareTo(a.Item1));

        Debug.Log("Mostrando iniciativas...");
        
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
    
    // Tells the NPC Entity (enemy/ally) to decide its next action
    private void NpcAction()
    {
        Debug.Log("Decidiendo...");
        currentEntity.Item2.DecideNextAction();
        Debug.Log("Decidido...");
        playerHealthTMP.text = playerEntity._stats["hitPoints"] + "/" + playerEntity._stats["totalHitPoints"];
        _turnEnded = true;
        _currentTurn++;
    }

    private void HideRoll()
    {
        _dice.gameObject.SetActive(false);
        playerRollTMP.text = "";
    }

    public void RollAttack(string attackType)
    {
        Debug.Log("Tirando ataque...");
        _dice.gameObject.SetActive(true);
        _playerRollingAttack = true;
        _attackType = attackType;
    }
}
