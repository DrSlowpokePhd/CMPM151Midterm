using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateB : MonoBehaviour
{
    GameController gc;
    // Start is called before the first frame update
    void Start()
    {
        gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gc.count == 0)
        {
            transform.position = transform.position + new Vector3(0.0f, -0.05f, .0f);
            GameObject.Destroy(this.gameObject, 1.0f);
        }
    }
}
