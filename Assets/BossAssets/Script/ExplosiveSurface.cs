using UnityEngine;
using System.Collections;

public class ExplosiveSurface : MonoBehaviour 
{
	public GameObject ExplosionFX;
	public bool ExplosionEnabled;

	private int HP = 10;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			TriggerExplosion(transform.position);
		}
	}

	public void TriggerExplosion(Vector3 position = (transform.position + transform.forward))
	{
		if (ExplosionEnabled)
		{
			GameObject FX = Instantiate(ExplosionFX, position, Quaternion.identity) as GameObject;
			HP --;
			if (HP < 0)
			{
				ExplosionEnabled = false;
				GetComponent<MeshRenderer>().material = GameObject.FindObjectOfType<BossFightManager>().RedEyeMaterial;
				GameObject.FindObjectOfType<BossFightManager>().EyesBrokenStage1 ++;
			}
		}
	}
}
