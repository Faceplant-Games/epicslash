using UnityEngine;
using System.Collections;

public class WeaponB : MonoBehaviour {
	public BulletB bulletPrefab;
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

    IEnumerator LongVibration(float length, ushort strength)
    {
        for (float i = 0; i < length; i += Time.deltaTime)
        {
            if (Game.gameManager.gameData.hasController)
            {
                SteamVR_Controller.Input((int)trackedController.controllerIndex).TriggerHapticPulse(strength);
            }
            yield return null; //every single frame for the duration of "length" you will vibrate at "strength" amount
        }
    }

    void OnTriggerEnter(Collider collider) {
        audioSource.PlayOneShot(slash);
        if (collider.gameObject.GetComponent<AbstractMonster>() != null) {
            if (Game.gameManager.gameData.hasController)
            {
                StartCoroutine(LongVibration(0.7f, 1500));
            }
            collider.gameObject.GetComponent<AbstractMonster>().BeingHit();
		}
	}

	void RangeHit(object sender, ClickedEventArgs e) 
	{
        if (!isShotEnabled)
            return;
        StartCoroutine(LongVibration(0.85f, 3000));
        BulletB bullet = Instantiate (bulletPrefab) as BulletB;
		bullet.transform.rotation = barrelEndTransform.rotation;
		bullet.transform.position = barrelEndTransform.position;
		bullet.transform.Rotate(-180, 0, 0);
	}

    public static GameObject CreateWeapon(string weapon, Transform parent, AudioSource audioSource)
    {

        GameObject current = Instantiate<GameObject>(Resources.Load<GameObject>(weapon), parent);
        current.name = "CurrentWeapon";
        current.GetComponent<WeaponB>().isShotEnabled = Game.gameManager.gameData.stages[Game.GetCurrentStage()].isShotEnabled;
        current.GetComponent<WeaponB>().trackedController = parent.GetComponent<SteamVR_TrackedController>();
        current.GetComponent<WeaponB>().audioSource = audioSource;
        return current;
    }
}
