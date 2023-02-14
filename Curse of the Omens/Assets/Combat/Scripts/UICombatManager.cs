using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class UICombatManager : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _message;

    private bool _rollingDice = false;
    private int _roll;
    [SerializeField]
    private TMP_Text rollTMP;

    private TMP_Text _hpText;
    
    [SerializeField]
    private UnityEvent<int> initiativeRolled;

    // Start is called before the first frame update
    void Start()
    {
        _message = GameObject.Find("Messages").GetComponent<TMP_Text>();
        ChangeMessage("Tira iniciativa");

        _roll = 1;
        
        _hpText = GameObject.Find("HP").GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_rollingDice)
        {
            _roll = Random.Range(1, 20);
            rollTMP.text = "" + _roll;

            if (Input.GetButtonDown("Fire1"))
            {
                _rollingDice = false;
                initiativeRolled.Invoke(_roll);
            }
        }
    }

    // Changes the player's HP text.
    public void SetHP(int txt)
    {
        _hpText.text = "" + txt;
    }
    
    // Hides the "messages" text after "time" seconds.
    private IEnumerator HideText(float time)
    {
        yield return new WaitForSeconds(time);

        _message.text = "";
    }
    
    // Changes the "messages" text and calls HideText with 3s.
    public void ChangeMessage(string msg)
    {
        _message.text = msg;
        StartCoroutine(HideText(3.0f));
    }

    // Sets _rollingDice to true.
    public void RollDice()
    {
        _rollingDice = true;
    }

    // Displays the Entity order on screen.
    public void ShowOrder(List<Tuple<int,Entity>> list)
    {
        
    }
}
