using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public GameObject player;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		Vector3 playerpos = player.transform.position;

		transform.position = new Vector3 (playerpos.x, 15f, playerpos.z - 10f);
	}
}
