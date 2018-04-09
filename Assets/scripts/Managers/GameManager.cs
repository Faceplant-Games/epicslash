using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System;
using System.IO;
using UnityEngine.Playables;

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
/// <seealso cref="Damage"/>
/// <seealso cref="GoldSpawnerB"/>
public class GameManager : MonoBehaviour {
    private const float LEVEL_DOWN_THRESH_OLD_FACTOR = 0.9f;
    public int currentStage = 0;
    public bool started;
    public GameObject instructions;

    long level;
    public AudioClip track;
    public AudioClip loopTrack;
    public AudioClip ups;
	public AudioClip downs;
	public AudioSource audioSource;

    public GameData gameData;
    private CoinGenerator coinGenerator;
    private string gameDataFileName = "data.json";

    private Fading fading;
    private DamageScript damage;
    private GameObject player;
    private Transform leftController;
    private Transform rightController;
    private GameObject leftWeapon;
    private GameObject rightWeapon;
    private ProgressBar gameInfoHUD;

    void Start ()
    {
        LoadGameData();
        InitializeTrack();
        InitializePlayer();
        InitializeScreen();
        InitializeIntroduction();
        coinGenerator = gameObject.AddComponent<CoinGenerator>();
    }

    void Update ()
    {
        ManageButtons();
    }

    public CoinGenerator getCoinGenerator()
    {
        return coinGenerator;
    }

    private void InitializeIntroduction()
    {
        if (currentStage != 0)
        {
            started = true;
            return;
        }
        started = false;
        GameObject movableMap = GameObject.FindGameObjectWithTag("MovableMap");
        movableMap.transform.position = new Vector3(0, 300, 0);
    }

    private void InitializeScreen()
    {
        print("Initializing screens (Fading, Damage...)");
        Vector3 pos = new Vector3(0, 0, 0);
        Quaternion rotation = Quaternion.Euler(0, 45, 0);
        fading = Instantiate(Resources.Load<GameObject>("FadingScreen"), pos, rotation).GetComponent<Fading>();
        damage = Instantiate(Resources.Load<GameObject>("DamageScreen"), pos, rotation).GetComponent<DamageScript>();
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

        Quaternion weaponRotation = Quaternion.Euler(0, 225, 0);
        leftWeapon = WeaponB.CreateWeapon(gameData.stages[currentStage].leftWeapon, pos, weaponRotation, leftController, audioSource);
        rightWeapon = WeaponB.CreateWeapon(gameData.stages[currentStage].rightWeapon, pos, weaponRotation, rightController, audioSource);

        GameObject gameInfoUI = Instantiate<GameObject>(Resources.Load<GameObject>("GameInfoUI"), pos, rotation);
        gameInfoUI.transform.parent = rightWeapon.transform;
        gameInfoUI.transform.localPosition = new Vector3(0.037f, 0, 0.07f);
        Quaternion gameInfoUIRotation = Quaternion.Euler(0, 135, 0);
        gameInfoUI.transform.rotation = gameInfoUIRotation;
        gameInfoHUD = gameInfoUI.GetComponentInChildren<ProgressBar>();
        if (currentStage == 0)
        {
            level = 0;
        }
        else
        {
            level = gameData.stageThresholds[currentStage - 1];
        }
        gameInfoHUD.currentExperience = level;
        gameInfoHUD.experienceGoal = gameData.stageThresholds[currentStage];

        // Debug
        if (gameData.profile == "Test" && !gameData.hasController)
        {
            leftController.gameObject.SetActive(true);
            rightController.gameObject.SetActive(true);
            leftController.transform.localPosition += new Vector3(-0.15f, 0, 0.2f);
            rightController.transform.localPosition += new Vector3(0.15f, 0, 0.2f);
            player.transform.position += new Vector3(0, 1.8f, 0);
        }
    }

    private void StartGame()
    {
        started = true;
        if (instructions != null)
        {
            instructions.SetActive(false);
        }
        GameObject movableMap = GameObject.FindGameObjectWithTag("MovableMap");
        movableMap.GetComponent<PlayableDirector>().Play();
    }

    private void ManageButtons() // TODO Split into multiple methods
    {
        if (Input.GetKeyDown(KeyCode.Space) && !started) // Start game
        {
            StartGame();
        }

        if (Input.GetKeyDown(KeyCode.Escape)) // Exit or return main menu game
        {
            if (started)
            {
                started = false;
                currentStage = 0;
                StartCoroutine(ChangeStage());

            } else
            {
                Application.Quit();
            }
        }

        if (gameData.profile != "Test")
        {
            return;
        }
        // Cheat Codes
        if (Input.GetKeyDown(KeyCode.B)) // Hit each monsters once
        {
            AbstractMonster[] monsters = GameObject.FindObjectsOfType<AbstractMonster>();

            print("Current monsters amount: " + monsters.Length);
            Array.ForEach(monsters, m => m.BeingHit());
        }
        if (Input.GetKeyDown(KeyCode.A)) // Hit one monster
        {
            AbstractMonster[] monsters = GameObject.FindObjectsOfType<AbstractMonster>();
            print("Current monsters amount: " + monsters.Length);
            monsters[0].BeingHit();
        }
        if (Input.GetKeyDown(KeyCode.H)) // Lose some experience 
        {
            LoseExperience(10);
        }
    }

    private void InitializeTrack()
    {
        audioSource.mute = gameData.muteAudio;
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
        loopAudio.mute = gameData.muteAudio;
    }   

    public void EarnExperience(int experience)
    {
        level += experience;
        gameInfoHUD.currentExperience = level;

        coinGenerator.SpawnGold(experience, this.gameObject.transform.position, this.gameObject.transform.rotation);

        if (level >= gameData.stageThresholds[currentStage])
        {
            StartCoroutine(StageUp());
        }
    }

    public void LoseExperience(int experience)
    {
        if (level < experience)
        {
            level = 0;
        } else
        {
            level -= experience;
        }
        damage.TakeDamage(experience);
        gameInfoHUD.currentExperience = level;

        if (currentStage == 0)
        {
            return;
        }

        if (level <= gameData.stageThresholds[currentStage-1] * LEVEL_DOWN_THRESH_OLD_FACTOR)
        {
            StartCoroutine(StageDown());
        }
    }

    private IEnumerator StageUp()
    {
        currentStage++;
        PlayStageUpSound();
        return ChangeStage();
    }

    private IEnumerator StageDown()
    {
        currentStage--;
        PlayStageDownSound();
        return ChangeStage();
    }

    private IEnumerator ChangeStage()
    {
        if (fading != null)
        {
            float fadeTime = fading.BeginFade(1);
            yield return new WaitForSeconds(1 + fadeTime);
        }
        SceneManager.LoadScene(currentStage);
    }

    public void PlayStageUpSound()
    {
        audioSource.PlayOneShot(ups);
    }

    public void PlayStageDownSound()
    {
        audioSource.PlayOneShot(downs);
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
        public String profile;
        public bool hasController;
        public bool muteAudio;

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
