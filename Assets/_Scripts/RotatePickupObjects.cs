using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatePickupObjects : MonoBehaviour
{
    [SerializeField] private float rotateSpeed;
    [SerializeField] private float _pullSpeed = 100f;
    private Rigidbody _objectRb;
    private PlayerStats _player;
    private bool _isPulled;

    private void Start()
    {
        _objectRb = GetComponent<Rigidbody>();
        _player = FindObjectOfType<PlayerStats>();
        _isPulled = false;
        transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
    }

    private void Update()
    {
        transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime, 0);

        if (_isPulled)
        {
            PulledToPlayer();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Collector"))
        {
            _isPulled = true;
        }
    }

    private void PulledToPlayer()
    {
        Vector3 forceDirection = (_player.transform.position - transform.position).normalized;
        _objectRb.AddForce(forceDirection * _pullSpeed);
        Destroy(gameObject, 3f);
    }

    //Vector3 forceDirection = (transform.position - otherRb.transform.position).normalized;
    //otherRb.AddForce(forceDirection * PullSpeed);
}