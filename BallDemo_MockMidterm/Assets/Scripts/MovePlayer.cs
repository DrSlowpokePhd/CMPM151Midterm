using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//************** use UnityOSC namespace...
using UnityOSC;
//*************

public class MovePlayer : MonoBehaviour {

	public float speed;
	public Text countText;

	private Rigidbody rb;
	private int count;
	private bool cubeSound = false;
	private float cubeSFXTime = 0;

	//************* Need to setup this server dictionary...
	Dictionary<string, ServerLog> servers = new Dictionary<string, ServerLog> ();
	//*************

	// Use this for initialization
	void Start () 
	{
		Application.runInBackground = true; //allows unity to update when not in focus

		//************* Instantiate the OSC Handler...
	    OSCHandler.Instance.Init ();
		OSCHandler.Instance.SendMessageToClient ("pd", "/unity/trigger", "ready");
		OSCHandler.Instance.SendMessageToClient ("pd", "/unity/playseq", "ready");
        //*************

        rb = GetComponent<Rigidbody> ();
		count = 0;
		setCountText ();
	}

	void Update() 
	{
		// update cubeSFXTime
		if (cubeSFXTime > 0) {
			cubeSFXTime -= Time.deltaTime;
		}
		
		if (cubeSFXTime < 0) {
			cubeSFXTime = 0f;
		}

		if (cubeSFXTime <= 0 && (Input.GetKey("w") || Input.GetKey("a") || Input.GetKey("s") || Input.GetKey("d"))) { 
			OSCHandler.Instance.SendMessageToClient("pd", "/unity/playseq", 1);
		}
		else
		{
			OSCHandler.Instance.SendMessageToClient("pd", "/unity/playseq", 0);
		}

		checkEnd();
	}

	void FixedUpdate()
	{
		/*float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");

		//Debug.Log(rb.position.x);

		Vector3 movement = new Vector3 (moveHorizontal, 0, moveVertical);

		rb.AddForce (movement*speed);*/

		this.GetComponent<CharacterController>().SimpleMove(new Vector3(
			Input.GetAxis("Horizontal") * this.speed,
			0.0f,
			Input.GetAxis("Vertical") * this.speed
		));

		//************* Routine for receiving the OSC...
		OSCHandler.Instance.UpdateLogs();
		Dictionary<string, ServerLog> servers = new Dictionary<string, ServerLog>();
		servers = OSCHandler.Instance.Servers;

		foreach (KeyValuePair<string, ServerLog> item in servers) {
			// If we have received at least one packet,
			// show the last received from the log in the Debug console
			if (item.Value.log.Count > 0) {
				int lastPacketIndex = item.Value.packets.Count - 1;

				//get address and data packet
				countText.text = item.Value.packets [lastPacketIndex].Address.ToString ();
				countText.text += item.Value.packets [lastPacketIndex].Data [0].ToString ();

			}
		}
		//*************
	}
		

	void OnTriggerEnter(Collider other) 
    {
        //Debug.Log("-------- COLLISION!!! ----------");

        if (other.gameObject.CompareTag ("Pick Up")) 
		{
			cubeSound = true;
			cubeSFXTime = 3.9f;
			other.gameObject.SetActive (false);
			count = count + 1;
			setCountText ();

			/*// change the tempo of the sequence based on how many obejcts we have picked up.
            if(count < 2)
            {
                OSCHandler.Instance.SendMessageToClient("pd", "/unity/tempo", 500);
            }
            if (count < 4)
            {
                OSCHandler.Instance.SendMessageToClient("pd", "/unity/tempo", 400);
            }
            else if(count < 6)
            {
                OSCHandler.Instance.SendMessageToClient("pd", "/unity/tempo", 300);
            }
            else if (count < 8)
            {
                OSCHandler.Instance.SendMessageToClient("pd", "/unity/tempo", 150);
            }
            else
            {
                OSCHandler.Instance.SendMessageToClient("pd", "/unity/playseq", 0);
            }
			*/
		}
        else if(other.gameObject.CompareTag("Wall"))
        {
            Debug.Log("-------- HIT THE WALL ----------");
            // trigger noise burst whe hitting a wall.
            OSCHandler.Instance.SendMessageToClient("pd", "/unity/colwall", 1);
        }

    }

	void setCountText()
	{
		countText.text = "Count: " + count.ToString ();

		//************* Send the message to the client...
		OSCHandler.Instance.SendMessageToClient ("pd", "/unity/trigger", 1);
		//*************
	}

	void checkEnd()
	{
		// get position of the player
		float x = transform.position.x;
		float z = transform.position.z;
		
		// check if player is close to exit
		if(z < 5f && x < -5.5f)
		{
            OSCHandler.Instance.SendMessageToClient("pd", "/unity/tempo", 100);
			OSCHandler.Instance.SendMessageToClient("pd", "/unity/end", 1);

			if((x > -11f && (x < -6)) && ((z < -5f) && (z > -11f)))
			{
				OSCHandler.Instance.SendMessageToClient("pd", "/unity/tempo", 300);
				OSCHandler.Instance.SendMessageToClient("pd", "/unity/end", 1);
				Debug.Log("here");
			}
		
		} else
		{
			OSCHandler.Instance.SendMessageToClient("pd", "/unity/end", 0);
		}
	}
		
}
