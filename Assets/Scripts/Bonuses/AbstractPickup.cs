using UnityEngine;
using System.Collections;

public class AbstractPickup : MonoBehaviour
{
    public enum State
    {
        TRANSITIONING,
        PICKABLE
    }

    public virtual State CurrentState { get; set; }

    public void OnTriggerEnter(Collider other)
    {
        if (CurrentState.Equals(State.TRANSITIONING))
        {
            return;
        }
        // Ramasser le pickup
    }
}
