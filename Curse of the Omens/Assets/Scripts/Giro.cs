using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Giro : MonoBehaviour {
    public GameObject c;

    // Start is called before the first frame update
    void Start() {
        c = GameObject.Find("Nubes");
    }

    // Update is called once per frame
    void Update() {
        c.transform.Rotate(0f, 0.01f, 0f, Space.Self);
    }
}
