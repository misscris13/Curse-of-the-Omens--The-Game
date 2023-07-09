using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
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
    [SerializeField] 
    private AudioSource sfxSource;
    [SerializeField] 
    private GameObject varenGo;

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
            
            if (_dialogueAnimation && _charIndex < currentDialogue.Item2.Length)
            {
                dialogueText.text += currentDialogue.Item2[_charIndex];
                _charIndex++;
            }
        }
    }

    public void GetDialogueFromFile(string fileName)
    {
        DialogueData = new List<Tuple<string, string, string>>();

        _fileName = fileName;
        
        string path = "Assets/Data/" + fileName + ".txt";   // build file path
        
        Debug.Log("Reading path...");
        StreamReader reader = new StreamReader(path);

        string line;
        string[] words;

        Debug.Log("Getting dialogues from file...");
        
        while ((line = reader.ReadLine()) != null) // while !eof
        {
            words = line.Split(";");    // words = [name, text, emotion]

            if (words.Length == 3)
            {
                Tuple<string, string, string> tuple = new Tuple<string, string, string>(words[0], words[1], words[2]);
                DialogueData.Add(tuple);
            }
        }

        _numberOfDialogues = DialogueData.Count;
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
        
        currentDialogue = DialogueData[_currentIndex];
        
        nameText.text = currentDialogue.Item1;

        dialogueText.text = "";
        
        _dialogueAnimation = true;
        _charIndex = 0;

        string path = "Assets/Sprites/Dialogues/" + currentDialogue.Item1 + currentDialogue.Item3 + ".png";
        Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path) as Sprite;
        characterSpeaking.sprite = sprite;
    }

    private void HideDialogue()
    {
        Debug.Log("Hiding dialogues...");
        
        panel.SetActive(false);

        gameManager.CanStartWalking();
        
        _inDialogue = false;

        if (_fileName == "dialogueKayVaren1")
        {
            sfxSource.Play();
            varenGo.SetActive(false);
        }
    }

    private void LoadCombat()
    {
        // Load Combat scene
        SceneManager.LoadScene("Scenes/Combat");
    }
}

