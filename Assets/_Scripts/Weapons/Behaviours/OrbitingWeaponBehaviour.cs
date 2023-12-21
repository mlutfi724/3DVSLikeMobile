using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Base script for all the orbiting type behaviours weapon [place in the orbiting weapon prefab]
public class OrbitingWeaponBehaviour : MonoBehaviour
{
    [SerializeField] protected WeaponScriptableObject WeaponStatsData;
    [SerializeField] private float _destroyAfterSeconds;

    private PlayerStateMachine _playerMovement;

    //Current Stats

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
        _playerMovement = FindObjectOfType<PlayerStateMachine>();
        Destroy(gameObject, _destroyAfterSeconds);
    }

    protected virtual void Update()
    {
        OrbitingPlayer();
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        // Reference the script from the collided collider and deal damage using TakeDamage()
        if (other.CompareTag("Enemy"))
        {
            EnemyController enemy = other.GetComponent<EnemyController>();
            enemy.EnemyTakeDamage(CurrentDamage); // Using CurrentDamage instead of WeaponData.Damage for applying any damage multiplier
            ReducePierce();
        }
        else if (other.CompareTag("Prop"))
        {
            if (other.gameObject.TryGetComponent(out BreakableProps breakable))
            {
                breakable.PropsTakeDamage(CurrentDamage);
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

    private void OrbitingPlayer()
    {
        transform.RotateAround(_playerMovement.transform.position, Vector3.up, WeaponStatsData.Speed * Time.deltaTime);
    }
}