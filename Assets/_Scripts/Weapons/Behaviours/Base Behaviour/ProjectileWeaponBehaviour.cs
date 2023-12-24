using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Base script for all projectile type weapon
public class ProjectileWeaponBehaviour : MonoBehaviour
{
    [SerializeField] protected WeaponScriptableObject WeaponStatsData;

    protected Vector3 Direction;
    [SerializeField] private float _destroyAfterSeconds;

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

    public void DirectionChecker(Vector3 dir)
    {
        Direction = dir;
    }

    public float GetCurrentDamage()
    {
        return CurrentDamage *= FindObjectOfType<PlayerStats>().CurrentMight;
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        // Reference the script from the collided collider and deal damage using TakeDamage()
        if (other.CompareTag("Enemy"))
        {
            EnemyStats enemy = other.GetComponent<EnemyStats>();
            enemy.EnemyTakeDamage(GetCurrentDamage(), transform.position); // Using CurrentDamage instead of WeaponData.Damage for applying any damage multiplier
            ReducePierce();
        }
        else if (other.CompareTag("Prop"))
        {
            if (other.gameObject.TryGetComponent(out BreakableProps breakable))
            {
                breakable.PropsTakeDamage(GetCurrentDamage());
                ReducePierce();
            }
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