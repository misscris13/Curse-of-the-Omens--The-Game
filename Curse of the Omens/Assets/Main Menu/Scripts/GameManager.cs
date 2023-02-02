using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
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
            continueButton.GetComponent<EventTrigger>().enabled = false;
        }
        else
        {
            // Set continue as white
            continueTMP.color = new Color(1f, 1f, 1f, 0.5f);
            // Able events
            continueButton.GetComponent<EventTrigger>().enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
