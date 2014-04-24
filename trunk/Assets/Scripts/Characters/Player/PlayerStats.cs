using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerStats : CharacterStats {

	public GameObject righthand;
	public GameObject lefthand;

	private Animator myAnim;
	
	[HideInInspector]
	public List<SmellPoint> smellPoints = new List<SmellPoint>();
	
	// Use this for initialization
	void Start ()
	{
		//myAnim = GetComponent<Animator>();
		totalHealth = 3;
		currentHealth = 3;
		currentWeapon = 1;
		currentShield = 1;
		currentSpell = 0;
		
		// initialize "smellPoints" to allow smarter enemy chasing
		InvokeRepeating("UpdateSmellPoints", 0, 0.5f);
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (currentHealth <= 0)
		{
			//lose the game
			
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
