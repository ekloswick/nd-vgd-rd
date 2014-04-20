using UnityEngine;
using System.Collections;

public class MeleeEnemyAI : MonoBehaviour {

	//private bool playerSpotted = false;
	private GameObject playerReference;
	private Vector3 playerDirection;
	private float FOVAngle = 70f; // 45 means 90 total FOV
	private float FOVDistance = 10f;
	private float turnRate = 60f;
	// private thingy currentWaypoint = stuff;

	// Use this for initialization
	void Start ()
	{
		Debug.Log (transform.forward);
		playerReference = GameObject.FindWithTag("Player");
		
		//transform.position = playerReference.transform.position + new Vector3(0f,0f,2f);
	}
	
	// Update is called once per frame
	void Update ()
	{
		//playerSpotted = canSeePlayer();
		
		// if enemy can see player, move forward and slightly turn towards player position
		if (canSeePlayer())
		{
			transform.position = transform.position + 1f*transform.forward * Time.deltaTime;
			
			Debug.Log (Vector3.Angle(transform.forward, playerDirection));
			
			// calculate cross product between forward vector and vector connecting enemy and player
			if (Vector3.Cross(transform.forward, playerDirection).y < 0f)
				transform.Rotate(transform.up, -turnRate*Time.deltaTime);
			else
				transform.Rotate(transform.up, turnRate*Time.deltaTime);
		}
		
		//Debug.Log ("CanSeePlayer: " + playerSpotted);
	}
	
	// return true or false based on if enemy has unobstructed LOS to player
	bool canSeePlayer()
	{
		RaycastHit hit = new RaycastHit();;
		playerDirection = playerReference.transform.position - transform.position;
		
		// if player is in field of view and raycast isn't blocked
		if (Vector3.Angle(playerDirection, transform.forward) < FOVAngle)
		{
			// performs raycast calcuations, ~(1 << 8) is a layer mask saying we want to test every layer BUT 8, which is floor
			if (Physics.Raycast(transform.position, playerDirection, out hit, FOVDistance, ~(1 << 8)))
			{
				Debug.DrawRay (transform.position, playerDirection, Color.red, 2);
				
				if (hit.transform.tag == "Player" || hit.transform.tag == "PlayerModel")
					return true;
				else
					return false;
			}
		}
		
		return false;
	}
	
}
