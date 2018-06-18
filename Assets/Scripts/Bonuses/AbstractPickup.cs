using UnityEngine;
using System.Collections;

public class AbstractPickup : MonoBehaviour
{
    public Vector3 StartPos;
    public GameObject BonusSpot;
    public Vector3 Direction;
    //public Vector3 rotationSpeed = new Vector3(0, Time.deltaTime* 50f, 0);

    public enum State
    {
        TRANSITIONING,
        PICKABLE
    }

    public virtual State CurrentState { get; set; }

    void Start()
    {
        StartPos = new Vector3() + transform.position;
        if (BonusSpot != null)
        {
            transform.localRotation = new Quaternion(0, Time.deltaTime * 50f, 0, 0);
            Direction = Vector3.Normalize(BonusSpot.transform.position - transform.position);
        }
    }

    void FixedUpdate()
    {
        var diffFromStart = (transform.position - StartPos).magnitude;
        var diffToEnd = (BonusSpot.transform.position - StartPos).magnitude;
        if (State.TRANSITIONING.Equals(CurrentState))
        {
            transform.position += (Direction * 0.1f) + new Vector3(0, (diffToEnd/2 - diffFromStart) * 0.0005f, 0);
            if ((transform.position - BonusSpot.transform.position).magnitude < 0.2f)
            {
                CurrentState = State.PICKABLE;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Détecter la collision avec une épée (et pas autre chose)
        if (State.TRANSITIONING.Equals(CurrentState))
        {
            return;
        }
        if (State.PICKABLE.Equals(CurrentState))
        {
            // Ramasser le pickup
            Game.BonusManager.BonusSpots.Push(BonusSpot);
            Destroy(this);
        }
    }
}
