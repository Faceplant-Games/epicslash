﻿using UnityEngine;
using System.Collections;

public class Ennemi3BulletB : MonoBehaviour 
{

	float timer = 0;
	float lifeTime = 60;
	float speed = 2;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void FixedUpdate () {
		float distanceThisFrame = speed * Time.fixedDeltaTime;
		RaycastHit hit = new RaycastHit ();

		// At each frame, we cast a ray forward from where we are to where we will be next frame
		if (Physics.Raycast (transform.position, transform.forward, out hit, distanceThisFrame)) 
		{
			if (hit.transform.gameObject.tag == "MainCamera") 
			{
                PlayerB player = GameObject.FindObjectOfType(typeof(PlayerB)) as PlayerB;
                player.levelDown(5);
            }
            Destroy(gameObject);
        } 
		else 
		{
			transform.position += transform.forward * distanceThisFrame;
		}

		timer += Time.fixedDeltaTime;
		if (timer > lifeTime) {
			Destroy(gameObject);
		}
	}
}