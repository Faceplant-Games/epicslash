using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviour {
	private int previousStage = 0;
	public int stage = 0;
	public bool change = false;

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
        if (Input.GetKeyDown("b"))
        {
            print("Boom, every mobs die");
            AbstractMonster[] monsters = GameObject.FindObjectsOfType<AbstractMonster>();

            Array.ForEach(monsters, m => m.Die());
        }
    }

    private IEnumerator changeStage() // TODO : remove this or use this. This method is never called. PlayerB.levelup is used instead.
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

    //catch events change of stage

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
