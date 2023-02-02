using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breathing : MonoBehaviour
{
    public float intensity = 1f;
    public float speed = 1f;

    private float initY;
    private float finalY;

    // Start is called before the first frame update
    void Start()
    {
        initY = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        finalY = initY + Mathf.Sin(Time.time * speed) * intensity;
        transform.position = new Vector3(transform.position.x, finalY, transform.position.z);
    }
}
