using UnityEngine;
using System.Collections;

public class PlayerB : MonoBehaviour {

	int level;
	int stage;
	long[] treshs = { 100, 100000, 100000000, 100000000000, 100000000000000 };
    GameManager gm;

    WeaponB weaponB;

	// Use this for initialization
	void Start () {
		level = 0;
		stage = 0;
        gm = this.gameObject.GetComponent<GameManager>();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

	public void levelUp(int levels) {
		level += levels;

        print("level up");
        if (level >= treshs [stage]) {
			stage++;
			gm.stage = stage;
			gm.change = true;
			//FIXME PAUSE
			if (stage > treshs.Length) {
				//stop the game
			}	
		}
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
