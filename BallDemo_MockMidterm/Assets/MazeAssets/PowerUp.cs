using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    bool disappearing = false;
    GameController gc;
    // Start is called before the first frame update
    void Start()
    {
        gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        gc.count++;
    }

    // Update is called once per frame
    void Update()
    {
        var transform = this.GetComponent<Transform>();
        var local_rotation = transform.localRotation;
        local_rotation = local_rotation * Quaternion.Euler(0.0f, 0.5f, 0.0f);
        transform.localRotation = local_rotation;

        if (this.disappearing) {
            local_rotation = local_rotation * Quaternion.Euler(2.0f, 2.0f, 2.0f);
            transform.localRotation = local_rotation;
            transform.position = transform.position + new Vector3(0.0f, 0.05f, .0f);
        }
    }

    private void OnTriggerEnter(Collider collider) {
        //Debug.Log("Collision Detected");
        var player = collider.GetComponent<Player>();

        if (player != null) {
            GameObject.Destroy(this.gameObject, 1.0f);
            this.disappearing = true;
            gc.count--;
        }
    }
}
