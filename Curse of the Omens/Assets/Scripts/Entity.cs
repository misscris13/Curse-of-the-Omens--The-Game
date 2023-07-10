using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class Entity : MonoBehaviour
{
    public bool isPlayer = false;  // Indicates whether the Entity is the player or not
    public bool dead = false;
    
    [SerializeField] 
    public string type = "";   // Type of entity (thief, npc...) to load from files

    [DoNotSerialize]
    public Entity target;   // Contains the target of the entity
    
    private string[] _statsToRecover;   // List of key names for stats
    private string[] _skillsToRecover;  // List of key names for skills
    
    public Dictionary<string, int> _class;      // Dictionary of entity classes
    public Dictionary<string, int> _stats;      // Dictionary of entity stats
    public Dictionary<string, float> _skills;   // Dictionary of entity skills
    public Tuple<string, float> _weapon;        // Weapon: name, damage modifier
    
    [SerializeField]
    private UnityEvent<bool> entityDiedEvent;

    // Start is called before the first frame update.
    void Start()
    {
        Debug.Log("Inicializando entidad...");
        
        _class = new Dictionary<string, int>();
        _stats = new Dictionary<string, int>();
        _skills = new Dictionary<string, float>();

        _statsToRecover = new string[]
        {
            "totalHitPoints", "hitPoints", "armorClass",
            "profBonus", "initiativeBonus",
            "str", "dex", "con", "int",
            "wis", "cha"
        };
            
        _skillsToRecover = new string[]
        {
            "athletics", "intimidation",
            "perception", "persuasion",
            "stealth"
        };
        
        if (isPlayer)
        {
            Debug.Log("Recuperando información de jugador...");
            GetClasses();
            GetStats();
            GetSkills();
        }
        else
        {
            Debug.Log("Recuperando información de npc...");
            LoadStatsFromFile();
        }
    }

    // Recovers classes and levels from PlayerPrefs, only intended for Player.
    private void GetClasses()
    {
        string class1 = PlayerPrefs.GetString("class1");
        _class.Add(class1, PlayerPrefs.GetInt(class1));
        
        if (PlayerPrefs.HasKey("class2"))
        {
            string class2 = PlayerPrefs.GetString("class2");
            _class.Add(class2, PlayerPrefs.GetInt(class2));
        }
    }

    // Recovers stats from PlayerPrefs, only intended for Player.
    private void GetStats()
    {
        foreach (string stat in _statsToRecover)
        {
            if (PlayerPrefs.HasKey(stat))
            {
                _stats.Add(stat, PlayerPrefs.GetInt(stat));
            }
        }
    }

    // Recovers skills from PlayerPrefs, only intended for Player.
    private void GetSkills()
    {
        foreach (string skill in _skillsToRecover)
        {
            if (PlayerPrefs.HasKey(skill))
            {
                _skills.Add(skill, PlayerPrefs.GetInt(skill));
            }
        }
    }

    // Loads a CSV as a character sheet, not intended for Player.
    // TODO: check if file exists
    private void LoadStatsFromFile()
    {
        string path = "Data/" + type;  // build file path
        
        TextAsset textAsset = Resources.Load<TextAsset>(path);

        string[] lines;
        string[] words;

        lines = textAsset.text.Split("\n");

        foreach (string line in lines)
        {
            words = line.Split(",");

            if (_statsToRecover.Contains(words[0]))
            {
                _stats.Add(words[0], Convert.ToInt32(words[1]));
            }
            else if (_skillsToRecover.Contains(words[0]))
            {
                _skills.Add(words[0], Convert.ToInt32(words[1]));
            }
            else
            {
                Debug.LogError("LoadDataFromFile Error: Invalid keyword");
            }
        }

        _stats.Add("totalHitPoints", _stats["hitPoints"]);
    }

    // For enemies only, decides what to do in combat.
    public int DecideNextAction()
    {
        int dmg = 0;

        if (_stats["hitPoints"] < 10 && Random.value > 0.8f)
        {
            Flee();
            Debug.Log(name + " is fleeing...");
        }
        else
        {
            Debug.Log(name + " is attacking...");
            // Calculate damage
            dmg = 15;
            Attack(dmg);
        }

        return dmg;
    }

    public int KayAttack(string attackType, int playerRoll)
    {
        playerRoll += (int)((_stats["str"] + 1)/2) - 5; // str mod
        
        Debug.Log("Kay is attacking with " + playerRoll + " damage...");
        
        if (attackType == "Skill")
        {
            float rnd = Random.Range(0, 1);

            if (rnd <= 0.3)
            {
                playerRoll *= 3;
            }
            else if (rnd <= 0.6)
            {
                playerRoll *= 2;
            }

            Attack(playerRoll);
            return playerRoll;
        }
        
        Debug.Log("Enemy now has " + target._stats["hitPoints"] + " out of " + target._stats["totalHitPoints"]);
        return playerRoll;
    }

    private void Flee()
    {
        gameObject.SetActive(false);
        Die();
    }

    private void Attack(int dmg)
    {
        Debug.Log(target.type + " has: " + target._stats["hitPoints"]);
        target._stats["hitPoints"] -= dmg;
        Debug.Log(target.type + " has: " + target._stats["hitPoints"]);

        if (target._stats["hitPoints"] <= 0)
        {
            target.Die();
        }
    }

    private void Die()
    {
        entityDiedEvent.Invoke(isPlayer);
        dead = true;
        // this.gameObject.GetComponent<Animator>().Play("Die");
    }
}
