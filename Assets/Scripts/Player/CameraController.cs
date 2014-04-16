using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public GameObject player;

	private float cameraHeight = 3f;

	// Use this for initialization
	void Start () {
		transform.eulerAngles = new Vector3 (45f, 0f, 0f);
	}
	
	// Update is called once per frame
	void Update () {

		Vector3 playerpos = player.transform.position;
		float playerang = player.transform.eulerAngles.y;

		transform.eulerAngles = new Vector3 (45f, playerang, 0f);

		float cameraoffx = 2 * Mathf.Sin (playerang * Mathf.Deg2Rad);
		float cameraoffz = 2 * Mathf.Cos (playerang * Mathf.Deg2Rad);

		transform.position = new Vector3 (playerpos.x - cameraoffx, cameraHeight, playerpos.z - cameraoffz);
		
		
		// added for playable core, can be commented out afterwards
		if (Input.GetButton("A_Win") || Input.GetKey (KeyCode.Q))
		{
			cameraHeight += 0.1f;
			
			if (cameraHeight > 15f)
				cameraHeight = 15f;
		}
		else
		{
			cameraHeight -= 0.1f;
			
			if (cameraHeight < 3f)
				cameraHeight = 3f;
		}
		
	}
}
