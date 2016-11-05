using UnityEngine;
using System.Collections;

public class PlayerB : MonoBehaviour {

	int level = 0;
	long [] treshs = {  100, 100000, 100000000, 100 000 000 000, 100 000 000 000 000   }
	WeaponB weaponB;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void levelUp(int levels) {
		level += levels;

	}

	void levelDown(int levels) {
		level -= levels;
	}

	void equipWeapon (WeaponB weaponB) {
		this.weaponB = weaponB;
	}


}
