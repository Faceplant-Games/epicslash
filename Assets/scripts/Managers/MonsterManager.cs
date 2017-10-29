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
public class MonsterManager : MonoBehaviour {
	private SpawnerB[] spawners;
	private List<string> [] mobTypesByStage; // TODO Retrieve it from config file
	private GameManager gm;
    private int spawnPeriodByFrame;

	void Start () {
        gm = gameObject.GetComponent<GameManager>();

        InitializeMobTypesByStage();

        spawners = GameObject.FindObjectsOfType(typeof(SpawnerB)) as SpawnerB[];

        spawnPeriodByFrame = Mathf.RoundToInt(gm.gameData.spawnPeriod * 30);

        print("spawnPeriodByFrame: " + spawnPeriodByFrame);
    }

    // Update is called once per frame
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
    
    private void InitializeMobTypesByStage() {
        mobTypesByStage = new List<string>[gm.gameData.numberOfStages];

        for (int i = 0; i < gm.gameData.numberOfStages; i++) {
            mobTypesByStage[i] = new List<string>();
        }
        mobTypesByStage[0].Add("Monster1"); // TODO Retrieve it from config file

        mobTypesByStage[1].Add("Monster1");
        mobTypesByStage[1].Add("Monster2");
        mobTypesByStage[1].Add("Monster3");

        mobTypesByStage[2].Add("Monster1");
        mobTypesByStage[2].Add("Monster2");
        mobTypesByStage[2].Add("Monster3");
        mobTypesByStage[2].Add("dragon");

        mobTypesByStage[3].Add("Monster1");
        mobTypesByStage[3].Add("Monster2");
        mobTypesByStage[3].Add("Monster3");
        mobTypesByStage[3].Add("dragon");
    }

    // Spawn 1 mob at a random spawner
    void SpawnMob() {
		spawners[UnityEngine.Random.Range (0, spawners.Length)].Spawn(ChooseRandomMobType());
	}


    string ChooseRandomMobType() {
		int stage = gm.currentStage;
		List<string> mobTypes = mobTypesByStage [stage];
		int r = UnityEngine.Random.Range (0, mobTypes.Count);
		return mobTypes [r % (mobTypes.Count)];
	}

}
