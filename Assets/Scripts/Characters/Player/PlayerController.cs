using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	
	public float speed;
	public float notblockrotate;
	public float blockrotate;

	private string L_XAxisname;
	private string L_YAxisname;
	private string R_XAxisname;
	private string R_YAxisname;
	private string Triggers_name;
	private string DPad_YAxis_name;

	private Animator myAnim;
	private float rotatespeed;

	// Use this for initialization
	void Start () {

		myAnim = GetComponent<Animator> ();
		myAnim.SetBool("blocking", false);
		myAnim.SetBool ("attacking", false);

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

		turnPlayer ();

		movePlayer ();

	}

	void turnPlayer(){

		if(myAnim.GetCurrentAnimatorStateInfo(0).IsName("Blocking")){
			rotatespeed = blockrotate;
		} else {
			rotatespeed = notblockrotate;
		}
			
		//Turn the player based on the right stick
		float xlook = Input.GetAxis (MyInput.R_XAxisname);
		float zlook = -1 * Input.GetAxis (MyInput.R_YAxisname);
		
		float newangle = -1 * (Mathf.Atan2 (zlook, xlook) * Mathf.Rad2Deg - 90);
		float currangle =  transform.eulerAngles.y;

		//First make sure both angles are positive by adding 360
		newangle += 360;
		currangle += 360;

		//Next take the modulus with 360 to get a result between 0 and 360
		newangle %= 360;
		currangle %= 360;

		//Lastly add 360 again to get a result between 360 and 720 so we can subtract
		newangle += 360;
		currangle += 360;


		float difference = newangle - currangle;
		float direction = Mathf.Sign (difference);

		if(Mathf.Abs (difference) > 180){
			direction *= -1;
		}

		//Only rotate if the stick is moved and there is a angle difference > 5
		if(((Mathf.Abs (xlook) > 0.5f) || (Mathf.Abs (zlook) > 0.5f)) && (Mathf.Abs (difference) > 15f)){
			transform.Rotate(0f, rotatespeed * direction * Time.deltaTime, 0f);
		}

		print ((currangle%360) + ", " + (newangle%360));
	}

	void movePlayer(){
		//Moving the player based on the left stick
		float xinput = Input.GetAxis (MyInput.L_XAxisname);
		float zinput = -1 * Input.GetAxis (MyInput.L_YAxisname);
		
		// alternate, non-Xbox controller controls
		if (Input.GetKey (KeyCode.W))
			zinput = 1.0f;
		else if (Input.GetKey (KeyCode.S))
			zinput = -1.0f;
		
		// alternate, non-Xbox controller controls
		if (Input.GetKey (KeyCode.A))
			xinput = -1.0f;
		else if (Input.GetKey(KeyCode.D))
			xinput = 1.0f;
		
		//Setting the values in the Animation Blend Tree
		myAnim.SetFloat("forward", xinput);
		myAnim.SetFloat("strafe", zinput);
		
		Vector3 newmotion = new Vector3 (xinput, 0f, zinput);
		
		rigidbody.MovePosition (transform.position + (speed * newmotion * Time.deltaTime));
	}
}
