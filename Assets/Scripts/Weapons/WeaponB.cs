using UnityEngine;
using System.Collections;


public class WeaponB : MonoBehaviour {
	public BulletB BulletPrefab;
	public Transform BarrelEndTransform;
    public SteamVR_TrackedController TrackedController;
    public SteamVR_Controller.Device Device;

	public AudioClip[] Slash;
    public AudioClip[] Shoot;
	public AudioSource AudioSource;
    public bool IsShotEnabled;

    // Use this for initialization
    private void Start () {
        TrackedController.TriggerClicked += RangeHit;
        TrackedController.TriggerClicked += StartGame;
    }

    private void FixedUpdate()
    {
        var index = (int)TrackedController.controllerIndex;
        if (index >= 0)
            Device = SteamVR_Controller.Input(index);
    }

    private void LongVibration(ushort length)
    {
        if (Game.GameManager.Data.hasController)
        {
            SteamVR_Controller.Input((int)TrackedController.controllerIndex).TriggerHapticPulse(length);
        }
    }

    private void OnTriggerEnter(Collider enteredCollider)
    {
        if (Device.velocity.magnitude <= 1.5) return;
        if (enteredCollider.gameObject.GetComponent<AbstractMonster>() == null) return;
        if (Slash.Length > 0)
            AudioSource.PlayOneShot(Slash[(int)(Random.Range(0, Slash.Length) % Slash.Length)], .5f);
        if (Game.GameManager.Data.hasController)
        {
            LongVibration(700);
        }
        enteredCollider.gameObject.GetComponent<AbstractMonster>().BeingHit();
    }

    private void StartGame(object sender, ClickedEventArgs e)
    {
        Game.GameManager.StartGame();
    }

    private void RangeHit(object sender, ClickedEventArgs e)
    {
        if (!IsShotEnabled)
            return;
        if (Shoot.Length > 0)
            AudioSource.PlayOneShot(Shoot[(int)(Random.Range(0, Shoot.Length) % Shoot.Length)], .5f);
        LongVibration(700);
        var bullet = Instantiate(BulletPrefab);
        bullet.AudioSource = AudioSource;
        bullet.transform.rotation = BarrelEndTransform.rotation;
        bullet.transform.position = BarrelEndTransform.position + bullet.transform.forward * -0.1f;
        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * -20;
        bullet.transform.Rotate(-180, 0, 0);
    }

    public void RangeHitTest()
    {
        if (!IsShotEnabled) return;
        if (Shoot.Length > 0)
            AudioSource.PlayOneShot(Shoot[(int)(Random.Range(0, Shoot.Length) % Shoot.Length)], .75f);
        LongVibration(850);
        var bullet = Instantiate(BulletPrefab);
        bullet.AudioSource = AudioSource;
        bullet.transform.rotation = BarrelEndTransform.rotation;
        bullet.transform.position = BarrelEndTransform.position + bullet.transform.forward * -0.1f;
        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 20 * -1;
        bullet.transform.Rotate(-180, 0, 0);
    }

    public static GameObject CreateWeapon(string weapon, Transform parent, AudioSource audioSource)
    {
        var current = Instantiate(Resources.Load<GameObject>(weapon), parent);
        current.name = "CurrentWeapon";
        current.GetComponent<WeaponB>().IsShotEnabled = Game.GameManager.Data.stages[Game.GetCurrentStage()].isShotEnabled;
        current.GetComponent<WeaponB>().TrackedController = parent.GetComponent<SteamVR_TrackedController>();
        current.GetComponent<WeaponB>().AudioSource = audioSource;
        return current;
    }
}
