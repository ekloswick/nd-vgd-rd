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

	private float modspeed;
	private float lookangle;

	private Animator myAnim;

	// Use this for initialization
	void Start () {

		myAnim = GetComponent<Animator> ();
		myAnim.SetBool("blocking", false);
		myAnim.SetBool ("attacking", false);
		myAnim.SetBool("walking", false);

		rigidbody.freezeRotation = true;
	
	}

	void Update(){
		float triggerval = Input.GetAxis (MyInput.Triggers_name);

		if(triggerval < -0.9 || Input.GetKey (KeyCode.Mouse0)){
			myAnim.SetBool("attacking", true);
		} else {
			myAnim.SetBool("attacking", false);
		}

		if(triggerval > 0.9 || Input.GetKey (KeyCode.Mouse1)){
			myAnim.SetBool("blocking", true);
		} else {
			myAnim.SetBool("blocking", false);
		}


	}
	
	// Update is called once per frame
	void FixedUpdate () {

		if(myAnim.GetCurrentAnimatorStateInfo(0).IsName("Blocking")){
			modspeed = speed * 0.5f;
		} else {
			modspeed = speed;
		}
	
		//Moving the player based on the left stick
		float xinput = Input.GetAxis (MyInput.L_XAxisname);
		float zinput = -1 * Input.GetAxis (MyInput.L_YAxisname);
		
		// alternate, non-Xbox controller controls
		if (Input.GetKey (KeyCode.W))
			zinput = 1.0f;
		else if (Input.GetKey (KeyCode.S))
			zinput = -1.0f;

		// alternate, non-Xbox controller controls
		bool keyboard = false;
		if (Input.GetKey (KeyCode.A))
			zinput += 0.02f;
		else if (Input.GetKey(KeyCode.D))
			zinput += 0.02f;

		if(Mathf.Abs (xinput) > 0.01f || Mathf.Abs (zinput) > 0.01f){
			lookangle = (Mathf.Atan2 (xinput, zinput) * Mathf.Rad2Deg) + CameraController.angle;
			transform.eulerAngles = new Vector3(0f, lookangle, 0f);
		}

		float xmotion1 = xinput * Mathf.Sin ((CameraController.angle + 90) * Mathf.Deg2Rad);
		float zmotion1 = xinput * Mathf.Cos ((CameraController.angle + 90) * Mathf.Deg2Rad);

		float xmotion2 = zinput * Mathf.Sin (CameraController.angle * Mathf.Deg2Rad);
		float zmotion2 = zinput * Mathf.Cos (CameraController.angle * Mathf.Deg2Rad);

		float xmotion = xmotion1 + xmotion2;
		float zmotion = zmotion1 + zmotion2;

		Vector3 newmotion = new Vector3 (xmotion, 0f, zmotion);
	
		rigidbody.MovePosition (transform.position + (modspeed * newmotion * Time.deltaTime));
		

	


	}
}
