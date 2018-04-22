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
	private GameManager gm;
    private int spawnPeriodByFrame;
    [Range(-360, 360)]
    public int angleMin;
    [Range(-360, 360)]
    public int angleMax; 
    [Range(0, 150)]
    public int magnitudeMin;
    [Range(0, 150)]
    public int magnitudeMax;

    void Start () {
        gm = gameObject.GetComponent<GameManager>();

        spawnPeriodByFrame = Mathf.RoundToInt(gm.gameData.spawnPeriod * 30);
    }

    void Update () {
        if (spawnPeriodByFrame != 0 && Time.frameCount%spawnPeriodByFrame == 0) { // Framerate
            if (GameObject.FindObjectsOfType(typeof(AbstractMonster)).Length < gm.gameData.maxAmountMonsters) { // Max amount of monsters
                SpawnMob();
            }
		}
    }

    /// <summary>
    /// Spawn a random mob in front of the player (distance and magnitude of spawn are customizable)
    /// </summary>
    void SpawnMob() {
        if (!Game.started) {
            return;
        }
        
        string prefabName = ChooseRandomMobType();
        if (prefabName != "")
        {
            float random = Random.Range(angleMin, angleMax);
            float magnitude = Random.Range(magnitudeMin, magnitudeMax);
            Vector3 pos = new Vector3(Mathf.Cos(Mathf.Deg2Rad * random) * magnitude, 0, Mathf.Sin(Mathf.Deg2Rad * random) * magnitude);
            GameObject monster = Instantiate(Resources.Load<GameObject>(prefabName));
            monster.transform.position += pos;
            //this.gameObject.transform.rotation

        }
	}


    string ChooseRandomMobType() {
        string[] mobTypes = gm.gameData.stages[Game.GetCurrentStage()].monsters;
        if (mobTypes.Length == 0)
        {
            return "";
        }
        int mobIndex = Random.Range(0, mobTypes.Length);
        return mobTypes[mobIndex];
	}

}
