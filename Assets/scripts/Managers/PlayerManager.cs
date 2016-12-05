using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour {

    public Fading fading;

    int level;
	int stage;
	long[] treshs = { 100, 100000, 100000000, 100000000000, 100000000000000 }; // TODO GameManager should manage this



    List<GoldSpawnerB> goldSpawners = new List<GoldSpawnerB>();
    GameManager gm;

    WeaponB weaponB; // TODO it seems to be useless. Remove this?

	// Use this for initialization
	void Start () {
		level = 0;
        gm = this.gameObject.GetComponent<GameManager>(); // TODO put a Require of GameManager
        stage = gm.stage;
        GoldSpawnerB[] goldS = GameObject.FindObjectsOfType (typeof(GoldSpawnerB)) as GoldSpawnerB[];
		foreach (GoldSpawnerB spawner in goldS) {
			goldSpawners.Add(spawner);
		}
    }
	
	// Update is called once per frame
	void Update () {
	
	}

	public void levelUp(int levels) { // TODO GameManager should call levelUp() and spawnGold()
		level += levels;
		spawnGold(levels % 37); // TODO make a better system (e.g. goldbag, treasure, diamonds...)
        print("Level: "+level); // TODO make an ingame display instead
		if (level >= treshs [stage])
        {
            StartCoroutine(stageUp()); // TODO GameManager should manage this
        }
    }

    private IEnumerator stageUp() // TODO GameManager should manage this
    {
        stage++;
        gm.stage = stage;
        gm.change = true;

        if (fading != null)
        {
            float fadeTime = fading.BeginFade(1);
            gm.playLevelUpSound();
            yield return new WaitForSeconds(fadeTime*7);
        }

        SceneManager.LoadScene(stage);

        if (stage > treshs.Length)
        {
            //stop the game
        }
    }


    public void spawnGold(int levels) { // TODO GameManager should manage this
		// random on goldSpawners.length, to pop some gold bags
		goldSpawners[Random.Range (0, goldSpawners.Count)].Spawn(levels);
	}

	public void levelDown(int levels) { // FIXME leveldown and stage down
		/*level -= levels;
        print("level down");
        if (stage >0)
        {
            if ( level < treshs[stage-1] ) {
				stage--;
				gm.stage = stage;
				gm.change = true;
				//FIXME PAUSE
			}
		}*/
	}

	void equipWeapon (WeaponB weaponB) { // TODO It seems to be useless. Remove this?
		this.weaponB = weaponB;
	}
}
