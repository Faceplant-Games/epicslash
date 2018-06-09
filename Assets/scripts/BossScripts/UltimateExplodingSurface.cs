using UnityEngine;
using System.Collections;

public class UltimateExplodingSurface : MonoBehaviour 
{
	private int _hp = 15;

	private void TriggerExplosion(Vector3 position)
	{
		_hp--;
        HellFireManager.PlayRandomSfxExplosion(position);
        if (_hp < 0)
		{
			FindObjectOfType<BossFightManager>().EndGame(); // TODO Better use GameManager
		}
	}

	private void OnTriggerEnter(Collider col)
    {
        if (col.GetComponent<WeaponB>() != null || col.GetComponent(typeof(BulletB)) as BulletB)
        {
            TriggerExplosion(transform.position);
        }
    }
}
