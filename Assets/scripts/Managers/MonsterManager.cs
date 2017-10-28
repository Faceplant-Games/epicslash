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
	private List<string> [] mobTypesByStage; // TODO Revrieve it from config file
	private GameManager gm;
    private int numberOfStages = 5;

	void Start () {
        gm = gameObject.GetComponent<GameManager>();

        InitializeMobTypesByStage();

        spawners = GameObject.FindObjectsOfType(typeof(SpawnerB)) as SpawnerB[];
    }

    // Update is called once per frame
    void Update () {
        if (spawners.Length == 0) {
            return;
        }
		if (Random.Range (0, 10) < 1) { // This is the frame rate. (0,10) < 1 means 10%
            if (GameObject.FindObjectsOfType(typeof(AbstractMonster)).Length < 100) {
                SpawnMob();
            }
		}
	}
    
    private void InitializeMobTypesByStage() {
        mobTypesByStage = new List<string>[numberOfStages];

        for (int i = 0; i < numberOfStages; i++) {
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
		spawners[Random.Range (0, spawners.Length)].Spawn(this.ChooseRandomMobType());
	}


    string ChooseRandomMobType() {
		int stage = gm.stage;
		List<string> mobTypes = mobTypesByStage [stage];
		int r = Random.Range (0, mobTypes.Count);
		return mobTypes [r % (mobTypes.Count)];
	}

}
