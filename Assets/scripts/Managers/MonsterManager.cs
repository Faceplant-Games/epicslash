using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MonsterManager : MonoBehaviour {
	private List<SpawnerB> spawns = new List<SpawnerB> ();
	private Dictionary<string,int>[] ennemies;


	// Use this for initialization
	void Start () {
		ennemies  = new Dictionary<string,int>[5];// nombre de niveaux de jeux
		for (int i = 0; i < 5; i++) {
			ennemies[i] =  new Dictionary<string,int>();
		}
		ennemies [0].Add ("",100);

		ennemies [1].Add ("",100);

		ennemies [2].Add ("",100);

		ennemies [3].Add ("",100);

		ennemies [4].Add ("",100);

		SpawnerB[] spawnerB = GameObject.FindObjectsOfType (typeof(SpawnerB)) as SpawnerB[];

		foreach (SpawnerB spawner in spawnerB) {
			spawns.Add(spawner);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (Random.Range (0, 10) < 1) {// changer ça pour modifier le spawn rate
			if (GameObject.FindObjectsOfType (typeof(AbstractMonster)).Length < 50)
				Spawn ();
		}
	}



	void Spawn(){
		// Find a random index between zero and one less than the number of spawn points.
		spawns[Random.Range (0, spawns.Count)].Spawn(this.findEnnemyTag());

	}

	string findEnnemyTag(){//FIXME ajouter les differents types de monstres
		if (Random.Range (0, 10) < 5)
			return "Monster2";
		else
			return "Monster2";
	}

}
