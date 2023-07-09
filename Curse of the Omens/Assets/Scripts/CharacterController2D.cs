using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.Events;

public class CharacterController2D : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private string _name;
    [SerializeField] private Entity _entity;
    
    private Vector3 _position;
    private string _lastDirection;
    [SerializeField] private float speed = 0.032f;

    public bool playerInput;

    // Start is called before the first frame update
    void Start()
    {
        _lastDirection = "f";
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInput && _entity.isPlayer)    // If player input is enabled
        {
            _position = GetComponent<Transform>().position;

            if (Input.GetButton("Horizontal") && Input.GetAxis("Horizontal") > 0.0f) // Right
            {
                WalkRight();
            }
            else if (Input.GetButton("Horizontal") && Input.GetAxis("Horizontal") < 0.0f) // Left
            {
                WalkLeft();
            }

            if (Input.GetButtonUp("Horizontal") && _lastDirection == "r")
            {
                IdleRight();
            }
            else if (Input.GetButtonUp("Horizontal") && _lastDirection == "l")
            {
                IdleLeft();
            }

            if (Input.GetButton("Vertical") && Input.GetAxis("Vertical") < 0.0f)    // Front
            {
                IdleFront();
            }
        }
    }

    public void WalkRight()
    {
        _animator.Play(_name + "WalkR");
        GetComponent<Transform>().position = new Vector3(_position.x + speed, _position.y, _position.z);
        _lastDirection = "r";
    }

    public void WalkLeft()
    {
        _animator.Play(_name + "WalkL");
        GetComponent<Transform>().position = new Vector3(_position.x - speed, _position.y, _position.z);
        _lastDirection = "l";
    }

    public void IdleFront()
    {
        _animator.Play(_name + "IdleF");
    }
    
    public void IdleRight()
    {
        _animator.Play(_name + "IdleR");
    }
    
    public void IdleLeft()
    {
        _animator.Play(_name + "IdleL");
    }
}