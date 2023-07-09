using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Debug = UnityEngine.Debug;
using Image = UnityEngine.UI.Image;

public class DialogueManager : MonoBehaviour
{
    public List<Tuple<string, string, string>> DialogueData;                // Contains the following format: <Name, Text, Expression>
    public List<Tuple<string, string, string, string>> AltDialogueData;     // Contains the following format: <Name, Picture name, Text, Expression>
    private int _numberOfDialogues;
    private bool _inDialogue;
    private int _currentIndex;
    private string _fileName;
    private bool _dialogueAnimation;
    private Tuple<string, string, string> currentDialogue;
    private Tuple<string, string, string, string> altCurrentDialogue;
    private int _charIndex;
    private bool altDialogue;
    private Dictionary<string, Sprite> spriteDictionary;

    [SerializeField] 
    private WorldGameManager gameManager;
    [SerializeField] 
    private GameObject panel;
    [SerializeField] 
    private Image characterSpeaking;
    [SerializeField] 
    private TMP_Text nameText;
    [SerializeField] 
    private TMP_Text dialogueText;
    [SerializeField] 
    private Animator animator;

    // void Awake()
    // {
    //     spriteDictionary = new Dictionary<string, Sprite>();
    //     
    //     spriteDictionary.Add("KayNeutral", Resources.Load<Sprite>("Sprites/Dialogues/KayNeutral"));
    //     spriteDictionary.Add("VarenNeutral", Resources.Load<Sprite>("Sprites/Dialogues/VarenNeutral"));
    //     // spriteDictionary.Add("Thug1Neutral", Resources.Load<Sprite>("Sprites/Dialogues/Thug1Neutral"));
    //     // spriteDictionary.Add("Thug2Neutral", Resources.Load<Sprite>("Sprites/Dialogues/Thug2Neutral"));
    // }
    
    // Update is called once per frame
    void Update()
    {
        if (_inDialogue)
        {
            if (Input.GetButtonDown(("Fire1")))
            {
                _dialogueAnimation = false;
                if (_currentIndex < _numberOfDialogues - 1)
                {
                    _currentIndex++;
                    NextDialogue();
                }
                else
                {
                    // fade to black
                    animator.Play("Fade");
                    Invoke("HideDialogue", 1f);

                    if (_fileName.Contains("Thug"))
                    {
                        Invoke("LoadCombat", 1f);
                    }
                }
            }

            if (!altDialogue)
            {
                if (_dialogueAnimation && _charIndex < currentDialogue.Item2.Length)
                {
                    dialogueText.text += currentDialogue.Item2[_charIndex];
                    _charIndex++;
                }
            }
            else
            {
                if (_dialogueAnimation && _charIndex < altCurrentDialogue.Item3.Length)
                {
                    dialogueText.text += altCurrentDialogue.Item3[_charIndex];
                    _charIndex++;
                }
            }
        }
    }

