using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiroNubes : MonoBehaviour
{
    public float vel = 0.002f;

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.Rotate(0.0f, vel, 0.0f, Space.Self);
    }
}
