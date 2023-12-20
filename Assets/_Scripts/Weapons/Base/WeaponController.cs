using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [Header("Weapon Stats")]
    [SerializeField] protected GameObject WeaponPrefab;

    [SerializeField] private float _damage;
    public float Speed;
    [SerializeField] private float _cooldownDuration;
    private float _currentCooldown;
    [SerializeField] private int _pierce;
    protected PlayerStateMachine PlayerMovement;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        PlayerMovement = FindObjectOfType<PlayerStateMachine>();
        _currentCooldown = _cooldownDuration; // set the current coooldown to be the cooldown duration
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        _currentCooldown -= Time.deltaTime;
        if (_currentCooldown <= 0) // once the cooldown become 0, attack
        {
            Attack();
        }
    }

    protected virtual void Attack()
    {
        _currentCooldown = _cooldownDuration;
    }
}