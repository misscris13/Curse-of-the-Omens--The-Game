using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
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
        CreateKayData();  // this function will change depending on the selected character
        
        // Load first scene
        SceneManager.LoadScene("Scenes/World");
    }
    
    // Deletes every item in PlayerPrefs
    private void ResetData()
    {
        PlayerPrefs.DeleteAll();
    }

    public void Exit()
    {
        Application.Quit();
    }

    // Creates the character sheet for Kay Ravely.
    // Only required values for the tutorial
    // TODO: read from file
    private void CreateKayData()
    {
        // Since the default character is Kay, she is multiclassed in Fighter and Barbarian
        PlayerPrefs.SetString("class1", "fighter");
        PlayerPrefs.SetString("class2", "barbarian");
        
        // For the sake of the story, she starts with higher level
        PlayerPrefs.SetInt(PlayerPrefs.GetString("class1"), 10);
        PlayerPrefs.SetInt(PlayerPrefs.GetString("class2"), 7);
        
        // Stats like HP, AC, Proficiency Bonus, Initiative Bonus
        PlayerPrefs.SetInt("totalHitPoints", 127);
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
        
        // Stat mod = (int)((stat + 1) / 2) - 5 

        // Skill proficiencies - if exists, there's proficiency
        PlayerPrefs.SetInt("athletics", 1);
        PlayerPrefs.SetInt("intimidation", 1);
        PlayerPrefs.SetInt("perception", 1);
        PlayerPrefs.SetInt("stealth", 1);
        
        PlayerPrefs.Save();
    }
}
