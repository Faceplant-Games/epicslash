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

    int level;
    long[] treshs = { 100, 100000, 100000000, 100000000000, 100000000000000 };
    List<GoldSpawnerB> goldSpawners = new List<GoldSpawnerB>();
    WeaponB weaponB;

    public AudioClip track;
    public AudioClip loopTrack;
    public AudioClip ups;
	public AudioClip downs;
	public AudioSource audioSource;


	void Start ()
    {
        InitializeTrack();
        
        GoldSpawnerB[] goldS = GameObject.FindObjectsOfType(typeof(GoldSpawnerB)) as GoldSpawnerB[];
        foreach (GoldSpawnerB spawner in goldS)
        {
            goldSpawners.Add(spawner);
        }
    }

    void Update () {

        // Cheat Codes
        if (Input.GetKeyDown("b"))
        {
            AbstractMonster[] monsters = GameObject.FindObjectsOfType<AbstractMonster>();

			print ("monstres : " +  monsters.Length);
            Array.ForEach(monsters, m => m.BeingHit());
            
		}

        if (Input.GetKeyDown("a"))
        {
            AbstractMonster[] monsters = GameObject.FindObjectsOfType<AbstractMonster>();
            print("monstres : " + monsters.Length);
            monsters[0].BeingHit();
        }
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

    public void EarnExperienceAndGold(int levels)
    {
        level += levels;
        spawnGold(levels % 37);
        print("Level: " + level);
        if (level >= treshs[stage])
        {
            StartCoroutine(StageUp());
        }
    }

    public void LevelDown(int levels)
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

    private IEnumerator StageUp()
    {
        stage++;
        change = true;

        if (fading != null)
        {
            float fadeTime = fading.BeginFade(1);
            PlayStageUpSound();
            yield return new WaitForSeconds(fadeTime * 7);
        }

        SceneManager.LoadScene(stage);

        if (stage > treshs.Length)
        {
            //stop the game
        }
    }

    public void PlayStageUpSound()
    {
        audioSource.PlayOneShot(ups);
    }

    public void spawnGold(int levels)
    {
        // random on goldSpawners.length, to pop some gold bags
        goldSpawners[UnityEngine.Random.Range(0, goldSpawners.Count)].Spawn(levels);
    }

    void EquipWeapon(WeaponB weaponB)
    {
        this.weaponB = weaponB;
    }
}
