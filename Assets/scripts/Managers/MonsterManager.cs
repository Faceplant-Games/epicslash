using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MonsterManager : MonoBehaviour {
	private List<SpawnerB> spawns = new List<SpawnerB> ();
	private List<string> [] ennemies;
	private GameManager gm;

	// Use this for initialization
	void Start () {

		gm = gameObject.GetComponent<GameManager>();
		ennemies  = new List<string>[5];// nombre de niveaux de jeux
		for (int i = 0; i < 5; i++) {
			ennemies[i] =  new List<string>();
		}
		ennemies [0].Add ("Monster1");

		ennemies [1].Add ("Monster1");
		ennemies [1].Add ("Monster2");
		ennemies [1].Add ("Monster3");

		ennemies [2].Add ("Monster1");
		ennemies [2].Add ("Monster2");
		ennemies [2].Add ("Monster3");
		ennemies [2].Add ("dragon");

		ennemies [3].Add ("");

		ennemies [4].Add ("");

		SpawnerB[] spawnerB = GameObject.FindObjectsOfType (typeof(SpawnerB)) as SpawnerB[];



		foreach (SpawnerB spawner in spawnerB) {
			spawns.Add(spawner);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (Random.Range (0, 10) < 1) {// changer ça pour modifier le spawn rate
			if (GameObject.FindObjectsOfType (typeof(AbstractMonster)).Length < 100)
				Spawn ();
		}
	}



	void Spawn(){
		// Find a random index between zero and one less than the number of spawn points.
		spawns[Random.Range (0, spawns.Count)].Spawn(this.findEnnemyTag());

	}

	string findEnnemyTag(){//FIXME ajouter les differents types de monstres

		int stage = gm.stage;
		List<string> spawnsrattttt = ennemies [stage];
		int r = Random.Range (0, 12);
		return spawnsrattttt [r % (spawnsrattttt.Count)];
	}

}
