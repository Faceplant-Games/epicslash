using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System;
using System.Collections.Generic;

/// <summary>
/// This class manages the game rules. It includes:
///     - stages initialization,
///     - player state (level up, weapons, audio, ...),
///     - monsters
///     - loot spawning
///     
/// Link this script to an Empty Game Object in each stage.
/// 
/// Mandatory fields:
///     - stage: the stage id, from 0 to the last stage.
///     - fading: a Fading Game object, used as a transition screen during stage transitions.
///     - track: original sound track of the stage. If there's no loopTrack specified, it will loop. Otherwise, it will be played once.
///     - ups: stage up sound.
///     - downs: stage down sound.
/// Optional fields:
///     - loopTrack: original sound track of the stage, played in loop after "track" is played once.
/// </summary>
/// <seealso cref="MonsterManager"/>
/// <seealso cref="WeaponB"/>
/// <seealso cref="Fading"/>
/// <seealso cref="GoldSpawnerB"/>
public class GameManager : MonoBehaviour {
	private int previousStage = 0;
	public int stage = 0;
	bool change = false;
    public Fading fading;

    // old playerB
    int level;
    long[] treshs = { 100, 100000, 100000000, 100000000000, 100000000000000 };
    List<GoldSpawnerB> goldSpawners = new List<GoldSpawnerB>();
    WeaponB weaponB;

    public AudioClip track;
    public AudioClip loopTrack;
    public AudioClip ups;
	public AudioClip downs;
	public AudioSource audioSource;


	// Use this for initialization
	void Start ()
    {
        InitializeTrack();
        
        GoldSpawnerB[] goldS = GameObject.FindObjectsOfType(typeof(GoldSpawnerB)) as GoldSpawnerB[];
        foreach (GoldSpawnerB spawner in goldS)
        {
            goldSpawners.Add(spawner);
        }
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
            AbstractMonster[] monsters = GameObject.FindObjectsOfType<AbstractMonster>();
			print ("monstres : " +  monsters.Length);
            Array.ForEach(monsters, m => m.BeingHit());
			//monsters = GameObject.FindObjectsOfType<AbstractMonster>();
		}

        if (Input.GetKeyDown("a"))
        {
            AbstractMonster[] monsters = GameObject.FindObjectsOfType<AbstractMonster>();
            print("monstres : " + monsters.Length);
            monsters[0].BeingHit();
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

        

    public void levelUp(int levels)
    {
        level += levels;
        spawnGold(levels % 37);
        print("Level: " + level);
        if (level >= treshs[stage])
        {
            StartCoroutine(stageUp());
        }
    }

    private IEnumerator stageUp()
    {
        stage++;
        change = true;

        if (fading != null)
        {
            float fadeTime = fading.BeginFade(1);
            playLevelUpSound();
            yield return new WaitForSeconds(fadeTime * 7);
        }

        SceneManager.LoadScene(stage);

        if (stage > treshs.Length)
        {
            //stop the game
        }
    }


    public void spawnGold(int levels)
    {
        // random on goldSpawners.length, to pop some gold bags
        goldSpawners[UnityEngine.Random.Range(0, goldSpawners.Count)].Spawn(levels);
    }

    public void levelDown(int levels)
    {
        /*level -= levels;
        print("level down");
        if (stage >0)
        {
            if ( level < treshs[stage-1] ) {
				stage--;
				gm.stage = stage;
				gm.change = true;
				//FIXME PAUSE
			}
		}*/
    }

    void equipWeapon(WeaponB weaponB)
    {
        this.weaponB = weaponB;
    }
}
