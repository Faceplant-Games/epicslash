using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MonsterManager : MonoBehaviour {
	private List<SpawnerB> spawns = new List<SpawnerB> ();
	private Dictionary<string,int>[] ennemies;


	// Use this for initialization
	void Start () {
		ennemies  = new Dictionary<string,int>[5];// nombre de niveaux de jeux



	}
	
	// Update is called once per frame
	void Update () {
	
	}


	void Spawn(){
		// Find a random index between zero and one less than the number of spawn points.
		spawns[Random.Range (0, spawns.Count)].Spawn(this.findEnnemyTag());

	}

	string findEnnemyTag(){
		return ennemies [0];
	}


	void addEnnemyTag(string tag){
		ennemies.Add (tag);
	}

}
