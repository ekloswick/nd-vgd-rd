using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	
	public float speed;
	public float rotatespeed;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		//Moving the player based on the left stick
		float xmotion = Input.GetAxis ("L_XAxis_1");
		float zmotion = -1 * Input.GetAxis ("L_YAxis_1");

		float newx = transform.position.x + (xmotion * speed * Time.deltaTime);
		float newz = transform.position.z + (zmotion * speed * Time.deltaTime);

		Vector3 newpos = new Vector3 (newx, 0f, newz);
		transform.position = newpos;

		//Roatating the player based on right stick
		float lookx = Input.GetAxis ("R_XAxis_1");
		//float lookz = -1 * Input.GetAxis ("R_YAxis_1");

		float newrotate = Mathf.Sign (lookx) * rotatespeed * Time.deltaTime;

		if(Mathf.Abs (lookx) > 0.1){
			transform.Rotate (new Vector3(0f, newrotate, 0f));
		}


		//some test stuff
		/*
		float testturn = Input.GetAxis ("DPad_XAxis_1");
		float newrotate = testturn * rotatespeed * Time.deltaTime;
		transform.Rotate (0f, newrotate, 0f);
		*/




	}
}
