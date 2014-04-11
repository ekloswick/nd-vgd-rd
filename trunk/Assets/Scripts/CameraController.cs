using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public GameObject player;

	// Use this for initialization
	void Start () {
		transform.eulerAngles = new Vector3 (45f, 0f, 0f);
	}
	
	// Update is called once per frame
	void Update () {

		Vector3 playerpos = player.transform.position;
		float playerang = player.transform.eulerAngles.y;

		transform.eulerAngles = new Vector3 (45f, playerang, 0f);

		float cameraoffx = 10 * Mathf.Sin (playerang * Mathf.Deg2Rad);
		float cameraoffz = 10 * Mathf.Cos (playerang * Mathf.Deg2Rad);

		transform.position = new Vector3 (playerpos.x - cameraoffx, 15f, playerpos.z - cameraoffz);
	}
}
