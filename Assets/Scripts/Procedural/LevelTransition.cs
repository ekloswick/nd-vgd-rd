using UnityEngine;
using System.Collections;

public class LevelTransition : MonoBehaviour {

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnTriggerStay(Collider other)
	{
		if (other.tag == "Player")
		{
			//Debug.Log ("Player collision");
			
			if (Input.GetButton(MyInput.A_name))
			{
			    GameObject.FindWithTag("GameController").GetComponent<GenerateLevel>().proceedToNextLevel();
			}
		}
	}
}
