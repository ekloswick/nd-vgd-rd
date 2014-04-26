using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerStats : CharacterStats {

	//public GameObject currentWeapon;
	//public GameObject currentShield;

	public Animator myAnim;
	public int currentLevel;
	public GameObject currentWeapon, currentShield;
	
	/*[HideInInspector]
	public GameObject currentSpell;*/
	[HideInInspector]
	public List<SmellPoint> smellPoints = new List<SmellPoint>();
	
	// Use this for initialization
	void Start ()
	{
		myAnim = GetComponent<Animator>();
		totalHealth = 3;
		currentHealth = 3;
		
		currentWeapon = (GameObject)Instantiate(Resources.Load("Prefabs/Items/Sword"));
		currentWeapon.transform.parent = GameObject.Find("Right_Forearm").transform;
		currentWeapon.transform.position = currentWeapon.transform.parent.position + new Vector3(0.5f,0.2f,0f);
		
		currentShield = (GameObject)Instantiate(Resources.Load("Prefabs/Items/Shield"));
		currentShield.transform.parent = GameObject.Find("Left_Forearm").transform;
		currentShield.transform.position = currentShield.transform.parent.position + new Vector3(-.05f,.075f,.075f);
		currentShield.transform.rotation = currentShield.transform.parent.rotation * Quaternion.Euler(40f, 160f, 80f);
		
		currentSpell = 0;

		GameObject.Find ("GeneralScripts").GetComponent<PlayerHUD> ().updateItems (currentWeapon, currentShield);

		currentLevel = 1;

		// initialize "smellPoints" to allow smarter enemy chasing
		InvokeRepeating("UpdateSmellPoints", 0, 0.5f);
	}
	
	// Update is called once per frame
	void Update ()
	{
		//lose the game
		if (currentHealth <= 0)
		{
			foreach (GUIText text in GameObject.Find("GameOverText").GetComponentsInChildren<GUIText>())
				text.guiText.enabled = true;
			
			Time.timeScale = 0.0f;
			
			if (Input.GetAxis (MyInput.Triggers_name) < -0.9 || Input.GetKeyDown (KeyCode.Space))
			{
				Time.timeScale = 1.0f;
				Application.LoadLevel("mainGame");
			}
		}
	}
	
	// handles smellpoints
	void UpdateSmellPoints()
	{
		// remove smellpoint if liftime <= 0
		for (int i = 0; i < smellPoints.Count; )
		{
			smellPoints[i].lifetime -= 0.5f;
			
			if (smellPoints[i].lifetime <= 0f)
			{
				GameObject.Destroy(smellPoints[i].smellPointObject);
				smellPoints.RemoveAt(i);
				continue;
			}
			else
				i++;
		}
		
		smellPoints.Insert(0, new SmellPoint(transform.position, 2.5f));
		//Debug.Log("New smellpoint at: " + smellPoints[smellPoints.Count - 1].point);
	}
}
