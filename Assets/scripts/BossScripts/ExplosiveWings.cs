using UnityEngine;
using System.Collections;

public class ExplosiveWings : MonoBehaviour 
{
	public GameObject ExplosionFX;

	private bool _explosionEnabled = false;
	public bool ExplosionEnabled
	{
		get
		{
			return _explosionEnabled;
		}
		set
		{
			_explosionEnabled = value;
			if (_explosionEnabled == true)
			{
				GetComponent<MeshRenderer>().material = GameObject.FindObjectOfType<BossFightManager>().WhiteEyeMaterial;
			}
			else
			{
				GetComponent<MeshRenderer>().material = GameObject.FindObjectOfType<BossFightManager>().BlueWingsMaterial;
			}
		}
	}

	public int HP = 30;

	public void TriggerExplosion(Vector3 position)
	{
		if (ExplosionEnabled)
		{
			GameObject FX = Instantiate(ExplosionFX);
            FX.transform.position = position;
			HP --;
			FindObjectOfType<HellFireManager>().PLayRandomSFXExplosion(position);
			if (HP < 0)
			{
				ExplosionEnabled = false;
				GetComponent<MeshRenderer>().material = GameObject.FindObjectOfType<BossFightManager>().BlueWingsMaterial;
				GameObject.FindObjectOfType<BossFightManager>().WingsBrokenStage2 ++;
				Destroy(gameObject);
			}
		}
	}
}
