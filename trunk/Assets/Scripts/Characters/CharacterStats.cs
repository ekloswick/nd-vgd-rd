using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

public class CharacterStats : MonoBehaviour {
	
	public float damagedCooldown;
	
	[HideInInspector]
	public int totalHealth, currentHealth;
	[HideInInspector]
	public GameObject currentSpell;
	[HideInInspector]
	public List<int> currentStatuses = new List<int>();
	
	private float damageTimeStamp;
	
	// Use this for initialization
	void Start ()
	{
		damageTimeStamp = Time.time;
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}
	
	public void isAttackedBy(GameObject source)
	{
		int damage = 0;
		
		// if cooldown has worn off, calculate possible damage
		if (damageTimeStamp < Time.time)
		{
			if (source.transform.root.tag == "Player")
			{
				// the original damage
				damage = source.GetComponent<WeaponStats>().damage;
				
				// add in any resistances/damage reductions here
				
				
				// if still some damage, hurt player and make invincible for short time
				if (damage > 0)
				{
					currentHealth -= damage;
					damageTimeStamp = Time.time + damagedCooldown;
				}
			}
			else if (source.transform.root.tag == "Enemy")
			{
				// the original damage
				damage = source.GetComponent<EnemyStats>().attackDamage;
				
				// add in any resistances/damage reductions here
				
				
				// if still some damage, hurt player and make invincible for short time
				if (damage > 0 && source.gameObject.GetComponentInChildren<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Attacking"))
				{
					currentHealth -= damage;
					damageTimeStamp = Time.time + damagedCooldown;
				}
				
			}
			// traps
			else if (source.transform.root.tag == "Floor" || source.transform.root.tag == "Boulder")
			{
				currentHealth -= 1;
				damageTimeStamp = Time.time + damagedCooldown;
			}
			
			// make sure negative health isn't a thing
			if (currentHealth < 0)
				currentHealth = 0;
		}
			
		return;
	}
}



/*private XmlDocument weaponsDoc = new XmlDocument();
	private XmlDocument shieldsDoc = new XmlDocument();
	private XmlDocument spellsDoc = new XmlDocument();
	private XmlDocument statusesDoc = new XmlDocument();*/


/*weaponsDoc.Load(Application.dataPath + "/Resources/XML/weapons.xml");
		shieldsDoc.Load(Application.dataPath + "/Resources/XML/shields.xml");
		spellsDoc.Load(Application.dataPath + "/Resources/XML/spells.xml");
		statusesDoc.Load(Application.dataPath + "/Resources/XML/statuses.xml");*/
