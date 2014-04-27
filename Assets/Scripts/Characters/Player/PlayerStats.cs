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

	private Vector3 weaponpos;
	private Vector3 weaponrot;
	private Vector3 shieldpos;
	private Vector3 shieldrot;
	
	// Use this for initialization
	void Start ()
	{
		myAnim = GetComponent<Animator>();
		totalHealth = 3;
		currentHealth = totalHealth;
		
		currentWeapon = (GameObject)Instantiate(Resources.Load("Prefabs/Items/Sword"));
		currentWeapon.transform.parent = GameObject.Find("Right_Forearm").transform;
		currentWeapon.transform.position = currentWeapon.transform.parent.position + new Vector3(0.5f,0.2f,0f);
		weaponpos = currentWeapon.transform.localPosition;
		weaponrot = currentWeapon.transform.localEulerAngles;

		//disable the non-trigger collider for the sword in hand
		BoxCollider[] colliders = currentWeapon.GetComponents<BoxCollider> ();
		foreach (BoxCollider coll in colliders){
			if(!coll.isTrigger){
				coll.enabled = false;
			}
		}
		
		currentShield = (GameObject)Instantiate(Resources.Load("Prefabs/Items/Shield"));
		currentShield.transform.parent = GameObject.Find("Left_Forearm").transform;
		currentShield.transform.position = currentShield.transform.parent.position + new Vector3(-.05f,.075f,.075f);
		currentShield.transform.rotation = currentShield.transform.parent.rotation * Quaternion.Euler(40f, 160f, 80f);
		shieldpos = currentShield.transform.localPosition;
		shieldrot = currentShield.transform.localEulerAngles;

		//disable the non-trigger collider for the sword in hand
		colliders = currentShield.GetComponents<BoxCollider> ();
		foreach (BoxCollider coll in colliders){
			if(!coll.isTrigger){
				coll.enabled = false;
			}
		}

		GameObject.FindWithTag ("GameController").GetComponent<GenerateLevel> ().swordList.Add (currentWeapon);
		GameObject.FindWithTag ("GameController").GetComponent<GenerateLevel> ().shieldList.Add (currentShield);
		
		//currentSpell = null;

		GameObject.FindWithTag ("GameController").GetComponent<PlayerHUD> ().updateItems (currentWeapon, currentShield);

		currentLevel = 1;

		// initialize "smellPoints" to allow smarter enemy chasing
		InvokeRepeating("UpdateSmellPoints", 0, 0.5f);
	}
	
	// Update is called once per frame
	void Update ()
	{
		int currdamage = currentWeapon.GetComponent<WeaponStats>().damage;
		float currturnspeed = currentShield.GetComponent<ShieldStats> ().turnspeed;
		float currmovespeed = currentShield.GetComponent<ShieldStats> ().movespeed;


		GameObject.Find ("ShieldGUI").GetComponent<GUIText> ().text = "Shield: (Move/Turn): " + currmovespeed + ", " + currturnspeed;
		GameObject.Find ("SpellGUI").GetComponent<GUIText>().text = "Spell: ?";
		GameObject.Find ("WeaponGUI").GetComponent<GUIText>().text = "Weapon (Damage): " + currdamage;

		//keep the currentweapon from falling out of the players hand
		currentWeapon.transform.localPosition = weaponpos;
		currentWeapon.transform.localEulerAngles = weaponrot;

		//keep the current shield from fallingout of the players hand
		currentShield.transform.localPosition = shieldpos;
		currentShield.transform.localEulerAngles = shieldrot;

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

	public void PickUpSword(GameObject newsword){
		GameObject oldsword = currentWeapon;
		currentWeapon = newsword;

		oldsword.transform.parent = null;
		currentWeapon.transform.parent = GameObject.Find("Right_Forearm").transform;

		//disable the non-trigger collider for the sword in hand
		BoxCollider[] colliders = currentWeapon.GetComponents<BoxCollider> ();
		foreach (BoxCollider coll in colliders){
			if(!coll.isTrigger){
				coll.enabled = false;
			}
		}

		//enable the non-trigger collider for the sword on ground
		colliders = oldsword.GetComponents<BoxCollider> ();
		foreach (BoxCollider coll in colliders){
			if(!coll.isTrigger){
				coll.enabled = true;
			}
		}

		oldsword.rigidbody.velocity = new Vector3 (1f, 3f, 1f);
	}

	public void PickUpShield(GameObject newShield){
		GameObject oldShield = currentShield;
		currentShield = newShield;
		
		oldShield.transform.parent = null;
		currentShield.transform.parent = GameObject.Find("Left_Forearm").transform;
		
		//disable the non-trigger collider for the sword in hand
		BoxCollider[] colliders = currentShield.GetComponents<BoxCollider> ();
		foreach (BoxCollider coll in colliders){
			if(!coll.isTrigger){
				coll.enabled = false;
			}
		}
		
		//enable the non-trigger collider for the sword on ground
		colliders = oldShield.GetComponents<BoxCollider> ();
		foreach (BoxCollider coll in colliders){
			if(!coll.isTrigger){
				coll.enabled = true;
			}
		}
		
		oldShield.rigidbody.velocity = new Vector3 (1f, 3f, 1f);
	}
}
