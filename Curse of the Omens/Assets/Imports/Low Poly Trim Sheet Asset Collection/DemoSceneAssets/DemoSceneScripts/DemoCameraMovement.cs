using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Small script to pan the camera around the scene
/// </summary>
public class DemoCameraMovement : MonoBehaviour
{
    [SerializeField]
    float horizontalSpeed = 3.0f;

    [SerializeField]
    float verticalSpeed = 3.0f;

    void Update()
    {
        float horizontal = horizontalSpeed * Input.GetAxis("Horizontal") * Time.deltaTime;
        float vertical = verticalSpeed * Input.GetAxis("Vertical") * Time.deltaTime;
        transform.Translate(horizontal, 0, 0);
        transform.Translate(0, 0, vertical);
    }
}
