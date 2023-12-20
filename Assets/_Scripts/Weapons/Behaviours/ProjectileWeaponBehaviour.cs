using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Base script for all projectile type weapon
public class ProjectileWeaponBehaviour : MonoBehaviour
{
    protected Vector3 Direction;
    [SerializeField] private float _destroyAfterSeconds;
    [SerializeField] private float _rotatingSpeed;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        Destroy(gameObject, _destroyAfterSeconds);
    }

    protected virtual void Update()
    {
        transform.Rotate(Vector3.right * _rotatingSpeed * Time.deltaTime); // rotating the object
    }

    public void DirectionChecker(Vector3 dir)
    {
        Direction = dir;
    }
}