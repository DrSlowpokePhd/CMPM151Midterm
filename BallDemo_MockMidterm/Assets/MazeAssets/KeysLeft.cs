using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeysLeft : MonoBehaviour
{
    public Text KLtext;
    GameController gc;
    // Start is called before the first frame update
    void Start()
    {
        gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }

    // Update is called once per frame
    void Update()
    {

        //Debug.Log(gc.count);
        if (gc.count == 2) {
            KLtext.text = "Keys Left : 2";
        }
        else if (gc.count == 1)
        {
            KLtext.text = "Keys Left : 1";
        }
        else {
            KLtext.text = "Keys Left : 0";
        }

    }
}
