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

	void OnCollisionEnter(Collision collision) {
		if (collision.gameObject.GetComponent<AbstractMonster> () != null) {
			collision.gameObject.GetComponent<AbstractMonster> ().Die ();
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
