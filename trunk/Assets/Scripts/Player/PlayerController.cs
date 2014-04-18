using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	
	public float speed;
	public float rotatespeed;

	private float modspeed;

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
	

	}

}
