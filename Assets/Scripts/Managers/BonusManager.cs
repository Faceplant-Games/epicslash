using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusManager : MonoBehaviour {

    public List<AbstractPickup> Bonuses = new List<AbstractPickup>();
    private List<AbstractPickup> BonusesInstances = new List<AbstractPickup>();

    public Stack<GameObject> BonusSpots = new Stack<GameObject>();

    public void Start()
    {
        Game.BonusManager = this;
    }

    public void CreateBonus(Vector3 position)
    {
        if (BonusSpots.Peek())
        {
            if (Random.Range(0, 1) < 0.05f)
            {
                AbstractPickup Bonus = Instantiate<AbstractPickup>(Bonuses[(int) Random.Range(0, Bonuses.Count - 1)]);
                BonusesInstances.Add(Bonus);
                Bonus.transform.position += new Vector3(0, 1f, 0);
                Bonus.CurrentState = AbstractPickup.State.TRANSITIONING;
                Bonus.BonusSpot = BonusSpots.Pop();
            }
        }
    }
}
