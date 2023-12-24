using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    [SerializeField] private EnemyScriptableObject _enemyData;

    [HideInInspector] public float CurrentMoveSpeed;
    [HideInInspector] public float CurrentHealth;
    [HideInInspector] public float CurrentDamage;

    public float DespawnDistance = 20f;
    private Transform _playerTransform;
    private EnemyChasing _enemyChasing;

    private void Awake()
    {
        CurrentMoveSpeed = _enemyData.MoveSpeed;
        CurrentHealth = _enemyData.MaxHealth;
        CurrentDamage = _enemyData.Damage;
    }

    private void Start()
    {
        _playerTransform = FindObjectOfType<PlayerStats>().transform;
        _enemyChasing = GetComponent<EnemyChasing>();
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, _playerTransform.position) > DespawnDistance)
        {
            ReturnEnemy();
        }
    }

    public void EnemyTakeDamage(float damage, Vector3 sourcePosition, float knockbackForce = 5f, float knockbackDuration = 0.2f)
    {
        CurrentHealth -= damage;

        //Apply knockback if it is not zero.
        if (knockbackForce > 0)
        {
            // gets the direction of the knockback
            Vector3 dir = transform.position - sourcePosition;
            _enemyChasing.Knockback(dir.normalized * knockbackForce, knockbackDuration);
        }

        if (CurrentHealth <= 0)
        {
            EnemyDied();
        }
    }

    public void EnemyDied()
    {
        Destroy(gameObject);
    }

    private void ReturnEnemy()
    {
        EnemySpawner enemySpawner = FindObjectOfType<EnemySpawner>();
        transform.position = _playerTransform.position + enemySpawner.RelativeSpawnPoints[Random.Range(0, enemySpawner.RelativeSpawnPoints.Count)].position;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerStats player = other.gameObject.GetComponent<PlayerStats>();
            player.PlayerTakeDamage(CurrentDamage);
        }
    }

    private void OnDestroy()
    {
        if (!gameObject.scene.isLoaded) // stops the spawning error from appearing when stop play mode
        {
            return;
        }
        EnemySpawner enemySpawner = FindObjectOfType<EnemySpawner>();
        enemySpawner.OnEnemyKilled();
    }
}