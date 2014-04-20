using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

public class Stats : MonoBehaviour {

	public int health = 1;
	public int currentWeapon = 1, currentShield = 1, currentSpell = 0;
	public List<int> currentStatuses = new List<int>();
	
	private XmlDocument weaponsDoc = new XmlDocument();
	private XmlDocument shieldsDoc = new XmlDocument();
	private XmlDocument spellsDoc = new XmlDocument();
	private XmlDocument statusesDoc = new XmlDocument();
	private Animator myAnim;

	// Use this for initialization
	void Start ()
	{
		// every character should have an animator, but for now only do this for the player
		if (this.tag == "Player")
		{
			myAnim = GetComponent<Animator>();
			health = 3;
		}
		
		/*weaponsDoc.Load(Application.dataPath + "/Resources/XML/weapons.xml");
		shieldsDoc.Load(Application.dataPath + "/Resources/XML/shields.xml");
		spellsDoc.Load(Application.dataPath + "/Resources/XML/spells.xml");
		statusesDoc.Load(Application.dataPath + "/Resources/XML/statuses.xml");*/
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (health <= 0)
		{
			if (this.tag == "Player")
			{
				//lose the game
				
			}
			else
			{
				// play death animation
				
				// deactivate this character (turn off AI)
				
			}
		}
	}
}
