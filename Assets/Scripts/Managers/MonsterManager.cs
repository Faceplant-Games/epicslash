using UnityEngine;

/// <summary>
/// Manages Monsters of the stage, including:
///     - Getting the specific monsters of this stage.
///     - Regularly spawning monsters
/// 
/// Link it to the same Empty Game Object as GameManager.
/// </summary>
/// <seealso cref="GameManager"/>
/// <seealso cref="MonsterGenerator"/>

[RequireComponent(typeof(GameManager))]
public class MonsterManager : MonoBehaviour {
    private int[] _spawnPeriodsByFrame;
    [Range(-360, 360)]
    public int AngleMin = -45;
    [Range(-360, 360)]
    public int AngleMax = 135; 
    [Range(0, 150)]
    public int MagnitudeMin = 50;
    [Range(0, 150)]
    public int MagnitudeMax = 80;

    private void Update () {
        if (_spawnPeriodsByFrame == null)
        {
            InitializeSpawnPeriodByFrame();
        }

        if (!IsMaxAmountMonstersReached()) return;
        for (var i = 0; i < GetCurrentStageMonsters().Length; i++)
        {
            if (_spawnPeriodsByFrame[i] != 0 && Time.frameCount % _spawnPeriodsByFrame[i] == 0)
            {
                SpawnMob(GetCurrentStageMonsters()[i].name);
            }
        }
    }

    private static bool IsMaxAmountMonstersReached() // TODO enhance this algorithm on performance issue
    {
        return FindObjectsOfType(typeof(AbstractMonster)).Length < Game.GameManager.Data.maxAmountMonsters;
    }

    private void InitializeSpawnPeriodByFrame()
    {
        _spawnPeriodsByFrame = new int[Game.GameManager.Data.stages[Game.GetCurrentStage()].monsters.Length];
        for (var i = 0; i < Game.GameManager.Data.stages[Game.GetCurrentStage()].monsters.Length; i++)
        {
            _spawnPeriodsByFrame[i] = ConvertToFramePeriod(GetCurrentStageMonsters()[i].spawnPeriod);
        }
    }

    private static int ConvertToFramePeriod(float period)
    {
        return Mathf.RoundToInt(period * 30);
    }

    private GameManager.GameData.Stage.MonsterData[] GetCurrentStageMonsters()
    {
        return Game.GameManager.Data.stages[Game.GetCurrentStage()].monsters;
    }

    /// <summary>
    /// Spawn a random mob in front of the player (distance and magnitude of spawn are customizable)
    /// </summary>
    private void SpawnMob(string monsterName) {
        if (!Game.Started) {
            return;
        }
        
        float random = Random.Range(AngleMin, AngleMax);
        float magnitude = Random.Range(MagnitudeMin, MagnitudeMax);
        var pos = new Vector3(Mathf.Cos(Mathf.Deg2Rad * random) * magnitude, 0, Mathf.Sin(Mathf.Deg2Rad * random) * magnitude);
        var monster = GetComponent<GameManager>().GetMonsterGenerator().GetMonsterFromName(monsterName);
        monster.transform.position += pos;
	}
}
