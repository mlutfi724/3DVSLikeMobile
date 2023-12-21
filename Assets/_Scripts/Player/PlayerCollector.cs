using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollector : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // check if the other game object has the ICollectible interface
        if (other.gameObject.TryGetComponent(out ICollectible collectible))
        {
            // if it has the interface than execute the collect function
            collectible.Collect();
        }
    }
}