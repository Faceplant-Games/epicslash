using UnityEngine;
using System.Collections;

public class ExplosiveSurface : MonoBehaviour 
{
	public GameObject ExplosionFX;
	public bool ExplosionEnabled;

	private int _hp = 10;

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
		var fx = Instantiate(ExplosionFX);
		fx.transform.position = position;
            
		_hp--;
		if (_hp < 0)
		{
			ExplosionEnabled = false;
			GetComponent<MeshRenderer>().material = FindObjectOfType<BossFightManager>().RedEyeMaterial;
			FindObjectOfType<BossFightManager>().EyesBrokenStage1 ++;
		}
	}
}
