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
        // Reset PlayerPrefs
        ResetData();
        
        // Write everything to PlayerPrefs
        
        // Since the default character is Kay, she is multiclassed in Fighter and Barbarian
        PlayerPrefs.SetString("class1", "fighter");
        PlayerPrefs.SetString("class2", "barbarian");
        // For the sake of the story, she starts with higher level
        PlayerPrefs.SetInt(PlayerPrefs.GetString("class1"), 10);
        PlayerPrefs.SetInt(PlayerPrefs.GetString("class2"), 7);
        // Default stats
        PlayerPrefs.SetInt("hitPoints", 127);
        PlayerPrefs.SetInt("str");
    }

    private void ResetData()
    {
        PlayerPrefs.DeleteAll();
    }
}
