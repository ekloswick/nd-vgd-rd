using UnityEngine;
using System.Collections;

public class WeaponStats : MonoBehaviour {

	public int damage;
	public float cooldown;
	//public List<Attribute> attributes = new List<Attribute>();
	public GameObject playerReference;

	// Use this for initialization
	void Start ()
	{
		playerReference = GameObject.FindWithTag("Player");
		// when weapon is created, give it random properties
		damage = Random.Range(1+playerReference.GetComponent<PlayerStats>().currentLevel, 1+playerReference.GetComponent<PlayerStats>().currentLevel*2);
		cooldown = Random.Range (1,5) / 4f;
	}
	
	// Update is called once per frame
	void Update ()
	{
		
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
