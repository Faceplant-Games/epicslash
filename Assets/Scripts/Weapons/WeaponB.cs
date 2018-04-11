using UnityEngine;
using System.Collections;

public class WeaponB : MonoBehaviour {
	public BulletB bulletPrefab;
    public GameObject fireMobile;
	public Transform barrelEndTransform;
    public SteamVR_TrackedController trackedController;

	public AudioClip slash;
	public AudioSource audioSource;
    public bool isShotEnabled;


    // Use this for initialization
    void Start () {
	    trackedController.TriggerClicked += new ClickedEventHandler(RangeHit);
	}
	
	// Update is called once per frame
	void Update () {
	}

	void OnTriggerEnter(Collider collider) {
        audioSource.PlayOneShot(slash);
        if (collider.gameObject.GetComponent<AbstractMonster>() != null) {
			SteamVR_Controller.Input((int)trackedController.controllerIndex).TriggerHapticPulse((ushort)Mathf.Lerp(0f, 1500f, 0.7f));
			collider.gameObject.GetComponent<AbstractMonster>().BeingHit();
		}

	}

	void RangeHit(object sender, ClickedEventArgs e) 
	{
        if (!isShotEnabled)
            return;
		SteamVR_Controller.Input((int)trackedController.controllerIndex).TriggerHapticPulse((ushort)Mathf.Lerp(0f, 3000f, 0.85f));
		BulletB bullet = Instantiate (bulletPrefab) as BulletB;
        GameObject fire = Instantiate(fireMobile);
        fire.transform.position = Vector3.zero;
        fire.transform.SetParent(bullet.transform, false);
		bullet.transform.rotation = barrelEndTransform.rotation;
		bullet.transform.position = barrelEndTransform.position;
		bullet.transform.Rotate(-180, 0, 0);
	}

    public static GameObject CreateWeapon(string weapon, Vector3 pos, Quaternion rotation, Transform controller, AudioSource audioSource)
    {
        GameObject current = Instantiate<GameObject>(Resources.Load<GameObject>(weapon), pos, rotation);
        current.name = "CurrentWeapon";
        current.GetComponent<WeaponB>().isShotEnabled = Game.gameManager.gameData.stages[Game.GetCurrentStage()].isShotEnabled;
        current.transform.parent = controller;
        current.GetComponent<WeaponB>().trackedController = controller.GetComponent<SteamVR_TrackedController>();
        current.GetComponent<WeaponB>().audioSource = audioSource;
        return current;
    }
}
