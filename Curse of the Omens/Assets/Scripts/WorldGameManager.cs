using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGameManager : MonoBehaviour
{
    [SerializeField] private DialogueManager dialogueManager;
    
    // Start is called before the first frame update
    void Start()
    {
        // wait?
        dialogueManager.GetDialogueFromFile("dialogueKayVaren1");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
