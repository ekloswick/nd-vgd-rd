using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public GameObject player;
	public float speed;

	private float cameraHeight = 5f;
	public static float angle = 0;
	private float direction;

	// Use this for initialization
	void Start () {
		transform.eulerAngles = new Vector3 (60f, 0f, 0f);
	}
	
	// Update is called once per frame
	void Update () {

		Vector3 playerpos = player.transform.position;

		transform.position = new Vector3 (playerpos.x, cameraHeight, playerpos.z - 2);
		
		// added for playable core, can be commented out afterwards
		if (Input.GetButton(MyInput.A_name) || Input.GetKey (KeyCode.Q))
		{
			cameraHeight += 0.1f;
			
			if (cameraHeight > 15f)
				cameraHeight = 15f;
		}
		else
		{
			cameraHeight -= 0.1f;
			
			if (cameraHeight < 5f)
				cameraHeight = 5f;
		}
		
	}
}
