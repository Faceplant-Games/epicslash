using System;
using System.IO;
using Managers.StageBoss;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

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
///     - StageUpSound: The audio clip to play on changing to next stage
///     - StageDownSound: The audio clip to play on changing to previous stage
///     - AudioSource
/// Optional fields:
///     - Instructions: Diegetic screen that disappears on start
/// </summary>
/// <seealso cref="MonsterManager"/>

public class GameManager : MonoBehaviour {
    public GameObject Instructions;

    public AudioSource AudioSource;
    public AudioClip StageUpSound;
	public AudioClip StageDownSound;

    public GameData Data;
        
    private CoinGenerator _coinGenerator;
    private BulletGenerator _bulletGenerator;
    private MonsterGenerator _monsterGenerator;
    private const string GameDataFileName = "data.json";
    private bool _stageBossActive;

    private GameObject _player;
    private Transform _leftController;
    private Transform _rightController;
    private GameObject _rightWeapon;
    private ProgressBar _gameInfoHud;
    private bool _isLoadingStage;

    private void Start ()
    {
        LoadGameData();
        Game.IsTransitioning = false;
        Game.GameManager = this;
        InitializeTrack();
        InitializePlayer();
        InitializeIntroduction();
        _coinGenerator = gameObject.AddComponent<CoinGenerator>();
        _bulletGenerator = gameObject.AddComponent<BulletGenerator>();
        _monsterGenerator = gameObject.AddComponent<MonsterGenerator>();
    }

    private void Update ()
    {
        ManageButtons();
    }

    private void InitializeTrack()
    {
        var bgm = Resources.Load<AudioClip>("SFX/"+Data.stages[Game.GetCurrentStage()].audioTrack);
        var loopBgm = Resources.Load<AudioClip>("SFX/"+Data.stages[Game.GetCurrentStage()].audioTrackLoop);

        AudioSource.mute = Data.muteAudio;
        AudioSource.clip = bgm;
        if (loopBgm == null)
        {
            AudioSource.loop = true;
            AudioSource.Play();
            return;
        }
        AudioSource.loop = false;

        var loopAudio = gameObject.AddComponent<AudioSource>();
        loopAudio.loop = true;
        loopAudio.clip = loopBgm;
        AudioSource.Play();
        loopAudio.PlayDelayed(bgm.length);
        loopAudio.mute = Data.muteAudio;
    }

    private void InitializeIntroduction()
    {
        if (Game.GetCurrentStage() != 0)
        {
            return;
        }
        if (Game.Started)
        {
            Instructions.SetActive(false);
        }
        else {
            var movableMap = GameObject.FindGameObjectWithTag("MovableMap");
            movableMap.transform.position = new Vector3(0, 300, 0);
        }
    }

    private void InitializePlayer()
    {
        if (_player != null)
        {
            print("Player found");
            return;
        }
        print("Initializing player");
        _player = Instantiate(Resources.Load<GameObject>("Player"));
        _player.name = "Player";
        // Get controllers
        foreach (Transform childTransform in _player.transform) {
            switch (childTransform.tag) {
                case "LeftController":
                    _leftController = childTransform;
                    break;
                case "RightController":
                    _rightController = childTransform;
                    break;
            }
        }
        
        WeaponB.CreateWeapon(Data.stages[Game.GetCurrentStage()].leftWeapon, _leftController, AudioSource);
        _rightWeapon = WeaponB.CreateWeapon(Data.stages[Game.GetCurrentStage()].rightWeapon, _rightController, AudioSource);


        // Debug
        if (Data.profile == "Test" && Game.GetCurrentStage() != 0 && Game.Level == 0)
        {
            Game.Level = Data.stageThresholds[Game.GetCurrentStage()-1];
        }
        var gameInfoUI = Instantiate(Resources.Load<GameObject>("GameInfoUI"), _rightWeapon.transform);
        gameInfoUI.transform.localPosition = new Vector3(0.037f, 0, 0.07f);
        var gameInfoUIRotation = new Vector3(0, 180, 90);
        gameInfoUI.transform.Rotate(gameInfoUIRotation);
        _gameInfoHud = gameInfoUI.GetComponentInChildren<ProgressBar>();
        _gameInfoHud.CurrentExperience = Game.Level;
        _gameInfoHud.ExperienceGoal = Data.stageThresholds[Game.GetCurrentStage()];
        _leftController.gameObject.SetActive(true);
        _rightController.gameObject.SetActive(true);

        // Debug
        if (Data.profile == "Test" && !Data.hasController)
        {
            _leftController.transform.localPosition += new Vector3(-0.15f, 0, 0.2f);
            _rightController.transform.localPosition += new Vector3(0.15f, 0, 0.2f);
            _leftController.transform.Rotate(new Vector3(-60f, 5, 10));
            _rightController.transform.Rotate(new Vector3(-20f, 0, 0));
            _player.transform.position += new Vector3(0, 1.8f, 0);
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
            if (Game.Started)
            {
                Game.InitializeDefaultValues();
                ChangeStage(0);
            } else
            {
                Application.Quit();
            }
        }

        if (Data.profile == "Test")
        {
            ManageCheatCodes();
        }
    }

