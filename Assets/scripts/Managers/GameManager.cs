using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
	private int previousStage = 0;
	public int stage = 0;
	public bool change = false;

	public AudioSource theme1;
	public AudioSource theme1to2;
	public AudioSource theme2;
	public AudioSource theme2to3;
	public AudioSource theme3;
	public AudioSource theme3to4;
	public AudioSource theme4;
	public AudioSource theme4to5;
	public AudioSource theme5;

	public AudioSource ups;
	public AudioSource downs;



	// Use this for initialization
	void Start () {
		theme1.Play ();
	}
	
	// Update is called once per frame
	void Update () {
		if (change) {
			change = false;
			if (stage > previousStage)
				print ("on monte de niveau");
			else if (stage < previousStage)
				print ("on baisse de niveau");
			previousStage = stage;
		}
	}

	//catch events change of stage



}
