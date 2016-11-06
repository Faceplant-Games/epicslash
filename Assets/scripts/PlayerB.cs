using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class PlayerB : MonoBehaviour {

	int level;
	int stage;
	long[] treshs = { 100, 100000, 100000000, 100000000000, 100000000000000 };




	List<GoldSpawnerB> goldSpawners = new List<GoldSpawnerB>();
    GameManager gm;

    WeaponB weaponB;

	// Use this for initialization
	void Start () {
		level = 0;
		stage = 0;
        gm = this.gameObject.GetComponent<GameManager>();
		GoldSpawnerB[] goldS = GameObject.FindObjectsOfType (typeof(GoldSpawnerB)) as GoldSpawnerB[];
		foreach (GoldSpawnerB spawner in goldS) {
			goldSpawners.Add(spawner);
		}
    }
	
	// Update is called once per frame
	void Update () {
	
	}

	public void levelUp(int levels) {
		level += levels;
		spawnGold(levels % 37);
        print(level);
		if (level >= treshs [stage]) {
			stage++;
			gm.stage = stage;
			gm.change = true;
            //SceneManager.LoadScene("BossFight");
			//FIXME PAUSE
			if (stage > treshs.Length) {
				//stop the game
			}	
		}
	}

	public void spawnGold(int levels) {
		// random on goldSpawners.length, to pop some gold bags
		goldSpawners[Random.Range (0, goldSpawners.Count)].Spawn(levels);
	}

	public void levelDown(int levels) {
		level -= levels;
        print("level down");
        if (stage >0)
        {
            if ( level < treshs[stage-1] ) {
				stage--;
				gm.stage = stage;
				gm.change = true;
				//FIXME PAUSE
			}
		}
	}

	void equipWeapon (WeaponB weaponB) {
		this.weaponB = weaponB;
	}
}
