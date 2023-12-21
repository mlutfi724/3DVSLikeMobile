using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private EnemyScriptableObject _enemyData;

    private Transform _playerTransform;
    private NavMeshAgent _enemyAgent;

    private float _currentMoveSpeed;
    private float _currentHealth;
    private float _currentDamage;

    private void Awake()
    {
        _currentMoveSpeed = _enemyData.MoveSpeed;
        _currentHealth = _enemyData.MaxHealth;
        _currentDamage = _enemyData.Damage;
    }

    // Start is called before the first frame update
    private void Start()
    {
        _playerTransform = FindObjectOfType<PlayerStateMachine>().transform;
        _enemyAgent = GetComponent<NavMeshAgent>();
        _enemyAgent.speed = _currentMoveSpeed;
    }

    // Update is called once per frame
    private void Update()
    {
        ChasePlayer();
    }

    private void ChasePlayer()
    {
        _enemyAgent.destination = _playerTransform.position;
    }

    public void TakeDamage(float damage)
    {
        _currentHealth -= damage;
        if (_currentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}