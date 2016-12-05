using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviour {
	private int previousStage = 0;
	public int stage = 0; // TODO Can we keep it in a global variable ?
	public bool change = false; // TODO rename this with a clearer name

    public AudioClip track;
    public AudioClip loopTrack;
    public AudioClip ups;
	public AudioClip downs;
	public AudioSource audioSource;
    public Fading fading;



	// Use this for initialization
	void Start ()
    {
        InitializeTrack();
    }

    // Update is called once per frame
    void Update () {
		if (change)
        {
            changeStage();
        }

        // Cheat Codes
		// TODO put this in a loadCheatCodes method
		// TODO add a "no controller mode" cheatcode
		// TODO add a "rotate view" cheatcode
        if (Input.GetKeyDown("b"))
        {
            AbstractMonster[] monsters = GameObject.FindObjectsOfType<AbstractMonster>();
            Array.ForEach(monsters, m => m.Die());
        }
    }

    private IEnumerator changeStage() // TODO : remove this or use this. This method is never called. PlayerManager.levelup is used instead.
    {
        change = false;
        if (stage > previousStage)
        {
            print("on monte de niveau");
            audioSource.PlayOneShot(ups);
            //SceneManager.LoadScene("BossFight");
            if (fading != null)
            {
                float fadeTime = fading.BeginFade(1);
                yield return new WaitForSeconds(fadeTime);
            }
            SceneManager.LoadScene(stage++);
        }
        else if (stage < previousStage)
        {
            print("on baisse de niveau");
            audioSource.PlayOneShot(downs);
            if (stage > 0)
            {
                SceneManager.LoadScene(stage - 1);
            }
        }
        previousStage = stage;
    }

    private void InitializeTrack()
    {
        audioSource.clip = track;
        if (loopTrack == null)
        {
            audioSource.loop = true;
            audioSource.Play();
            return;
        }
        audioSource.loop = false;

        AudioSource loopAudio = gameObject.AddComponent<AudioSource>();
        loopAudio.loop = true;
        loopAudio.clip = loopTrack;
        audioSource.Play();
        loopAudio.PlayDelayed(track.length);
    }

    public void playLevelUpSound()
    {
        audioSource.PlayOneShot(ups);
    }

}