    public void GetDialogueFromFile(string fileName)
    {
        DialogueData = new List<Tuple<string, string, string>>();
        AltDialogueData = new List<Tuple<string, string, string, string>>();

        Resources.UnloadUnusedAssets();

        spriteDictionary = new Dictionary<string, Sprite>();

        _fileName = fileName;
        
        string path = "Data/" + fileName;   // build file path
        
        Debug.Log("Reading path...");
        TextAsset textAsset = Resources.Load<TextAsset>(path);
        
        string[] lines;
        string[] words;

        lines = textAsset.text.Split("\n");
        
        Debug.Log("Getting dialogues from file...");

        string spritePath = "";

        foreach (string line in lines)
        {
            words = line.Split(";");    // words = [name, text, emotion]

            if (words.Length == 3)
            {
                words[2] = words[2].Substring(0, words[2].Length - 1); // for some reason it's longer than expected
                
                Tuple<string, string, string> tuple = new Tuple<string, string, string>(words[0], words[1], words[2]);
                DialogueData.Add(tuple);
                _numberOfDialogues = DialogueData.Count;
                altDialogue = false;

                if (!spriteDictionary.ContainsKey(tuple.Item1 + tuple.Item3))
                {
                    spritePath = "Sprites/Dialogues/" + tuple.Item1 + tuple.Item3;
                    Debug.Log("'" + spritePath + "' = '" + "Sprites/Dialogues/KayNeutral'");
                    Debug.Log(spritePath == "Sprites/Dialogues/KayNeutral");
                    spriteDictionary.Add(tuple.Item1 + tuple.Item3, Resources.Load<Sprite>(spritePath));
                    Debug.Log(tuple.Item1 + tuple.Item3);
                    Debug.Log(spriteDictionary[tuple.Item1 + tuple.Item3]);
                }
            }
            else if (words.Length == 4)
            {
                words[3] = words[3].Substring(0, words[3].Length - 1); // for some reason it's longer than expected
                
                Tuple<string, string, string, string> tuple =
                    new Tuple<string, string, string, string>(words[0], words[1], words[2], words[3]);
                AltDialogueData.Add(tuple);
                _numberOfDialogues = AltDialogueData.Count;
                altDialogue = true;

                if (!spriteDictionary.ContainsKey(tuple.Item2 + tuple.Item4))
                {
                    spritePath = "Sprites/Dialogues/" + tuple.Item2 + tuple.Item4;
                    spriteDictionary.Add(tuple.Item2 + tuple.Item4, Resources.Load<Sprite>(spritePath));
                    Debug.Log(tuple.Item2 + tuple.Item4);
                    Debug.Log(spriteDictionary[tuple.Item2 + tuple.Item4]);
                }
            }
        }

        _currentIndex = 0;
        _inDialogue = true;

        animator.Play("Nothing");
        gameManager.CantMove();
        
        // make everything visible
        NextDialogue();
    }

    private void NextDialogue()
    {
        panel.SetActive(true);
        
        dialogueText.text = "";
        
        _dialogueAnimation = true;
        _charIndex = 0;

        // string path = "";
        
        if (!altDialogue)
        {
            currentDialogue = DialogueData[_currentIndex];
            
            nameText.text = currentDialogue.Item1;
            
            // path = "Sprites/Dialogues/" + currentDialogue.Item1 + currentDialogue.Item3;
            // Debug.Log(currentDialogue.Item1 + currentDialogue.Item3 + " " + (currentDialogue.Item1 + currentDialogue.Item3).Equals("KayNeutral") + " estupido");
            // Debug.Log("tiene key a mano: " + spriteDictionary.ContainsKey("VarenNeutral"));
            // Debug.Log("tiene key auto: " + spriteDictionary.ContainsKey(currentDialogue.Item1 + currentDialogue.Item3));
            // Debug.Log("Items: " + (currentDialogue.Item3 == "Neutral"));
            // Debug.Log("'" + currentDialogue.Item3 + "' - 'Neutral'");
            // Debug.Log(currentDialogue.Item3.Length);
            characterSpeaking.sprite = spriteDictionary[currentDialogue.Item1 + currentDialogue.Item3];
        }
        else
        {
            altCurrentDialogue = AltDialogueData[_currentIndex];

            nameText.text = altCurrentDialogue.Item1;

            if (altCurrentDialogue.Item1 == "Varen")
            {
                gameManager.TriggerVarenThugRun();
            }
            
            characterSpeaking.sprite = spriteDictionary[altCurrentDialogue.Item2 + altCurrentDialogue.Item4];
        }
        
        // Debug.Log("Searching for sprite at " + path);
        // Sprite sprite = Resources.Load(path) as Sprite;
        // if (sprite != null)
        // {
        //     Debug.Log("Found");
        // }
        // characterSpeaking.sprite = sprite;
        // Debug.Log("Changed");
        
    }

    private void HideDialogue()
    {
        Debug.Log("Hiding dialogues...");
        
        panel.SetActive(false);

        gameManager.CanStartWalking();
        
        _inDialogue = false;

        if (_fileName == "dialogueKayVaren1")
        {
            gameManager.TriggerVarenOut();
            // sfxSource.Play();
            // varenGo.SetActive(false);
        }
    }

    private void LoadCombat()
    {
        // Load Combat scene
        SceneManager.LoadScene("Scenes/Combat");
    }
}

