using UnityEngine;
using System.Collections;

public class ExplosiveWings : MonoBehaviour 
{
	public GameObject ExplosionFX;

	private bool _explosionEnabled;
	public bool ExplosionEnabled
	{
		get
		{
			return _explosionEnabled;
		}
		set
		{
			_explosionEnabled = value;
			if (_explosionEnabled)
			{
				GetComponent<MeshRenderer>().material = FindObjectOfType<BossFightManager>().WhiteEyeMaterial;
			}
			else
			{
				GetComponent<MeshRenderer>().material = FindObjectOfType<BossFightManager>().BlueWingsMaterial;
			}
		}
	}

	public int HP = 30;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent(typeof(BulletB)) as BulletB)
        {
            TriggerExplosion(other.transform.position);
        }
    }

	private void TriggerExplosion(Vector3 position)
	{
		if (!ExplosionEnabled) return;
		var FX = Instantiate(ExplosionFX);
		FX.transform.position = position;
		HP--;
		HellFireManager.PlayRandomSfxExplosion(position);
		if (HP < 0)
		{
			ExplosionEnabled = false;
			GetComponent<MeshRenderer>().material = FindObjectOfType<BossFightManager>().BlueWingsMaterial;
			FindObjectOfType<BossFightManager>().WingsBrokenStage2 ++;
			Destroy(gameObject);
		}
	}
}
