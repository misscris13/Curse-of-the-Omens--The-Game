using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class UICombatManager : MonoBehaviour
{
    [SerializeField]
    private GameObject messages;
    private TMP_Text _text;

    private bool _rollingDice = false;
    private int _roll;
    [SerializeField]
    private TMP_Text rollTMP;

    [SerializeField]
    private UnityEvent<int> initiativeRolled;

    // Start is called before the first frame update
    void Start()
    {
        messages.SetActive(true);
        _text = messages.GetComponent<TMP_Text>();
        _text.text = "Tira iniciativa";
        StartCoroutine(HideText(3.0f));

        _roll = 1;
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

    private IEnumerator HideText(float time)
    {
        yield return new WaitForSeconds(time);
        
        messages.SetActive(false);
    }
    
    public void ChangeMessage(string msg)
    {
        
    }

    public void RollDice()
    {
        _rollingDice = true;
    }
}
