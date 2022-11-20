using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadBob : MonoBehaviour {
    
    Vector3 startPos;
    public float amplitude = 0.7f;
    public float period = 3f;
    
    protected void Start() {
        startPos = transform.position;
    }
    
    protected void Update() {
        float theta = Time.timeSinceLevelLoad / period;
        float distance = amplitude * Mathf.Sin(theta);
        transform.position = startPos + Vector3.up * distance + Vector3.right * ((distance+2)/2);
    }
}
