using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
	private int previousStage = 0;
	public int stage = 0;
	public bool change = false;

    public AudioClip track;
    public AudioClip loopTrack;
    public AudioClip ups;
	public AudioClip downs;
	public AudioSource audioSource;



	// Use this for initialization
	void Start ()
    {
        InitializeTrack();
    }

    // Update is called once per frame
    void Update () {
		if (change) {
			change = false;
			if (stage > previousStage) {
				print ("on monte de niveau");
				audioSource.PlayOneShot (ups);
                //SceneManager.LoadScene("BossFight");
				SceneManager.LoadScene(stage ++);
			} else if (stage < previousStage) {
				print ("on baisse de niveau");
				audioSource.PlayOneShot (downs);
				if (stage > 0) 
				{
					SceneManager.LoadScene(stage - 1);
				}

			}
			previousStage = stage;
		}
	}

    //catch events change of stage

    private void InitializeTrack()
    {
        audioSource.PlayOneShot(track);
        if (loopTrack == null)
        {
            audioSource.loop = true;
            return;
        }
        audioSource.loop = false;

        AudioSource loopAudio = new AudioSource();
        loopAudio.transform.position = audioSource.transform.position;
        GameObject.Instantiate(loopAudio);
        loopAudio.PlayScheduled(track.length);
        loopAudio.PlayOneShot(loopTrack);
        loopAudio.loop = true;
    }


}
