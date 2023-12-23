using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollector : MonoBehaviour
{
    private PlayerStats _playerStats;
    private SphereCollider _playerCollectorCollider;
    //public float PullSpeed;

    private void Start()
    {
        _playerStats = FindObjectOfType<PlayerStats>();
        _playerCollectorCollider = GetComponent<SphereCollider>();
    }

    private void Update()
    {
        _playerCollectorCollider.radius = _playerStats.CurrentMagnetRadius;
    }

    private void OnTriggerEnter(Collider other)
    {
        // check if the other game object has the ICollectible interface
        if (other.gameObject.TryGetComponent(out ICollectible collectible))
        {
            // Pulling pickup items towards the player

            //Rigidbody otherRb = other.gameObject.GetComponent<Rigidbody>();
            //Vector3 forceDirection = (transform.position - otherRb.transform.position).normalized;
            //otherRb.AddForce(forceDirection * PullSpeed);

            // if it has the interface then execute the collect function
            collectible.Collect();
        }
    }
}