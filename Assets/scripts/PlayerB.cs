using UnityEngine;
using System.Collections;

public class PlayerB : MonoBehaviour {

	int level = 0;
	WeaponB weaponB;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void equipWeapon (WeaponB weaponB) {
		this.weaponB = weaponB;
	}
}
