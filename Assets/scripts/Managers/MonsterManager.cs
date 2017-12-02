using UnityEngine;
using System.Collections.Generic;

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
	private SpawnerB[] spawners;
	private GameManager gm;
    private int spawnPeriodByFrame;

	void Start () {
        gm = gameObject.GetComponent<GameManager>();

        spawners = GameObject.FindObjectsOfType(typeof(SpawnerB)) as SpawnerB[];

        spawnPeriodByFrame = Mathf.RoundToInt(gm.gameData.spawnPeriod * 30);
    }

    void Update () {
        if (spawners.Length == 0) {
            return;
        }

        if (Time.frameCount%spawnPeriodByFrame == 0) { // Framerate
            if (GameObject.FindObjectsOfType(typeof(AbstractMonster)).Length < gm.gameData.maxAmountMonsters) { // Max amount of monsters
                SpawnMob();
            }
		}
    }

    /// <summary>
    /// Spawn 1 mob at a random spawner
    /// </summary>
    void SpawnMob() {
        if (!gm.started) {
            return;
        }
		spawners[UnityEngine.Random.Range (0, spawners.Length)].Spawn(ChooseRandomMobType());
	}


    string ChooseRandomMobType() {
        string[] mobTypes = gm.gameData.stages[gm.currentStage].monsters;
        int mobIndex = UnityEngine.Random.Range(0, mobTypes.Length);
        return mobTypes[mobIndex];
	}

}
