using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterController2D : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Rigidbody2D _rigidbody;

    private Vector3 _position;
    private string _lastDirection;
    private float speed = 0.032f;

    // Start is called before the first frame update
    void Start()
    {
        string _lastDirection = "f";
    }

    // Update is called once per frame
    void Update()
    {
        _position = GetComponent<Transform>().position;
        
        if (Input.GetButton("Horizontal") && Input.GetAxis("Horizontal") > 0.0f) // Right
        {
            _animator.Play("KayWalkR");
            GetComponent<Transform>().position = new Vector3(_position.x + speed, _position.y, _position.z);
            _lastDirection = "r";
        }
        else if (Input.GetButton("Horizontal") && Input.GetAxis("Horizontal") < 0.0f)    // Left
        {
            _animator.Play("KayWalkL");
            GetComponent<Transform>().position = new Vector3(_position.x - speed, _position.y, _position.z);
            _lastDirection = "l";
        }
        
        if (Input.GetButtonUp("Horizontal") && _lastDirection == "r")
        {
            _animator.Play("KayIdleR");
        }
        else if (Input.GetButtonUp("Horizontal") && _lastDirection == "l")
        {
            _animator.Play("KayIdleL");
        }
    }
}