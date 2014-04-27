using UnityEngine;
using System.Collections;

public class BoulderDamage : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnCollisionEnter(Collision other)
	{
		if (other.transform.tag == "Player" || other.transform.tag == "Enemy")
		{
			if (transform.rigidbody.velocity.magnitude > 1.5f)
			{
				//Debug.Log (other.tag + " HIT for 1 damage");
				//Debug.Log (transform.rigidbody.velocity.magnitude);
				other.gameObject.GetComponent<CharacterStats>().isAttackedBy(transform.gameObject);
			}
		}
	}
}
