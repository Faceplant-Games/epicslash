using UnityEngine;
using System.Collections;

public abstract class AbstractMonster : MonoBehaviour {
    public int Hp { get; set; }
    public int Experience { get; set; }
    public int Malus { get; set; }

    public virtual string Name { get; set; }

    public virtual void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent(typeof(BulletB)) as BulletB) {
            BeingHit();
        } 
    }

    public void BeingHit()
    {
        if (Hp > 1)
        {
            Hp--;
            return;
        }
        Game.GameManager.EarnExperience(Experience);
        Game.BonusManager.CreateBonus(gameObject.transform.position);

        Game.GameManager.GetMonsterGenerator().DestroyObjectPool(gameObject);
    }
}
