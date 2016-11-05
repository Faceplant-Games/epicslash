using UnityEngine;
using System.Collections;

public class PlayerB : MonoBehaviour {

	int level;
	int stage;
	long[] treshs = { 100, 100000, 100000000, 100000000000, 100000000000000 };
	WeaponB weaponB;

	// Use this for initialization
	void Start () {
		level = 0;
		stage = 0;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void levelUp(int levels) {
		level += levels;
		if (level >= treshs [stage]) {
			stage++;
			//fire event
			if (stage > treshs.Length) {
				//stop the game
			}	
		}
	}

	void levelDown(int levels) {
		level -= levels;

	}

	void equipWeapon (WeaponB weaponB) {
		this.weaponB = weaponB;
	}



}
