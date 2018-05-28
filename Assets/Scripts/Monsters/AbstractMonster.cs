using UnityEngine;
using System.Collections;

public abstract class AbstractMonster : MonoBehaviour {
    public int hp { get; set; }
    public int experience { get; set; }
    public int malus { get; set; }

    public virtual string Name { get; set; }

    public virtual void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent(typeof(BulletB)) as BulletB) {
            BeingHit();
        } 
    }

    public void BeingHit()
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
                player.EarnExperience(experience);
            }
            Game.gameManager.GetMonsterGenerator().DestroyObjectPool(gameObject);
        }

    }
}
