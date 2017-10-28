using UnityEngine;
using System.Collections;

public abstract class AbstractMonster : MonoBehaviour, Monster {
	public abstract void Spawn();
	public abstract int GetExperience();
	public abstract void BeingHit();

}
