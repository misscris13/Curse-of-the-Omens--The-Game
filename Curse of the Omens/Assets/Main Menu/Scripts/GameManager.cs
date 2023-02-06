using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.PlayerLoop;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private bool existsGame;
    
    [SerializeField]
    private GameObject continueText;
    [SerializeField]
    private GameObject continueButton;
    
    // Start is called before the first frame update
    void Start()
    {
        // Check PlayerPrefs for existing game
        existsGame = false; // testing
        TextMeshProUGUI continueTMP = this.continueText.GetComponent<TextMeshProUGUI>();

        if (existsGame)
        {
            // Set continue as grey
            continueTMP.color = new Color(1f, 1f, 1f, 1f);
            // Disable events
            continueButton.GetComponent<EventTrigger>().enabled = true;
            continueButton.GetComponent<Button>().interactable = true;
        }
        else
        {
            // Set continue as white
            continueTMP.color = new Color(1f, 1f, 1f, 0.5f);
            // Able events
            continueButton.GetComponent<EventTrigger>().enabled = false;
            continueButton.GetComponent<Button>().interactable = false;
        }
    }

    public void NewGame()
    {
        // Reset PlayerPrefs since it's a new game
        ResetData();
        
        // Write everything to PlayerPrefs
        CreateKayData();  // this function will change depending on the selected character, only Kay for now
    }
    
    private void ResetData()
    {
        PlayerPrefs.DeleteAll();
    }

    private void CreateKayData()
    {
        // Since the default character is Kay, she is multiclassed in Fighter and Barbarian
        PlayerPrefs.SetString("class1", "fighter");
        PlayerPrefs.SetString("class2", "barbarian");
        
        // For the sake of the story, she starts with higher level
        PlayerPrefs.SetInt(PlayerPrefs.GetString("class1"), 10);
        PlayerPrefs.SetInt(PlayerPrefs.GetString("class2"), 7);
        
        // Stats like HP, AC, Proficiency Bonus, Initiative Bonus
        PlayerPrefs.SetInt("hitPoints", 127);
        PlayerPrefs.SetInt("armorClass", 24);
        PlayerPrefs.SetInt("profBonus", 6);
        PlayerPrefs.SetInt("initiativeBonus", 6);
        
        // STR, DEX, CON, INT, WIS, CHA
        PlayerPrefs.SetInt("str", 20);
        PlayerPrefs.SetInt("dex", 17);
        PlayerPrefs.SetInt("con", 13);
        PlayerPrefs.SetInt("int", 10);
        PlayerPrefs.SetInt("wis", 13);
        PlayerPrefs.SetInt("cha", 15);
        
        // Saving throws bonuses - 1 = proficiency, 0.5 = half-proficiency
        PlayerPrefs.SetFloat("strSaving", 1f);
        PlayerPrefs.SetFloat("dexSaving", 0f);
        PlayerPrefs.SetFloat("conSaving", 1f);
        PlayerPrefs.SetFloat("intSaving", 0f);
        PlayerPrefs.SetFloat("wisSaving", 0f);
        PlayerPrefs.SetFloat("chaSaving", 0f);
        
        // Skill proficiencies - 1 = proficiency, 0.5 = half-proficiency
        PlayerPrefs.SetFloat("acrobatics", 1f);
        PlayerPrefs.SetFloat("animalHandling", 0f);
        PlayerPrefs.SetFloat("arcana", 0f);
        PlayerPrefs.SetFloat("athletics", 1f);
        PlayerPrefs.SetFloat("deception", 0f);
        PlayerPrefs.SetFloat("history", 0f);
        PlayerPrefs.SetFloat("insight", 0f);
        PlayerPrefs.SetFloat("intimidation", 1f);
        PlayerPrefs.SetFloat("investigation", 0f);
        PlayerPrefs.SetFloat("medicine", 0f);
        PlayerPrefs.SetFloat("nature", 0f);
        PlayerPrefs.SetFloat("perception", 1f);
        PlayerPrefs.SetFloat("performance", 0f);
        PlayerPrefs.SetFloat("persuasion", 0f);
        PlayerPrefs.SetFloat("religion", 0f);
        PlayerPrefs.SetFloat("sleightOfHand", 0.5f);
        PlayerPrefs.SetFloat("stealth", 1f);
        PlayerPrefs.SetFloat("survival", 1f);
        
        // Other proficiencies
        PlayerPrefs.SetString("armorProficiency", "heavy;light;medium;shields");
        PlayerPrefs.SetString("weaponProficiency", "martial;simple");
        PlayerPrefs.SetString("spokenLanguages", "common;elvish;sylvan");
        
        // Defenses & Conditions
        PlayerPrefs.SetString("resistances", "");
        PlayerPrefs.SetString("immunities", "");
        PlayerPrefs.SetString("vulnerabilities", "");
        PlayerPrefs.SetString("conditions", "");
        
        // Senses
        PlayerPrefs.SetString("senses", "darkvision");
    }
}
