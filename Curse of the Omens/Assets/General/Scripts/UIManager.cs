using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private bool rollingDice = false;
    [SerializeField]
    private TMP_Text text;
    
    // Start is called before the first frame update
    void Start()
    {
        text.text = "Tira iniciativa";
        HideText(3.0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (rollingDice)
        {
            
        }
    }

    private void HideText(float time)
    {
        
    }
    
    public void ChangeMessage(string msg)
    {
        
    }

    public void RollDice()
    {
        rollingDice = true;
    }
}
