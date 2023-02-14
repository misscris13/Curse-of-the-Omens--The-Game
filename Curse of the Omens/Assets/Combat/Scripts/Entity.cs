using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class Entity : MonoBehaviour
{
    public bool isPlayer = false;  // Indicates whether the Entity is the player or not
    
    [SerializeField] 
    private string type = "";   // Type of entity (thief, npc...) to load from files

    [DoNotSerialize]
    public Entity target;   // Contains the target of the entity
    
    private string[] _statsToRecover;   // List of key names for stats
    private string[] _skillsToRecover;  // List of key names for skills
    
    public Dictionary<string, int> _class;      // Dictionary of entity classes
    public Dictionary<string, int> _stats;      // Dictionary of entity stats
    public Dictionary<string, float> _skills;   // Dictionary of entity skills

    [SerializeField]
    private UnityEvent<Tuple<Entity, int>> attackEvent;
    [SerializeField]
    private UnityEvent endTurnEvent;

    // Start is called before the first frame update.
    void Start()
    {
        _class = new Dictionary<string, int>();
        _stats = new Dictionary<string, int>();
        _skills = new Dictionary<string, float>();

        _statsToRecover = new string[]
        {
            "hitPoints", "armorClass",
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
            GetClasses();
            GetStats();
            GetSkills();
        }
        else
        {
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
        string path = "Assets/Data/" + type + ".txt";  // build file path

        StreamReader reader = new StreamReader(path);
        
        string line;
        string[] words;

        while ((line = reader.ReadLine()) != null)  // while !eof
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
    }

    // For enemies only, decides what to do in combat.
    public IEnumerator DecideNextAction()
    {
        yield return new WaitForSeconds(2f);
        if (_stats["hitPoints"] < 10 && Random.value > 0.8f)
        {
            Flee();
            Debug.Log(name + " is fleeing");
            endTurnEvent.Invoke();
        }
        else
        {
            // Calculate damage
            var dmg = 5;
            Attack(dmg);
            //attackEvent.Invoke(new Tuple<Entity, int>(this, dmg));
            endTurnEvent.Invoke();
        }
    }

    private void Flee()
    {
        gameObject.SetActive(false);
    }

    private void Attack(int dmg)
    {
        target._stats["hitPoints"] -= dmg;

        if (target._stats["hitPoints"] <= 0)
        {
            // target.Die();
            // check if combat ended
        }

        // Move this to the last turn
        // if (target.isPlayer)
        // {
        //     PlayerPrefs.SetInt("hitPoints", target._stats["hitPoints"]);
        //     PlayerPrefs.Save();
        // }
    }
}
