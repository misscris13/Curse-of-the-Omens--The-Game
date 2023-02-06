using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class Entity : MonoBehaviour
{
    [SerializeField]
    private bool isPlayer = false;

    [SerializeField] private string _name;
    
    private string[] _statsToRecover;
    private string[] _skillsToRecover;
    
    public Dictionary<string, int> _class;
    public Dictionary<string, int> _stats;
    public Dictionary<string, float> _skills;

    private UnityEvent attackEvent;
    
    // Start is called before the first frame update
    void Start()
    {
        _class = new Dictionary<string, int>();
        _stats = new Dictionary<string, int>();
        _skills = new Dictionary<string, float>();

        if (isPlayer)
        {
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

            GetClasses();
            GetStats();
            GetSkills();
        }
        else
        {
            LoadFromFile();
        }
    }

    // Recovers classes and levels from PlayerPrefs, only intended for Player
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

    // Recovers stats from PlayerPrefs, only intended for Player
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

    // Recovers skills from PlayerPrefs, only intended for Player
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

    // Loads a CSV as a character sheet, not intended for Player
    private void LoadFromFile()
    {
        
    }

    // For enemies only, decides what to do in combat
    public void DecideNextAction()
    {
        if (_stats["hitPoints"] < 10 && Random.value > 0.8f)
        {
            Flee();
        }
        else
        {
            attackEvent.Invoke();
        }
    }

    private void Flee()
    {
        gameObject.SetActive(false);
    }
}
