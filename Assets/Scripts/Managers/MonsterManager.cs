using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages Monsters of the stage, including:
///     - Fetching Monster Spawners.
///     - Getting the specific monsters of this stage.
///     - Regularly spawning monsters
/// 
/// Link it to the same Empty Game Object as GameManager.
/// </summary>
/// <seealso cref="GameManager"/>
/// <seealso cref="SpawnerB"/>

[RequireComponent(typeof(GameManager))]
public class MonsterManager : MonoBehaviour {
	private GameManager gm;
    private int[] spawnPeriodsByFrame;
    [Range(-360, 360)]
    public int angleMin;
    [Range(-360, 360)]
    public int angleMax; 
    [Range(0, 150)]
    public int magnitudeMin;
    [Range(0, 150)]
    public int magnitudeMax;

    void Start () {
        gm = Game.gameManager;
        InitializeSpawnPeriodByFrame();
    }

    void Update () {
        if (GameObject.FindObjectsOfType(typeof(AbstractMonster)).Length < gm.gameData.maxAmountMonsters) // Max amount of monsters
        { 
            for (int i = 0; i < GetCurrentStageMonsters().Length; i++)
            {
                if (spawnPeriodsByFrame[i] != 0 && Time.frameCount % spawnPeriodsByFrame[i] == 0)
                {
                    SpawnMob(GetCurrentStageMonsters()[i].name);
                }
            }
        }
    }

    private void InitializeSpawnPeriodByFrame()
    {
        spawnPeriodsByFrame = new int[gm.gameData.stages[Game.GetCurrentStage()].monsters.Length];
        for (int i = 0; i < gm.gameData.stages[Game.GetCurrentStage()].monsters.Length; i++)
        {
            spawnPeriodsByFrame[i] = ConvertToFramePeriod(GetCurrentStageMonsters()[i].spawnPeriod);
        }
    }

    private int ConvertToFramePeriod(float period)
    {
        return Mathf.RoundToInt(period * 30);
    }

    private GameManager.GameData.Stage.MonsterData[] GetCurrentStageMonsters()
    {
        return gm.gameData.stages[Game.GetCurrentStage()].monsters;
    }

    /// <summary>
    /// Spawn a random mob in front of the player (distance and magnitude of spawn are customizable)
    /// </summary>
    private void SpawnMob(string monsterName) {
        if (!Game.started) {
            return;
        }
        
        float random = Random.Range(angleMin, angleMax);
        float magnitude = Random.Range(magnitudeMin, magnitudeMax);
        Vector3 pos = new Vector3(Mathf.Cos(Mathf.Deg2Rad * random) * magnitude, 0, Mathf.Sin(Mathf.Deg2Rad * random) * magnitude);
        GameObject monster = Instantiate(Resources.Load<GameObject>(monsterName));
        monster.transform.position += pos;
	}
}
