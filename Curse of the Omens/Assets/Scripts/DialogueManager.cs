using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public List<Tuple<string, string, string>> DialogueData;  // Contains the following format: 
    private int _numberOfDialogues;
    private bool _inDialogue;
    private int _currentIndex;
    
    [SerializeField] 
    private Image characterSpeaking;
    [SerializeField] 
    private TMP_Text nameText;
    [SerializeField] 
    private TMP_Text dialogueText;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_inDialogue)
        {
            if (Input.GetButtonDown(("Fire1")))
            {
                if (_currentIndex < _numberOfDialogues - 1)
                {
                    _currentIndex++;
                    ShowDialogue();
                }
                else
                {
                    HideDialogue();
                }
                
            }
        }
    }

    public void GetDialogueFromFile(string fileName)
    {
        DialogueData = new List<Tuple<string, string, string>>();
        
        string path = "Assets/Data/" + fileName + ".txt";   // build file path
        
        Debug.Log("Reading path...");
        StreamReader reader = new StreamReader(path);

        string line;
        string[] words;

        Debug.Log("Getting dialogues from file...");
        
        while ((line = reader.ReadLine()) != null) // while !eof
        {
            Debug.Log(line);
            words = line.Split(";");    // words = [name, text, emotion]
            Debug.Log(words[0] + " said '" + words[1] + "' with a " + words[2] + " expression");
            Tuple<string, string, string> tuple = new Tuple<string, string, string>(words[0], words[1], words[2]);
            DialogueData.Add(tuple);
        }

        _numberOfDialogues = DialogueData.Count;
        _currentIndex = 0;
        _inDialogue = true;
        // make everything visible
        ShowDialogue();
    }

    private void ShowDialogue()
    {
        Debug.Log("Showing dialogue...");
        
        Tuple<string, string, string> currentDialogue = DialogueData[_currentIndex];
        
        nameText.text = currentDialogue.Item1;
        dialogueText.text = currentDialogue.Item2;

        string path = "Assets/Sprites/Dialogues/" + currentDialogue.Item1 + currentDialogue.Item3 + ".png";
        Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path) as Sprite;
        characterSpeaking.sprite = sprite;
    }

    private void HideDialogue()
    {
        Debug.Log("Hiding dialogues...");
        _inDialogue = false;
    }
}