    private void ManageCheatCodes()
    {
        if (Input.GetKeyDown(KeyCode.B)) // Hit each monsters once
        {
            var monsters = FindObjectsOfType<AbstractMonster>();

            print("Current monsters amount: " + monsters.Length);
            Array.ForEach(monsters, m => m.BeingHit());
        }
        if (Input.GetKeyDown(KeyCode.N)) // Heavy Damage on each monsters
        {
            var monsters = FindObjectsOfType<AbstractMonster>();

            print("Current monsters amount: " + monsters.Length);
            Array.ForEach(monsters, m => m.BeingHit(100));
        }
        
        if (Input.GetKeyDown(KeyCode.A)) // Hit one monster
        {
            var monsters = FindObjectsOfType<AbstractMonster>();
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
            Debug.Log("Started: " + Game.Started);
            Debug.Log("CurrentStage: " + Game.GetCurrentStage());
            Debug.Log("Level: " + Game.Level);
        }
        if (Input.GetKey(KeyCode.H))
        {
            _player.transform.Rotate(new Vector3(0, -1, 0));
        }
        if (Input.GetKey(KeyCode.J))
        {
            _player.transform.Rotate(new Vector3(1, 0, 0));
        }
        if (Input.GetKey(KeyCode.K))
        {
            _player.transform.Rotate(new Vector3(-1, 0, 0));
        }
        if (Input.GetKey(KeyCode.L))
        {
            _player.transform.Rotate(new Vector3(0, 1, 0));
        }
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            _rightWeapon.GetComponent<WeaponB>().RangeHitTest();
        }
    }

    private void StartGame()
    {
        if (Game.Started)
        {
            return;
        }
        Game.Started = true;
        if (Instructions != null) // Hide instructions
        {
            Instructions.SetActive(false);
        }
        // Start introduction timeline
        var movableMap = GameObject.FindGameObjectWithTag("MovableMap");
        movableMap.GetComponent<PlayableDirector>().Play();
    }

    public void EarnExperience(int experience)
    {
        if (Game.IsTransitioning)
        {
            return;
        }

        Game.Level += experience;
        _gameInfoHud.CurrentExperience = Game.Level;

        _coinGenerator.SpawnGold(experience, gameObject.transform.position, gameObject.transform.rotation);

        if (Game.Level >= Data.stageThresholds[Game.GetCurrentStage()] && !_stageBossActive)
        {
            _stageBossActive = true;
            Game.StageBossManager.StartBossFight();
        }
    }

    public void LoseExperience(int experience)
    {
        if (Game.IsTransitioning)
        {
            return;
        }
        if (Game.Level < experience)
        {
            Game.Level = 0;
        } else
        {
            Game.Level -= experience;
        }
        _gameInfoHud.CurrentExperience = Game.Level;

        if (Game.GetCurrentStage() == 0)
        {
            return;
        }

        if (Game.Level <= Data.stageThresholds[Game.GetCurrentStage() - 1] * Data.levelDownThresholdFactor)
        {
            StageDown();
        }
    }

    public void StageUp()
    {
        if (Game.IsTransitioning) return;
        PlayStageUpSound();
        ChangeStage(Game.GetCurrentStage()+1);
    }

    private void StageDown()
    {
        if (Game.IsTransitioning) return;
        PlayStageDownSound();
        ChangeStage(Game.GetCurrentStage()-1);
    }

    private static void ChangeStage(int targetStage)
    {
        Game.IsTransitioning = true;
        var loading = SceneManager.LoadSceneAsync(targetStage);
        loading.allowSceneActivation = true;
    }

    private void PlayStageUpSound()
    {
        AudioSource.PlayOneShot(StageUpSound);
    }

    private void PlayStageDownSound()
    {
        AudioSource.PlayOneShot(StageDownSound);
    }

    private void LoadGameData()
    {
        var filePath = Path.Combine(Application.streamingAssetsPath, GameDataFileName);
        if (!File.Exists(filePath))
        {
            Debug.LogError("Cannot load game data!");
            return;
        }

        var dataAsJson = File.ReadAllText(filePath);
        Data = JsonUtility.FromJson<GameData>(dataAsJson);
    }

    public CoinGenerator GetCoinGenerator()
    {
        return _coinGenerator;
    }

    public BulletGenerator GetBulletGenerator()
    {
        return _bulletGenerator;
    }

    public MonsterGenerator GetMonsterGenerator()
    {
        return _monsterGenerator;
    }

    [Serializable]
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

        [Serializable]
        public class Stage
        {
            public string name;
            public MonsterData[] monsters;
            public string leftWeapon;
            public string rightWeapon;
            public bool isShotEnabled;
            public string audioTrack;
            public string audioTrackLoop;

            [Serializable]
            public class MonsterData
            {
                public string name;
                public float spawnPeriod;
            }
        }
    }
}
