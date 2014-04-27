using UnityEngine;
using System.Collections;

public class EnemyStats : CharacterStats {
	
	[HideInInspector]
	public int attackDamage;
	//public float attackCooldown;
	
	// Use this for initialization
	void Start ()
	{
		// we eventually want to read this data in from the XML files
		totalHealth = 2 * GameObject.FindWithTag("Player").GetComponent<PlayerStats>().currentLevel;
		currentHealth = totalHealth;
		
		//currentSpell = null;
		
		attackDamage = 1;
		//attackCooldown = 1;
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (currentHealth <= 0)
		{
			// play death animation
			//GameObject.Destroy(transform.root.gameObject);
			
			// either deactivate this character (turn off AI and ragdoll it) or destroy the GameObject
			transform.gameObject.GetComponent<EnemyAI>().enabled = false;
			transform.rigidbody.freezeRotation = false;
			
		}
	}
}
