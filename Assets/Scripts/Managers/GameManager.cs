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
    public GameObject instructions;

    public AudioClip track;
    public AudioClip loopTrack;
    public AudioClip ups;
	public AudioClip downs;
	public AudioSource audioSource;

    public GameData gameData;
    private CoinGenerator coinGenerator;
    private BulletGenerator bulletGenerator;
    private MonsterGenerator monsterGenerator;
    private string gameDataFileName = "data.json";

    private Fading fading;
    private DamageScript damage;
    private GameObject player;
    private Transform leftController;
    private Transform rightController;
    private GameObject rightWeapon;
    private ProgressBar gameInfoHUD;


    void Start ()
    {
        LoadGameData();
        Game.isTransitioning = false;
        Game.gameManager = this;
        InitializeTrack();
        InitializePlayer();
        InitializeScreen();
        InitializeIntroduction();
        coinGenerator = gameObject.AddComponent<CoinGenerator>();
        bulletGenerator = gameObject.AddComponent<BulletGenerator>();
        monsterGenerator = gameObject.AddComponent<MonsterGenerator>();
    }

    void Update ()
    {
        ManageButtons();
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

    private void InitializeIntroduction()
    {
        if (Game.GetCurrentStage() != 0)
        {
            return;
        }
        if (Game.started)
        {
            instructions.SetActive(false);
        }
        else {
            GameObject movableMap = GameObject.FindGameObjectWithTag("MovableMap");
            movableMap.transform.position = new Vector3(0, 300, 0);
        }
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
        player = Instantiate<GameObject>(Resources.Load<GameObject>("Player"));
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
        
        WeaponB.CreateWeapon(gameData.stages[Game.GetCurrentStage()].leftWeapon, leftController, audioSource);
        rightWeapon = WeaponB.CreateWeapon(gameData.stages[Game.GetCurrentStage()].rightWeapon, rightController, audioSource);


        // Debug
        if (gameData.profile == "Test" && Game.GetCurrentStage() != 0 && Game.level == 0)
        {
            Game.level = gameData.stageThresholds[Game.GetCurrentStage()-1];
        }
        GameObject gameInfoUI = Instantiate<GameObject>(Resources.Load<GameObject>("GameInfoUI"), rightWeapon.transform);
        gameInfoUI.transform.localPosition = new Vector3(0.037f, 0, 0.07f);
        Vector3 gameInfoUIRotation = new Vector3(0, 180, 90);
        gameInfoUI.transform.Rotate(gameInfoUIRotation);
        gameInfoHUD = gameInfoUI.GetComponentInChildren<ProgressBar>();
        gameInfoHUD.currentExperience = Game.level;
        gameInfoHUD.experienceGoal = gameData.stageThresholds[Game.GetCurrentStage()];
        leftController.gameObject.SetActive(true);
        rightController.gameObject.SetActive(true);

        // Debug
        if (gameData.profile == "Test" && !gameData.hasController)
        {
            leftController.transform.localPosition += new Vector3(-0.15f, 0, 0.2f);
            rightController.transform.localPosition += new Vector3(0.15f, 0, 0.2f);
            leftController.transform.Rotate(new Vector3(-60f, 5, 10));
            rightController.transform.Rotate(new Vector3(-20f, 0, 0));
            player.transform.position += new Vector3(0, 1.8f, 0);
        }
    }

    private void ManageButtons()
    {
        if (Input.GetKeyDown(KeyCode.Space)) // Start game
        {
            StartGame();
        }

        if (Input.GetKeyDown(KeyCode.Escape)) // Exit or return main menu game
        {
            if (Game.started)
            {
                Game.InitializeDefaultValues();
                StartCoroutine(ChangeStage(0));
            } else
            {
                Application.Quit();
            }
        }

        if (gameData.profile == "Test")
        {
            ManageCheatCodes();
        }
    }

    private void ManageCheatCodes()
    {
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
            if (monsters.Length == 0)
            {
                return;
            }
            monsters[0].BeingHit();
        }
        if (Input.GetKeyDown(KeyCode.Z)) // Lose some experience 
        {
            LoseExperience(10);
        }
        if (Input.GetKeyDown(KeyCode.I)) // Debug Game Info
        {
            Debug.Log("Started: " + Game.started);
            Debug.Log("CurrentStage: " + Game.GetCurrentStage());
            Debug.Log("Level: " + Game.level);
        }
        if (Input.GetKey(KeyCode.H))
        {
            player.transform.Rotate(new Vector3(0, -1, 0));
        }
        if (Input.GetKey(KeyCode.J))
        {
            player.transform.Rotate(new Vector3(1, 0, 0));
        }
        if (Input.GetKey(KeyCode.K))
        {
            player.transform.Rotate(new Vector3(-1, 0, 0));
        }
        if (Input.GetKey(KeyCode.L))
        {
            player.transform.Rotate(new Vector3(0, 1, 0));
        }
    }

    private void StartGame()
    {
        if (Game.started)
        {
            return;
        }
        Game.started = true;
        if (instructions != null) // Hide instructions
        {
            instructions.SetActive(false);
        }
        // Start introduction timeline
        GameObject movableMap = GameObject.FindGameObjectWithTag("MovableMap");
        movableMap.GetComponent<PlayableDirector>().Play();
    }

    public void EarnExperience(int experience)
    {
        if (Game.isTransitioning)
        {
            return;
        }

        Game.level += experience;
        gameInfoHUD.currentExperience = Game.level;

        coinGenerator.SpawnGold(experience, this.gameObject.transform.position, this.gameObject.transform.rotation);

        if (Game.level >= gameData.stageThresholds[Game.GetCurrentStage()])
        {
            StartCoroutine(StageUp());
        }
    }

    public void LoseExperience(int experience)
    {
        if (Game.isTransitioning)
        {
            return;
        }
        if (Game.level < experience)
        {
            Game.level = 0;
        } else
        {
            Game.level -= experience;
        }
        damage.TakeDamage(experience);
        gameInfoHUD.currentExperience = Game.level;

        if (Game.GetCurrentStage() == 0)
        {
            return;
        }

        if (Game.level <= gameData.stageThresholds[Game.GetCurrentStage() - 1] * gameData.levelDownThresholdFactor)
        {
            StartCoroutine(StageDown());
        }
    }

    private IEnumerator StageUp()
    {
        PlayStageUpSound();
        return ChangeStage(Game.GetCurrentStage()+1);
    }

    private IEnumerator StageDown()
    {
        PlayStageDownSound();
        return ChangeStage(Game.GetCurrentStage()-1);
    }

    private IEnumerator ChangeStage(int targetStage)
    {
        Game.isTransitioning = true;
        if (fading != null)
        {
            float fadeTime = fading.BeginFade(1);
            yield return new WaitForSeconds(1 + fadeTime);
        }
        SceneManager.LoadScene(targetStage);
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

    public CoinGenerator GetCoinGenerator()
    {
        return coinGenerator;
    }

    public BulletGenerator GetBulletGenerator()
    {
        return bulletGenerator;
    }

    public MonsterGenerator GetMonsterGenerator()
    {
        return monsterGenerator;
    }

    [System.Serializable]
    public class GameData
    {
        public long[] stageThresholds;
        public float levelDownThresholdFactor;
        public int numberOfStages;
        public int maxAmountMonsters;
        public Stage[] stages;
        public String profile;
        public bool hasController;
        public bool muteAudio;

        [System.Serializable]
        public class Stage
        {
            public string name;
            public MonsterData[] monsters;
            public string leftWeapon;
            public string rightWeapon;
            public bool isShotEnabled;

            [System.Serializable]
            public class MonsterData
            {
                public string name;
                public float spawnPeriod;
            }
        }
    }
}
