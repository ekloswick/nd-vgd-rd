using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

public class CharacterStats : MonoBehaviour {

	public int totalHealth, currentHealth;
	public int currentWeapon, currentShield, currentSpell;
	[HideInInspector]
	public List<int> currentStatuses = new List<int>();
	
	/*private XmlDocument weaponsDoc = new XmlDocument();
	private XmlDocument shieldsDoc = new XmlDocument();
	private XmlDocument spellsDoc = new XmlDocument();
	private XmlDocument statusesDoc = new XmlDocument();*/

	// Use this for initialization
	void Start ()
	{
		/*weaponsDoc.Load(Application.dataPath + "/Resources/XML/weapons.xml");
		shieldsDoc.Load(Application.dataPath + "/Resources/XML/shields.xml");
		spellsDoc.Load(Application.dataPath + "/Resources/XML/spells.xml");
		statusesDoc.Load(Application.dataPath + "/Resources/XML/statuses.xml");*/
		
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}
}



