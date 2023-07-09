using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGameManager : MonoBehaviour
{
    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] private GameObject player;
    [SerializeField] private Transform talkingPosition;
    [SerializeField] private Transform walkingPosition;
    [SerializeField] private CharacterController2D _characterController;

    // Start is called before the first frame update
    void Start()
    {
        // Starts with dialogue
        dialogueManager.GetDialogueFromFile("dialogueKayVaren1");
        // Move player to dialogue position
        // player.GetComponent<Transform>().position = talkingPosition.position;
        // Disable player input
        _characterController.playerInput = false;
    }

    public void CanStartWalking()
    {
        // Move player to walking position
        Debug.Log(player.GetComponent<Transform>().position);
        player.GetComponent<Transform>().position = walkingPosition.position;
        Debug.Log("moved player to walking position " + player.GetComponent<Transform>().position);
        // Enable player input
        _characterController.playerInput = true;
    }
    
    public void CantMove()
    {
        // Disable player input
        _characterController.playerInput = false;
    }
}
