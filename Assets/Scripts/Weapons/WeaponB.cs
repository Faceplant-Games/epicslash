using UnityEngine;
using System.Collections;


public class WeaponB : MonoBehaviour {
	public BulletB bulletPrefab;
	public Transform barrelEndTransform;
    public SteamVR_TrackedController trackedController;
    public SteamVR_Controller.Device device;

	public AudioClip slash;
    public AudioClip[] shoot;
	public AudioSource audioSource;
    public bool isShotEnabled;

    // Use this for initialization
    void Start () {
        trackedController.TriggerClicked += new ClickedEventHandler(RangeHit);
    }

    void FixedUpdate()
    {
        int index = (int)trackedController.controllerIndex;
        if (index >= 0)
            device = SteamVR_Controller.Input(index);
    }

    IEnumerator LongVibration(float length, ushort strength)
    {
        for (float i = 0; i < length; i += Time.deltaTime)
        {
            if (Game.GameManager.Data.hasController)
            {
                SteamVR_Controller.Input((int)trackedController.controllerIndex).TriggerHapticPulse(strength);
            }
            yield return null; //every single frame for the duration of "length" you will vibrate at "strength" amount
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (device.velocity.magnitude > 1.5)
        { 
            if (collider.gameObject.GetComponent<AbstractMonster>() != null)
            {
                audioSource.PlayOneShot(slash);
                if (Game.GameManager.Data.hasController)
                {
                    StartCoroutine(LongVibration(0.7f, 1500));
                }
                collider.gameObject.GetComponent<AbstractMonster>().BeingHit();
            }
		}
    }

    void RangeHit(object sender, ClickedEventArgs e)
    {
        if (!isShotEnabled)
            return;
        if (shoot.Length > 0)
            audioSource.PlayOneShot(shoot[(int)(Random.Range(0, shoot.Length) % shoot.Length)], .5f);
        StartCoroutine(LongVibration(0.7f, 1500));
        BulletB bullet = Instantiate(bulletPrefab) as BulletB;
        bullet.audioSource = audioSource;
        bullet.transform.rotation = barrelEndTransform.rotation;
        bullet.transform.position = barrelEndTransform.position + bullet.transform.forward * -0.1f;
        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * -20;
        bullet.transform.Rotate(-180, 0, 0);
    }

    public void RangeHitTest()
    {
        if (!isShotEnabled)
            return;
        if (shoot.Length > 0)
            audioSource.PlayOneShot(shoot[(int)(Random.Range(0, shoot.Length) % shoot.Length)], .75f);
        StartCoroutine(LongVibration(0.85f, 3000));
        BulletB bullet = Instantiate(bulletPrefab) as BulletB;
        bullet.audioSource = audioSource;
        bullet.transform.rotation = barrelEndTransform.rotation;
        bullet.transform.position = barrelEndTransform.position + bullet.transform.forward * -0.1f;
        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 20 * -1;
        bullet.transform.Rotate(-180, 0, 0);
    }

    public static GameObject CreateWeapon(string weapon, Transform parent, AudioSource audioSource)
    {

        GameObject current = Instantiate<GameObject>(Resources.Load<GameObject>(weapon), parent);
        current.name = "CurrentWeapon";
        current.GetComponent<WeaponB>().isShotEnabled = Game.GameManager.Data.stages[Game.GetCurrentStage()].isShotEnabled;
        current.GetComponent<WeaponB>().trackedController = parent.GetComponent<SteamVR_TrackedController>();
        current.GetComponent<WeaponB>().audioSource = audioSource;
        return current;
    }
}
