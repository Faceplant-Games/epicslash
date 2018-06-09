using UnityEngine;

public abstract class AbstractMonster : MonoBehaviour
{
    protected int Hp { get; set; }
    protected int Experience { get; set; }
    protected int Malus { get; set; }

    public virtual string Name { get; set; }

    public virtual void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent(typeof(BulletB)) as BulletB)
        {
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
        Game.GameManager.GetMonsterGenerator().DestroyObjectPool(gameObject);
    }
}