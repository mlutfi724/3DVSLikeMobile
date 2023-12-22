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

    private void Awake()
    {
        CurrentMoveSpeed = _enemyData.MoveSpeed;
        CurrentHealth = _enemyData.MaxHealth;
        CurrentDamage = _enemyData.Damage;
    }

    private void Start()
    {
        _playerTransform = FindObjectOfType<PlayerStats>().transform;
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, _playerTransform.position) > DespawnDistance)
        {
            ReturnEnemy();
        }
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
        EnemySpawner enemySpawner = FindObjectOfType<EnemySpawner>();
        enemySpawner.OnEnemyKilled();
    }
}