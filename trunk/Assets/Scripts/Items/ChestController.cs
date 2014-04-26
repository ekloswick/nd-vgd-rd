using UnityEngine;
using System.Collections;

public class ChestController : MonoBehaviour {

	private GameObject player;
	public GUIText popupref;
	private Animator myAnim;
	private GUIText popup;

	// Use this for initialization
	void Start () {
		player = GameObject.Find("simpleplayer");
		popup = (GUIText) Instantiate (popupref);
		myAnim = GetComponent<Animator> ();

		myAnim.SetBool ("isOpen", false);
	}
	
	// Update is called once per frame
	void Update () {

		Vector3 playerpos = player.transform.position;
		Vector3 chestpos = gameObject.transform.position;

		if(Vector3.Distance(playerpos, chestpos) < 1){
			popup.text = "Press A (Xbox) / Q (Keyboard) to open chest";
			if(Input.GetKeyDown(KeyCode.Q) || Input.GetAxis(MyInput.A_name) > 0.1f){
				myAnim.SetBool("isOpen", true);
			}
		} else {
			popup.text = "";
		}
	}
}
