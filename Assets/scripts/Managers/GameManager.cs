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

    public GameData gameData;
    private CoinGenerator coinGenerator;
    private string gameDataFileName = "data.json";

    private GameObject player;
    private Transform leftController;
    private Transform rightController;
    private GameObject leftWeapon;
    private GameObject rightWeapon;
    private bool positionated = false;
    private GameObject weapon;

    void Start ()
    {
        LoadGameData();
        InitializeTrack();
        InitializePlayer();
        coinGenerator = gameObject.AddComponent<CoinGenerator>();

        started = currentStage != 0;
    }

    void Update ()
    {
        ManageButtons();
    }

    private void InitializePlayer()
    {
        if (player != null)
        {
            print("Player found");
            return;
        }
        print("Initializing player");
        Vector3 pos = new Vector3(0, 0, 0);
        Quaternion rotation = Quaternion.Euler(0, 45, 0);
        player = Instantiate<GameObject>(Resources.Load<GameObject>("Player"), pos, rotation);
        player.name = "Player";
        // Get controllers
        foreach (Transform childTransform in player.transform) {
            switch (childTransform.tag) {
                case "LeftController":
                    leftController = childTransform;
                    break;
                case "RightController":
                    rightController = childTransform;
                    break;
            }
        }

        leftWeapon = WeaponB.createWeapon(Resources.Load<GameObject>(gameData.stages[currentStage].leftWeapon), pos, rotation, leftController, audioSource);
        rightWeapon = WeaponB.createWeapon(Resources.Load<GameObject>(gameData.stages[currentStage].rightWeapon), pos, rotation, rightController, audioSource);

        GameObject gameInfoUI = Instantiate<GameObject>(Resources.Load<GameObject>("GameInfoUI"), pos, rotation);
        gameInfoUI.transform.parent = rightWeapon.transform;
        gameInfoUI.transform.localPosition = new Vector3(-0.03f,0,-0.1f);
        Quaternion gameInfoUIRotation = Quaternion.Euler(0, 90, 0);
        gameInfoUI.transform.rotation = gameInfoUIRotation;

    }

    private void ManageButtons() // TODO Split into multiple methods
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
        if (Input.GetKeyDown(KeyCode.P) && !positionated) // Positionate controllers
        { // TODO enable this only once
            positionated = true;
            leftController.gameObject.SetActive(true);
            rightController.gameObject.SetActive(true);
            leftController.transform.localPosition += new Vector3(-0.15f, 0, 0.2f);
            rightController.transform.localPosition += new Vector3(0.15f, 0, 0.2f);
            player.transform.position += new Vector3(0, 1.8f, 0);
            print("Controllers and user vision positionned");
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
            public string leftWeapon;
            public string rightWeapon;
        }
    }

}
