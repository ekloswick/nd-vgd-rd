using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

public class CharacterStats : MonoBehaviour {

	public int totalHealth, currentHealth;
	public int currentSpell;
	
	[HideInInspector]
	public List<int> currentStatuses = new List<int>();
	
	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}
	
	public void isAttackedBy(GameObject source)
	{
		if (source.transform.parent.tag == "Player")
		{
			// add in any resistances/damage reductions here
			
			// the actual damage
			currentHealth -= source.GetComponent<WeaponStats>().damage;
		}
		else if (source.transform.parent.tag == "Enemy")
		{
			// add in any resistances/damage reductions here
			
			// the actual damage
			currentHealth -= source.GetComponent<EnemyStats>().attackDamage;
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
