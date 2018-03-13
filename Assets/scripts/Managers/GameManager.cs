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
/// <seealso cref="Fading"/>
/// <seealso cref="GoldSpawnerB"/>
public class GameManager : MonoBehaviour {
	public int currentStage = 0;
    public bool started;
    public Fading fading;
    public GameObject instructions;

    int level;    
    public AudioClip track;
    public AudioClip loopTrack;
    public AudioClip ups;
	public AudioClip downs;
	public AudioSource audioSource;

    private string gameDataFileName = "data.json";
    public GameData gameData;
    private CoinGenerator coinGenerator;

    public GameObject leftController; // Optional
    public GameObject rightController; // Optional
    public GameObject userVision; // Optional

    void Start ()
    {
        LoadGameData();
        InitializeTrack();
        coinGenerator = gameObject.AddComponent<CoinGenerator>();

        started = currentStage != 0;
    }

    void Update ()
    {
        ManageButtons();
    }

    private bool HasControllersAndVision() {
        return leftController != null && rightController != null && userVision != null;
    }

    private void ManageButtons()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !started) // Start game
        {
            started = true;
            if (instructions != null)
            {
                instructions.SetActive(false);
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape)) // Exit game
        {
            Application.Quit();
        }

        // Cheat Codes
        if (Input.GetKeyDown(KeyCode.B)) // Hit each monsters once
        {
            AbstractMonster[] monsters = GameObject.FindObjectsOfType<AbstractMonster>();

            print("monstres : " + monsters.Length);
            Array.ForEach(monsters, m => m.BeingHit());
        }
        if (Input.GetKeyDown(KeyCode.A)) // Hit one monster
        {
            AbstractMonster[] monsters = GameObject.FindObjectsOfType<AbstractMonster>();
            print("monstres : " + monsters.Length);
            monsters[0].BeingHit();
        }
        if (Input.GetKeyDown(KeyCode.P)) // Positionate controllers
        {
            if (HasControllersAndVision()) {
                leftController.SetActive(true);
                rightController.SetActive(true);
                leftController.transform.localPosition += new Vector3(-0.15f, 0, 0.2f);
                rightController.transform.localPosition += new Vector3(0.15f, 0, 0.2f);
                userVision.transform.position += new Vector3(0, 1.8f, 0);
            }
            else
            {
                print("Controllers not attached");
            }
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

        if (level >= gameData.stageThresholds[currentStage])
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
        currentStage++;

        if (fading != null)
        {
            float fadeTime = fading.BeginFade(1);
            PlayStageUpSound();
            yield return new WaitForSeconds(1+fadeTime);
        }

        SceneManager.LoadScene(currentStage);

        if (currentStage >= gameData.numberOfStages)
        {
            //TODO stop the game
        }
    }

    public void PlayStageUpSound()
    {
        audioSource.PlayOneShot(ups);
    }


    private void LoadGameData()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, gameDataFileName);
        if (!File.Exists(filePath))
        {
            Debug.LogError("Cannot load game data!");
            return;
        }

        string dataAsJson = File.ReadAllText(filePath);
        gameData = JsonUtility.FromJson<GameData>(dataAsJson);
    }

    [System.Serializable]
    public class GameData
    {
        public long[] stageThresholds;
        public int numberOfStages;
        public float spawnPeriod;
        public int maxAmountMonsters;
        public Stage[] stages;

        [System.Serializable]
        public class Stage
        {
            public string name;
            public string[] monsters;
        }
    }

}
