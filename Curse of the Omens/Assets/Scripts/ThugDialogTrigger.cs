using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThugDialogTrigger : MonoBehaviour
{
    [SerializeField] 
    private DialogueManager _dialogueManager;

    [SerializeField] 
    private CharacterController2D _character;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Entering trigger...");
        
        if (other.CompareTag("Player"))
        {
            _dialogueManager.GetDialogueFromFile("dialogueThugsForest1");
            _character.IdleRight();
        }
    }
}