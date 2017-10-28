using UnityEngine;
using System.Collections;

public abstract class AbstractMonster : MonoBehaviour, Monster {
    public int hp { get; set; }
    public int experience { get; set; }


	public  void BeingHit()
    {
        if (hp > 1)
        {
            hp--;
        }
        else
        {
            GameManager player = GameObject.FindObjectOfType(typeof(GameManager)) as GameManager;
            if (player != null)
            {
                player.LevelUp(experience);
            }
            DestroyImmediate(this.gameObject);
        }

    }


}
