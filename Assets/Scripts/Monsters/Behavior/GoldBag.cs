using UnityEngine;
using System.Collections;

public class GoldBag : MonoBehaviour 
{
	public float GoldValue;

	private float timeEllapsed = 0;

	void Start() {
	}

	void FixedUpdate() {
		timeEllapsed += Time.fixedDeltaTime;

		if (timeEllapsed >= 5f) {
			Destroy (gameObject);
		}
	}
}
