using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour {
	private int previousStage = 0;
	public int stage = 0;
	public bool change = false;

    public AudioClip track;
	public AudioClip ups;
	public AudioClip downs;
	public AudioSource audio;



	// Use this for initialization
	void Start () {
		audio.PlayOneShot(track);

	}
	
	// Update is called once per frame
	void Update () {
		if (change) {
			change = false;
			if (stage > previousStage) {
				print ("on monte de niveau");
				audio.PlayOneShot (ups);
                //SceneManager.LoadScene("BossFight");
				SceneManager.LoadScene(stage + 1);
			} else if (stage < previousStage) {
				print ("on baisse de niveau");
				audio.PlayOneShot (downs);
				if (stage > 0) 
				{
					SceneManager.LoadScene(stage - 1);
				}

			}
			previousStage = stage;
		}
	}

	//catch events change of stage



}
