using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	
	public float speed;
	public float rotatespeed;

	public string L_XAxisname;
	public string L_YAxisname;
	public string R_XAxisname;
	public string R_YAxisname;
	public string Triggers_name;

	// Use this for initialization
	void Start () {

		if(Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor){
			L_XAxisname = "L_XAxis_Win";
			L_YAxisname = "L_YAxis_Win";
			R_XAxisname = "R_XAxis_Win";
			R_YAxisname = "R_YAxis_Win";
		}

		if(Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.OSXEditor){
			L_XAxisname = "L_XAxis_OSX";
			L_YAxisname = "L_YAxis_OSX";
			R_XAxisname = "R_XAxis_OSX";
			R_YAxisname = "R_YAxis_OSX";
		}
	
	}
	
	// Update is called once per frame
	void Update () {
	
		//Moving the player based on the left stick
		float xinput = Input.GetAxis (L_XAxisname);
		float zinput = -1 * Input.GetAxis (L_YAxisname);
		float playerang = transform.eulerAngles.y;

		float xmotion1 = xinput * speed * Mathf.Sin ((playerang + 90) * Mathf.Deg2Rad) * Time.deltaTime;
		float zmotion1 = xinput * speed * Mathf.Cos ((playerang + 90) * Mathf.Deg2Rad) * Time.deltaTime;

		float xmotion2 = zinput * speed * Mathf.Sin (playerang * Mathf.Deg2Rad) * Time.deltaTime;
		float zmotion2 = zinput * speed * Mathf.Cos (playerang * Mathf.Deg2Rad) * Time.deltaTime;

		float newx = transform.position.x + xmotion1 + xmotion2;
		float newz = transform.position.z + zmotion1 + zmotion2;

		//float newx = transform.position.x + (xmotion * speed * Time.deltaTime);
		//float newz = transform.position.z + (zmotion * speed * Time.deltaTime);

		transform.position = new Vector3 (newx, 0f, newz);

		//Roatating the player based on right stick
		float lookx = Input.GetAxis (R_XAxisname);
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
