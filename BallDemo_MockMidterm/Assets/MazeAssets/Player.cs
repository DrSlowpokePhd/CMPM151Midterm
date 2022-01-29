using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 1.0f;
    public Camera cam1;
    public Camera cam2;
    // Start is called before the first frame update
    void Start()
    {
        cam1.enabled = false;
        cam2.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            cam1.enabled = !cam1.enabled;
            cam2.enabled = !cam2.enabled;
        }

        this.GetComponent<CharacterController>().SimpleMove(new Vector3(
            Input.GetAxis("Horizontal") * this.speed, 
            0.0f,
            Input.GetAxis("Vertical") * this.speed
        ));
    }

    void OnTriggerEnter(Collider other)
    {
        //Debug.Log("-------- COLLISION!!! ----------");

        if (other.gameObject.CompareTag("Pick Up"))
        {
            Debug.Log("-------- PICKED UP ----------");
        }
        else if (other.gameObject.CompareTag("Wall"))
        {
            Debug.Log("-------- HIT THE WALL ----------");
        }

    }
}
