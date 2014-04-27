using UnityEngine;
using System.Collections;

public class BoulderDamage : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player" || other.tag == "Enemy")
		{
			if (transform.rigidbody.velocity.magnitude > 0.5f)
			{
				Debug.Log (other.tag + " HIT for 1 damage");
				other.gameObject.GetComponent<CharacterStats>().isAttackedBy(transform.gameObject);
			}
		}
	}
}
