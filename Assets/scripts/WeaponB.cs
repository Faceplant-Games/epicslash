using UnityEngine;
using System.Collections;

public class WeaponB : MonoBehaviour {
	public BulletB bulletPrefab;
	public Transform barrelEndTransform;

	// Use this for initialization
	void Start () {
		GetComponent<SteamVR_TrackedController>().TriggerClicked += new ClickedEventHandler(RangeHit);

	}
	
	// Update is called once per frame
	void Update () {
		PhysicalHit ();
	}

	void PhysicalHit() {
		// Get all mobs
		AbstractMonster[] monsters = GameObject.FindObjectsOfType(typeof(AbstractMonster)) as AbstractMonster[];
		foreach (AbstractMonster monster in monsters) {
			// Is touching ?
			float dist = Vector3.Distance(transform.position, monster.gameObject.transform.position);
			if (dist < 1) { // Touching = under 1 distance unit
				monster.Die();
			}
		}
	}

	void RangeHit(object sender, ClickedEventArgs e) 
	{
		BulletB bullet = Instantiate (bulletPrefab) as BulletB;
		bullet.transform.rotation = barrelEndTransform.rotation;
		bullet.transform.position = barrelEndTransform.position;
		bullet.transform.Rotate(-90, 0, 0);
	}
}
