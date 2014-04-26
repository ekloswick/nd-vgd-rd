using UnityEngine;
using System.Collections;

public class PlayerHUD : MonoBehaviour {

	public GUIText temptext;

	public GUITexture guiref;
	public GameObject player;
	public GameObject mycamera;
	public Texture fullheart;
	public Texture emptyheart;
	public float heartscale;
	public float offset;

	private int maxhealth;
	private int currhealth;

	private GUITexture[] heartsarray;
	private GameObject mysword;
	private GameObject myshield;

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

		refreshItemsPosition ();
	}

	public void updateItems(GameObject sword, GameObject shield){

		Destroy (mysword);
		Destroy (myshield);

		mysword = (GameObject)Instantiate (sword);
		myshield = (GameObject)Instantiate (shield);

		mysword.transform.parent = mycamera.transform;
		myshield.transform.parent = mycamera.transform;

		mysword.transform.localEulerAngles = new Vector3 (270f, 0f, 0f);
		myshield.transform.localEulerAngles = new Vector3 (270f, 90f, 0f);
		
		mysword.transform.localScale = new Vector3 (0.05f, 0.01f, 0.15f);
		myshield.transform.localScale = new Vector3 (0.02f, 0.04f, 0.04f);

		refreshItemsPosition ();

	}

	void refreshItemsPosition(){
		float aspect = (float) Screen.width / (float) Screen.height;
		
		mysword.transform.localPosition = new Vector3 ((0.5f * aspect) - 0.5f, 0.55f , 1f);
		myshield.transform.localPosition = new Vector3 ((0.5f * aspect) - 0.25f, 0.45f, 1f);
	}
}
