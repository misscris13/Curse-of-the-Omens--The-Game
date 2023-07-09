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
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private GameObject varenGo;
    [SerializeField] private GameObject thug2Go;

    // Start is called before the first frame update
    void Start()
    {
        // Starts with dialogue
        dialogueManager.GetDialogueFromFile("dialogueKayVaren1");
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

    public void TriggerVarenOut()
    {
        Debug.Log("Varen hides in bushes");
        sfxSource.Play();
        varenGo.SetActive(false);
    }
    
    public void TriggerVarenThugRun()
    {
        Debug.Log("Varen and Thug run");
        varenGo.GetComponent<Transform>().position = thug2Go.GetComponent<Transform>().position - new Vector3(1.5f, 0, 0);
        sfxSource.Play();
        varenGo.SetActive(true);
        varenGo.GetComponent<Animator>().Play("ChaseThug");
        thug2Go.GetComponent<Animator>().Play("RunFromVaren");
    }
}
