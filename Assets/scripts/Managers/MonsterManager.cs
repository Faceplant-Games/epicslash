using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MonsterManager : MonoBehaviour {
	private List<SpawnerB> spawns = new List<SpawnerB> ();
	private List<string> [] ennemies;
	private GameManager gm;

	void Start () {

		gm = gameObject.GetComponent<GameManager>();
		ennemies  = new List<string>[5]; // TODO rename this variable "type of enemies spawned in some level" or something like this
		for (int i = 0; i < 4; i++) {
			ennemies[i] =  new List<string>();
		}
		ennemies [0].Add ("Monster1"); // TODO Put this in a config file

        ennemies [1].Add ("Monster1");
		ennemies [1].Add ("Monster2");
		ennemies [1].Add ("Monster3");

		ennemies [2].Add ("Monster1");
		ennemies [2].Add ("Monster2");
		ennemies [2].Add ("Monster3");
		ennemies [2].Add ("dragon");

        ennemies[3].Add("Monster1");
        ennemies[3].Add("Monster2");
        ennemies[3].Add("Monster3");
        ennemies[3].Add("dragon");

      //  ennemies [4].Add ("");

		SpawnerB[] spawnerB = GameObject.FindObjectsOfType (typeof(SpawnerB)) as SpawnerB[]; // TODO rename this clearer



		foreach (SpawnerB spawner in spawnerB) { // TODO use lambdas, it's sexier : Array.ForEach(spawners, s => spawns.Add(s));
			spawns.Add(spawner); // TODO And... is it really useful?
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (spawns.Count > 0) {
			if (Random.Range (0, 10) < 1) {// changer ça pour modifier le spawn rate
				if (GameObject.FindObjectsOfType (typeof(AbstractMonster)).Length < 100)
					Spawn ();
			}
		}
	}



	void Spawn(){ // TODO rename it spawnMob
		// Find a random index between zero and one less than the number of spawn points.
		spawns[Random.Range (0, spawns.Count)].Spawn(this.findEnnemyTag());

	}

    // TODO rename this method "Give a random ennemi of this stage" or something like this
	string findEnnemyTag(){ //FIXME ajouter les differents types de monstres

		int stage = gm.stage;
		List<string> spawnsrattttt = ennemies [stage]; // Name this clearer
		int r = Random.Range (0, 12); // FIXME why 12?
		return spawnsrattttt [r % (spawnsrattttt.Count)];
	}

}
