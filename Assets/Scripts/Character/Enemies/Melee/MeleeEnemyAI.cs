using UnityEngine;
using System.Collections;

public class MeleeEnemyAI : EnemyAI {


	// Use this for initialization
	void Start ()
	{
		//Debug.Log (transform.forward);
		playerReference = GameObject.FindWithTag("Player");
		desiredDistance = 0.5f;
	}
	
	// Update is called once per frame
	void Update ()
	{
		//Debug.Log(currentTarget);
		
		// if enemy can see player, move forward and slightly turn towards player position
		if (canSeePlayer())
		{
			// in range/capacity to attack player
			if (canAttackPlayer())
				attackPlayer();
			else
				moveToTarget(chaseSpeed, chaseTurnRate);
		}
		// if enemy has sight of a smellpoint
		else if (canSeeSmellPoint())
		{
			moveToTarget(chaseSpeed, chaseTurnRate);
		}
		// if last smellpoint is still in memory
		else if (targetIsFromMemory)
		{
			if (distanceToTarget() > 0.1f)
				moveToTarget(chaseSpeed, chaseTurnRate);
			else
				targetIsFromMemory = false;
		}
		// no chase targets, enemy should wander
		else
		{
			//
			// TODO IMPLEMENT
			//
			
			if (canSeeWanderPoint())
				moveToTarget(wanderSpeed, wanderTurnRate);
			else
			{
				// mill about?
				
			}
		}
		
		// pushes the enemy away from walls
		objectAvoidance();
	}
}
