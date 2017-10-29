using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System;
using System.IO;

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
	public int stage = 0;
    public Fading fading;

    int level;
    WeaponB weaponB;
    
    public AudioClip track;
    public AudioClip loopTrack;
    public AudioClip ups;
	public AudioClip downs;
	public AudioSource audioSource;

    private string gameDataFileName = "data.json";
    public GameData gameData;
    private CoinGenerator coinGenerator = new CoinGenerator();

    void Start ()
    {
        LoadGameData();
        InitializeTrack();
        coinGenerator.build();
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

    public void EarnExperienceAndGold(int experience)
    {
        level += experience;
        coinGenerator.SpawnGold(experience, this.gameObject.transform.position, this.gameObject.transform.rotation);


        print("Level: " + level);

        if (level >= gameData.stageThresholds[stage])
        {
            StartCoroutine(StageUp());
        }
    }


    public CoinGenerator getCoinGenerator()
    {
        return coinGenerator; 
    }

    // TODO rename and code this method
    public void LevelDown(int levels)
    {
 
    }

    private IEnumerator StageUp()
    {
        stage++;

        if (fading != null)
        {
            float fadeTime = fading.BeginFade(1);
            PlayStageUpSound();
            yield return new WaitForSeconds(1+fadeTime);
        }

        SceneManager.LoadScene(stage);

        if (stage > gameData.stageThresholds.Length)
        {
            //TODO stop the game
        }
    }

    public void PlayStageUpSound()
    {
        audioSource.PlayOneShot(ups);
    }



    void EquipWeapon(WeaponB weaponB)
    {
        this.weaponB = weaponB;
    }

    private void LoadGameData()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, gameDataFileName);
        if (File.Exists(filePath))
        {
            string dataAsJson = File.ReadAllText(filePath);
            gameData = JsonUtility.FromJson<GameData>(dataAsJson);
        }
        else
        {
            Debug.LogError("Cannot load game data!");
        }
    }

    [System.Serializable]
    public class GameData
    {
        public long[] stageThresholds;
        public string test;
    }

}
