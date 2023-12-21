using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    [SerializeField] private EnemyScriptableObject _enemyData;

    [HideInInspector] public float CurrentMoveSpeed;
    [HideInInspector] public float CurrentHealth;
    [HideInInspector] public float CurrentDamage;

    private void Awake()
    {
        CurrentMoveSpeed = _enemyData.MoveSpeed;
        CurrentHealth = _enemyData.MaxHealth;
        CurrentDamage = _enemyData.Damage;
    }

    public void EnemyTakeDamage(float damage)
    {
        CurrentHealth -= damage;
        if (CurrentHealth <= 0)
        {
            EnemyDied();
        }
    }

    public void EnemyDied()
    {
        Destroy(gameObject);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerStats player = other.gameObject.GetComponent<PlayerStats>();
            player.PlayerTakeDamage(CurrentDamage);
        }
    }
}