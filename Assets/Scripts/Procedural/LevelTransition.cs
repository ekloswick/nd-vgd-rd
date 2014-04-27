using UnityEngine;
using System.Collections;

public class LevelTransition : MonoBehaviour {

	public GUIText popupref;
	private GUIText popup;

	// Use this for initialization
	void Start ()
	{
		popup = (GUIText)Instantiate (popupref);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnTriggerStay(Collider other)
	{
		if (other.tag == "Player")
		{
			//Debug.Log ("Player collision");
			popup.text = "Press A (Xbox) / Q (Keyboard) to advance to next level";

			if (Input.GetButton(MyInput.A_name))
			{
			    GameObject.FindWithTag("GameController").GetComponent<GenerateLevel>().proceedToNextLevel();
			}
		}
	}

	void OnTriggerExit(Collider other)
	{
		if(other.tag == "Player")
		{
			popup.text = "";
		}
	}
}
