using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [Header("Weapon Stats")]
    [SerializeField] protected WeaponScriptableObject WeaponStatsData;

    protected PlayerStateMachine PlayerMovement;

    private float _currentCooldown;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        PlayerMovement = FindObjectOfType<PlayerStateMachine>();
        _currentCooldown = WeaponStatsData.CooldownDuration; // set the current coooldown to be the cooldown duration
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
        _currentCooldown = WeaponStatsData.CooldownDuration;
    }
}