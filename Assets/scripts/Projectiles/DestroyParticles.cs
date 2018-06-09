using UnityEngine;

public class DestroyParticles : MonoBehaviour {
    
	private void Start () {
        Destroy(gameObject, gameObject.GetComponentInChildren<ParticleSystem>().duration);
	}
}
