using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;
using static System.Linq.Enumerable;
using Random = UnityEngine.Random;

public class CombatGameManager : MonoBehaviour
{
    private bool startLoaded;
    
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
    [SerializeField] private Button playerAttack;
    [SerializeField] private Button playerSkill;
    
    // ---------- Entities ---------- //
    [SerializeField] private Entity playerEntity;       // Player entity
    [SerializeField] private Entity[] allyList;         // List of ally entities
    [SerializeField] private Entity[] enemyList;        // List of enemy entities
    private Tuple<int, Entity> currentEntity;   // debug
    private int deadEnemies = 0;

    // ---------- Canvas ---------- //
    [SerializeField] private TMP_Text messageTMP;       // TMP for messages
    [SerializeField] private TMP_Text playerRollTMP;    // TMP for the player's roll
    [SerializeField] private TMP_Text playerHealthTMP;  // TMP for the player's health
    [SerializeField] private Animator messageAnimator;  // Animator for messages
    [SerializeField] private Animator fadeAnimator;     // Animator for black fade

    void Start()
    {
        StartAnimations();
        playerAttack.gameObject.SetActive(false);
        playerSkill.gameObject.SetActive(false);
        Invoke("AltStart", 1.0f);
    }

    private void AltStart()
    {
        Debug.Log("Starting combat...");

        // ---------- Initialize variables ---------- //
        _turnOrderList = new List<Tuple<int, Entity>>();
        _initiativeRollsFinished = false;
        _turnEnded = true;

        playerAttack.gameObject.SetActive(true);
        playerSkill.gameObject.SetActive(true);
        
        // ---------- Start combat ---------- //
        ChangeMessage("Tira iniciativa");   // Change message
        _playerRollingInitiative = true;        // Player starts rolling    
        _playerRollingAttack = false;  
        _dice.StartRolling();                   // Dice starts rolling
        playerHealthTMP.text = "" + playerEntity._stats["totalHitPoints"] + "/"+ playerEntity._stats["hitPoints"];   // Show player HP

        startLoaded = true;
        
        Debug.Log("Tirando iniciativa del jugador...");
    }
    
    void Update()
    {
        if (startLoaded)
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
                    playerAttack.enabled = true;
                    playerSkill.enabled = true;
                    Debug.Log("Player's turn - " + currentEntity.Item1);
                }
                else
                {
                    if (!currentEntity.Item2.dead)
                    {
                        Debug.Log("Enemy " + currentEntity.Item2.name + " turn - "
                                  + _turnOrderList[_currentTurn].Item1);
                        playerAttack.enabled = false;
                        playerSkill.enabled = false;
                        Invoke("NpcAction", 1.0f);
                    }
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
                    Invoke("HideRoll", 1.0f);   // Hide dice and text in 1
                    
                    playerEntity.gameObject.GetComponent<Animator>().Play(playerEntity.type + _attackType + "Attack");
                    _playerRoll = playerEntity.KayAttack(_attackType, _playerRoll);
                    
                    currentEntity.Item2.target.dmgText.text = "" + _playerRoll;
                    currentEntity.Item2.target.dmgText.gameObject.GetComponent<Animator>().Play("Float");
                    currentEntity.Item2.target.gameObject.GetComponent<Animator>().Play(currentEntity.Item2.target.type + "Hit");

                    Invoke("NextTurn", 1.0f);
                }
            }
        }
    }

    // Receives a string and changes the on screen message, then disappears in 2 seconds
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
        messageAnimator.Play("Nothing");
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
        // ShowInitiativeOrder();

        _currentTurn = 0;
        _initiativeRollsFinished = true;
    }

    // Tells the NPC Entity (enemy/ally) to decide its next action
    private void NpcAction()
    {
        int dmg = currentEntity.Item2.DecideNextAction();
        
        currentEntity.Item2.gameObject.GetComponent<Animator>().Play(currentEntity.Item2.type + "Attack");
        
        playerHealthTMP.text = playerEntity._stats["hitPoints"] + "/" + playerEntity._stats["totalHitPoints"];

        currentEntity.Item2.target.dmgText.text = "" + dmg;
        currentEntity.Item2.target.dmgText.gameObject.GetComponent<Animator>().Play("Float");
        currentEntity.Item2.target.gameObject.GetComponent<Animator>().Play(currentEntity.Item2.target.type + "Hit");
        
        Invoke("NextTurn", 1.0f);
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

    private void NextTurn()
    {
        _turnEnded = true;
        if (_currentTurn == _turnOrderList.Count - 1)
            _currentTurn = 0;
        else
            _currentTurn++;
    }

    private void StartAnimations()
    {
        Debug.Log("Starting animations");
        Debug.Log(playerEntity.type + "IdleCombat");

        fadeAnimator.Play("FadeIn");
        
        playerEntity.gameObject.GetComponent<Animator>().Play(playerEntity.type + "IdleCombat");

        for (int i = 0; i < enemyList.Length; i++)
        {
            enemyList[i].gameObject.GetComponent<Animator>().Play(enemyList[i].type + "IdleCombat");
        }
    }

    public void EntityDied(bool isPlayer)
    {
        if (isPlayer)
        {
            Debug.Log("Player died...");
            // end combat
            // Game over scene
        }
        else
        {
            Debug.Log("An enemy died...");
            deadEnemies++;

            Invoke("EnemyDeathAnimation", 0.5f);
            
            if (deadEnemies == enemyList.Length)
            {
                Debug.Log("Every enemy died...");
                
                // PlayerPrefs.SetInt("hitPoints", playerEntity._stats["hitPoints"]);
                // PlayerPrefs.Save();
                
                Invoke("FadeOut", 3.0f);
                Invoke("LoadEndScene", 4.0f);
            }
        }
    }

    private void EnemyDeathAnimation()
    {
        enemyList[deadEnemies - 1].GetComponent<Animator>().Play(enemyList[deadEnemies - 1].type + "Death");
    }
    
    private void FadeOut()
    {
        playerAttack.gameObject.SetActive(false);
        playerSkill.gameObject.SetActive(false);
        fadeAnimator.Play("Fade");
    }
    
    public void LoadEndScene()
    {
        // SceneManager.LoadScene("Scenes/Profeta");
        SceneManager.LoadScene("Scenes/MainMenu");
    }
}