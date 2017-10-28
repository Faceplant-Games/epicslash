using UnityEngine;
using System.Collections;

public class WeaponB : MonoBehaviour {
	public BulletB bulletPrefab;
	public Transform barrelEndTransform;
    public SteamVR_TrackedController trackedController;

	public AudioClip slash;
	public AudioSource audio;
    public bool isShotEnabled;


    // Use this for initialization
    void Start () {
	    trackedController.TriggerClicked += new ClickedEventHandler(RangeHit);
	}
	
	// Update is called once per frame
	void Update () {
	}

	void OnTriggerEnter(Collider collider) {
        audio.PlayOneShot(slash);
        if (collider.gameObject.GetComponent<AbstractMonster> () != null) {
			SteamVR_Controller.Input((int)trackedController.controllerIndex).TriggerHapticPulse((ushort)Mathf.Lerp(0f, 1500f, 0.7f));
			collider.gameObject.GetComponent<AbstractMonster> ().BeingHit ();
		}

	}

	void RangeHit(object sender, ClickedEventArgs e) 
	{
        if (!isShotEnabled)
            return;
		SteamVR_Controller.Input((int)trackedController.controllerIndex).TriggerHapticPulse((ushort)Mathf.Lerp(0f, 3000f, 0.85f));
		BulletB bullet = Instantiate (bulletPrefab) as BulletB;
		bullet.transform.rotation = barrelEndTransform.rotation;
		bullet.transform.position = barrelEndTransform.position;
		bullet.transform.Rotate(-180, 0, 0);
	}
}
