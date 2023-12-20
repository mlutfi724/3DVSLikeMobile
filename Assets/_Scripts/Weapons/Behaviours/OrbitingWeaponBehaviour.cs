using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Base script for all the orbiting type behaviours weapon [place in the orbiting weapon prefab]
public class OrbitingWeaponBehaviour : MonoBehaviour
{
    [SerializeField] private float _destroyAfterSeconds;
    [SerializeField] private float _orbitingSpeed;
    private PlayerStateMachine _playerMovement;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        _playerMovement = FindObjectOfType<PlayerStateMachine>();
        Destroy(gameObject, _destroyAfterSeconds);
    }

    protected virtual void Update()
    {
        transform.RotateAround(_playerMovement.transform.position, Vector3.up, _orbitingSpeed * Time.deltaTime);
    }
}