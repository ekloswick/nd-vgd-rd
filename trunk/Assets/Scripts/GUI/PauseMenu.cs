using UnityEngine;
using System.Collections;

public class PauseMenu : MonoBehaviour {

	public GameObject player;
	private bool paused;

	// Use this for initialization
	void Start () {
		paused = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.P) || Input.GetButtonDown(MyInput.Start_name)){
			paused = !paused;

			GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
			GameObject[] guiitems = GameObject.FindGameObjectsWithTag("HUD");

			foreach(GameObject enemy in enemies){
				enemy.GetComponent<EnemyAI>().enabled = !paused;
			}

			player.GetComponent<PlayerController>().enabled = !paused;
			player.GetComponent<Animator>().enabled = !paused;

			foreach(GameObject gui in guiitems){
				gui.SetActive(!paused);
			}

			GameObject.Find ("GeneralScripts").GetComponent<PlayerHUD>().Pause (paused);

			GameObject.Find ("Level").SetActive(!paused);
		}


	}
}
