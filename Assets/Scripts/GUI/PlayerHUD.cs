using UnityEngine;
using System.Collections;

public class PlayerHUD : MonoBehaviour {

	public GUITexture guiref;
	public GameObject player;
	public Texture fullheart;
	public Texture emptyheart;
	public float heartscale;
	public float offset;

	private int maxhealth;
	private int currhealth;

	private GUITexture[] heartsarray;

	// Use this for initialization
	void Start ()
	{
		maxhealth = player.GetComponent<PlayerStats> ().totalHealth;

		heartsarray = new GUITexture[maxhealth];
		for(int i = 0; i < maxhealth; i++){
			heartsarray[i] = (GUITexture) Instantiate (guiref);
		}

	
	}
	
	// Update is called once per frame
	void Update ()
	{
		float yscale = heartscale;
		float xscale = (heartscale * Screen.height) / Screen.width;

		float yoffset = offset;
		float xoffset = (offset * Screen.height) / Screen.width;

		float currentx = xoffset;

		currhealth = player.GetComponent<PlayerStats> ().currentHealth;
		int temphealth = currhealth;

		for (int i = 0; i < maxhealth; i++) {
			heartsarray[i].transform.localScale = new Vector3 (xscale, yscale, 1f);
			heartsarray[i].transform.position = new Vector3 (currentx, 1 - yoffset);

			if(temphealth > 0){
				heartsarray[i].texture = fullheart;
				temphealth --;
			} else {
				heartsarray[i].texture = emptyheart;
			}


			currentx += xscale;
		}

		if(Input.GetKeyDown(KeyCode.Keypad0)){
			player.GetComponent<PlayerStats> ().currentHealth --;
		}

		if(Input.GetKeyDown(KeyCode.KeypadPeriod)){
			player.GetComponent<PlayerStats> ().currentHealth ++;
		}

	
	}
}
