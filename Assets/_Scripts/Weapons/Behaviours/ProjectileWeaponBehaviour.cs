using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Base script for all projectile type weapon
public class ProjectileWeaponBehaviour : MonoBehaviour
{
    [SerializeField] protected WeaponScriptableObject WeaponStatsData;

    protected Vector3 Direction;
    [SerializeField] private float _destroyAfterSeconds;
    [SerializeField] private float _rotatingSpeed;

    //Current stats

    protected float CurrentDamage;
    protected float CurrentSpeed;
    protected float CurrentCooldownDuration;
    protected int CurrentPierce;

    private void Awake()
    {
        CurrentDamage = WeaponStatsData.Damage;
        CurrentSpeed = WeaponStatsData.Speed;
        CurrentCooldownDuration = WeaponStatsData.CooldownDuration;
        CurrentPierce = WeaponStatsData.Pierce;
    }

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

    protected virtual void OnTriggerEnter(Collider other)
    {
        // Reference the script from the collided collider and deal damage using TakeDamage()
        if (other.CompareTag("Enemy"))
        {
            EnemyController enemy = other.GetComponent<EnemyController>();
            enemy.TakeDamage(CurrentDamage); // Using CurrentDamage instead of WeaponData.Damage for applying any damage multiplier
            ReducePierce();
        }
    }

    private void ReducePierce() // destroy the weapon once the pierce reaches 0
    {
        CurrentPierce--;
        if (CurrentPierce <= 0)
        {
            Destroy(gameObject);
        }
    }
}