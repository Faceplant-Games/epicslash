using UnityEngine;
using System.Collections;

public abstract class AbstractMonster : MonoBehaviour, Monster {
	public abstract void Spawn();
	public abstract int Experience();
	public abstract void Die();

}
