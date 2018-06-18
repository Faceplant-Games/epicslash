using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusManager : MonoBehaviour {

    public List<AbstractPickup> Bonuses = new List<AbstractPickup>();
    private List<AbstractPickup> BonusesInstances = new List<AbstractPickup>();

    public List<GameObject> BonusSpots = new List<GameObject>();

    public void Start()
    {
        Game.BonusManager = this;
    }

    public void CreateBonus(Vector3 position)
    {
        if (Random.Range(0, 1) < 0.05f)
        {
            AbstractPickup Bonus = Instantiate<AbstractPickup>(Bonuses[(int) Random.Range(0, Bonuses.Count - 1)]);
            BonusesInstances.Add(Bonus);
            Bonus.transform.localPosition = position + new Vector3(0, 1f, 0);
            Bonus.transform.localScale = Bonus.transform.localScale / 4;
            Bonus.CurrentState = AbstractPickup.State.TRANSITIONING;
        }
    }

    public void Update()
    {
        foreach (var Bonus in BonusesInstances)
        {
            Bonus.transform.Rotate(new Vector3(0, Time.deltaTime * 50f, 0));
        }
    }
}
