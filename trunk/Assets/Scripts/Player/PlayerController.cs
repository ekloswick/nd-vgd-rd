using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	
	public float speed;
	public float rotatespeed;

	private string L_XAxisname;
	private string L_YAxisname;
	private string R_XAxisname;
	private string R_YAxisname;
	private string Triggers_name;
	private string DPad_YAxis_name;

	private bool blockup;
	private bool blockdown;
	private bool blocking;
	private bool attacking;
	private bool walking;

	private float modspeed;

	// Use this for initialization
	void Start () {

		blocking = false;
		attacking = false;
		walking = false;

		animation.GetClip ("Walk").wrapMode = WrapMode.Loop;
		animation.GetClip ("Idle").wrapMode = WrapMode.Once;
		animation.GetClip ("Attack").wrapMode = WrapMode.Once;
		animation.GetClip ("BlockUp").wrapMode = WrapMode.Once;
		animation.GetClip ("BlockDown").wrapMode = WrapMode.Once;

		if(Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor){
			L_XAxisname = "L_XAxis_Win";
			L_YAxisname = "L_YAxis_Win";
			R_XAxisname = "R_XAxis_Win";
			R_YAxisname = "R_YAxis_Win";
			DPad_YAxis_name = "DPad_YAxis_Win";
			Triggers_name = "Triggers_Win";
		}

		if(Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.OSXEditor){
			L_XAxisname = "L_XAxis_OSX";
			L_YAxisname = "L_YAxis_OSX";
			R_XAxisname = "R_XAxis_OSX";
			R_YAxisname = "R_YAxis_OSX";
			DPad_YAxis_name = "DPad_YAxis_OSX";
			Triggers_name = "Triggers_OSX";
		}

		rigidbody.freezeRotation = true;

		animation.Play ("Idle");
	
	}

	void Update(){
		float triggerval = Input.GetAxis (Triggers_name);

		if(triggerval < -0.9 && !blocking && !blockup && !blockdown && !attacking){
			animation["Attack"].speed = 2f;
			animation.Play ("Attack");
			attacking = true;
		}

		if(attacking && !animation.IsPlaying("Attack")){
			attacking = false;
		}

		if(triggerval > 0.9 && !blocking && !blockup && !blockdown && !attacking){
			animation.Play ("BlockUp");
			blockup = true;
		}

		if(blockup && !animation.IsPlaying("BlockUp")){
			blockup = false;
			blocking = true;
		}
		
		// alternate, non-Xbox controller controls
		if (Input.GetKeyUp (KeyCode.Mouse1) && blocking)
		{
			animation.Play ("BlockDown");
			blocking = false;
			blockdown = true;
		}
		else if (!Input.GetKey (KeyCode.Mouse1) && triggerval < 0.1 && blocking){
			animation.Play ("BlockDown");
			blocking = false;
			blockdown = true;
		}

		if(!animation.IsPlaying("BlockDown")){
			blockdown = false;
		}

		// alternate, non-Xbox controller controls
		if (Input.GetKeyDown (KeyCode.Mouse0))
		{
			animation.Play("Attack");
			attacking = true;
		}
		else if (Input.GetKeyDown (KeyCode.Mouse1))
		{
			animation.Play ("BlockUp");
			blockup = true;
		}

	}
	
	// Update is called once per frame
	void FixedUpdate () {

		if(blockup || blocking || blockdown){
			modspeed = speed * 0.5f;
		} else {
			modspeed = speed;
		}
	
		//Moving the player based on the left stick
		float xinput = Input.GetAxis (L_XAxisname);
		float zinput = -1 * Input.GetAxis (L_YAxisname);
		
		// alternate, non-Xbox controller controls
		if (Input.GetKey (KeyCode.W))
			zinput = 1.0f;
		else if (Input.GetKey (KeyCode.S))
			zinput = -1.0f;
		
		float playerang = transform.eulerAngles.y;

		float xmotion1 = xinput * Mathf.Sin ((playerang + 90) * Mathf.Deg2Rad);
		float zmotion1 = xinput * Mathf.Cos ((playerang + 90) * Mathf.Deg2Rad);

		float xmotion2 = zinput * Mathf.Sin (playerang * Mathf.Deg2Rad);
		float zmotion2 = zinput * Mathf.Cos (playerang * Mathf.Deg2Rad);

		float xmotion = xmotion1 + xmotion2;
		float zmotion = zmotion1 + zmotion2;

		Vector3 newmotion = new Vector3 (xmotion, 0f, zmotion);

		
		rigidbody.MovePosition (transform.position + (modspeed * newmotion * Time.deltaTime));

		//Roatating the player based on right stick
		float lookx = Input.GetAxis (R_XAxisname);
		//float lookz = -1 * Input.GetAxis ("R_YAxis_1");

		float newrotate = Mathf.Sign (lookx) * rotatespeed * Time.deltaTime;

		if(Mathf.Abs (lookx) > 0.1){
			transform.Rotate (new Vector3(0f, newrotate, 0f));
		}
		
		// alternate, non-Xbox controller controls
		if (Input.GetKey (KeyCode.A))
			transform.Rotate (new Vector3(0f, -1.0f * rotatespeed * Time.deltaTime, 0f));
		else if (Input.GetKey(KeyCode.D))
			transform.Rotate (new Vector3(0f, 1.0f * rotatespeed * Time.deltaTime, 0f));
		
		//some test stuff
		/*
		float testturn = Input.GetAxis ("DPad_XAxis_1");
		float newrotate = testturn * rotatespeed * Time.deltaTime;
		transform.Rotate (0f, newrotate, 0f);
		*/


		//Vector3 ppos = transform.position;
		//print ("Player position: " + ppos.x + ", " + ppos.y + ", " + ppos.z);



	}
}
