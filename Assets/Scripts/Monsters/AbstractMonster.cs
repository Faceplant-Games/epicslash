using UnityEngine;

public abstract class AbstractMonster : MonoBehaviour
{
    public int Hp { get; set; }
    public int Experience { get; set; }
    protected int Malus { get; set; }

    public virtual void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent(typeof(BulletB)) as BulletB)
        {
            BeingHit();
        }
    }

    public void BeingHit(int damage = 1)
    {
        Hp -= damage;
        if (Hp > 0) return;

        Game.GameManager.EarnExperience(Experience);
        Game.GameManager.GetMonsterGenerator().DestroyObjectPool(gameObject);
    }
}