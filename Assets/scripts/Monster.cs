using System;
using UnityEngine;

public interface Monster : MonoBehaviour
	{
		void Spawn();
		int Experience();
		void Die();

	}

