using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Giro : MonoBehaviour {
    public GameObject c;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        c.transform.Rotate(Mathf.Sin(Time.frameCount/1000)/200, 0.04f, 0f, Space.Self);
        if (c.transform.eulerAngles.y >= 360f)
            c.transform.eulerAngles = new Vector3(c.transform.eulerAngles.x, 0f, c.transform.eulerAngles.z);
    }
}
