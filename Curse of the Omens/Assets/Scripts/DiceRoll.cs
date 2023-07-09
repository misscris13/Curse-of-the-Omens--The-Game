using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class DiceRoll : MonoBehaviour
{
    private Rigidbody rb;

    // Applies torque to 3d dice object
    public void StartRolling()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = false;
        rb.isKinematic = false;
        Vector3 force = new Vector3(500f, 300f, 500f);
        rb.AddTorque(force, ForceMode.Force);
    }
    
    // Gets rid of torque
    public void StopRolling()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        rb.isKinematic = true;
    }
}