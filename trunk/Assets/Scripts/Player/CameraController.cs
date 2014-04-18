using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public GameObject player;
	public float speed;

	private float cameraHeight = 3f;
	public static float angle = 0;
	private float direction;

	// Use this for initialization
	void Start () {
		transform.eulerAngles = new Vector3 (45f, 0f, 0f);
	}
	
	// Update is called once per frame
	void Update () {

		Vector3 playerpos = player.transform.position;

		float rotatein = Input.GetAxis (MyInput.R_XAxisname);

		if(Mathf.Abs (rotatein) < 0.1){
			direction = 0f;
		} else {
			direction = Mathf.Sign (rotatein);
		}

		angle += direction * speed * Time.deltaTime;

		if(angle > 360){
			angle -= 360;
		}

		if(angle < 0){
			angle += 360;
		}

		transform.eulerAngles = new Vector3 (45f, angle, 0f);

		float cameraoffx = 2 * Mathf.Sin (angle * Mathf.Deg2Rad);
		float cameraoffz = 2 * Mathf.Cos (angle * Mathf.Deg2Rad);

		transform.position = new Vector3 (playerpos.x - cameraoffx, cameraHeight, playerpos.z - cameraoffz);
		
		
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
			
			if (cameraHeight < 3f)
				cameraHeight = 3f;
		}
		
	}
}
