using UnityEngine;
using System.Collections;

public class EnemyStats : CharacterStats {
	
	//private Animator myAnim;
	
	// Use this for initialization
	void Start ()
	{
		// we eventually want to read this data in from the XML files
		totalHealth = 2;
		currentHealth = 2;
		currentWeapon = 1;
		currentShield = 0;
		currentSpell = 0;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (currentHealth <= 0)
		{
			// play death animation
			
			// either deactivate this character (turn off AI and ragdoll it) or destroy the GameObject
			
		}
	}
}
