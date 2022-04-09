using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class EnergyPickup : MonoBehaviour
{
    [SerializeField] int value;

    private void OnTriggerEnter(Collider other) {

        PlayerWanderEnergy reference = null;
        other.gameObject.TryGetComponent<PlayerWanderEnergy>(out reference);
        if (reference != null) {
            reference.HealDirect(value);
        }

        Destroy(gameObject);
    }

}
