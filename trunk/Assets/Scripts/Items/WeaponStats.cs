using UnityEngine;
using System.Collections;

public class WeaponStats : MonoBehaviour {

	public GUIText popupref;
	public int damage;
	public float cooldown;
	//public List<Attribute> attributes = new List<Attribute>();
	public GameObject playerReference;

	private GUIText popup;

	// Use this for initialization
	void Start ()
	{
		playerReference = GameObject.FindWithTag("Player");
		
		// when weapon is created, give it random properties
		// down to half current weapon damage, up to double current weapon damage
		// first sword starts at 2 damage
		GameObject currentweapon = playerReference.GetComponent<PlayerStats> ().currentWeapon;
		int currdamage = currentweapon.GetComponent<WeaponStats>().damage;
		if(currdamage == 0){
			damage = 2;
			renderer.material.color = new Color(0.75f, 0.75f, 0.75f);

		} else {
			damage = Random.Range (currdamage / 2, currdamage * 2);
			renderer.material.color = new Color(Random.value, Random.value, Random.value);
		}


		//damage = Random.Range(1+playerReference.GetComponent<PlayerStats>().currentLevel, (1+playerReference.GetComponent<PlayerStats>().currentLevel)*2);

		cooldown = Random.Range (1,5) / 4f;

		popup = (GUIText) Instantiate (popupref);
	}
	
	// Update is called once per frame
	void Update ()
	{
		Vector3 playerpos = playerReference.transform.position;
		Vector3 swordpos = gameObject.transform.position;
		
		if(Vector3.Distance(playerpos, swordpos) < 1 && transform.root.tag != "Player"){

			int currdamage = playerReference.GetComponent<PlayerStats>().currentWeapon.GetComponent<WeaponStats>().damage;

			popup.text = "Press A (Xbox) / Q (Keyboard) to pickup sword (Damage: " + currdamage + " -> " + damage + ")";
			if(Input.GetKeyDown(KeyCode.Q) || Input.GetButtonDown(MyInput.A_name)){
				playerReference.GetComponent<PlayerStats>().PickUpSword(gameObject);
			}
		} else {
			popup.text = "";
		}
		
	}
	
	void OnTriggerEnter(Collider other)
	{
		if (playerReference.GetComponent<PlayerStats>().myAnim.GetBool("attacking"))
		{
			if (other.tag == "Enemy")
			{
				// assume melee for now
				//if (weaponType == "Melee")
				//else if (weaponType == "Ranged")
				
				other.gameObject.GetComponent<EnemyStats>().isAttackedBy(playerReference.GetComponent<PlayerStats>().currentWeapon);
				
				Debug.Log ("HIT, Damage: " + damage + ", Enemy HP left: " + other.gameObject.GetComponent<EnemyStats>().currentHealth);
			}
		}
	}
}
