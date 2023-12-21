using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyChasing : MonoBehaviour
{
    private Transform _playerTransform;
    private NavMeshAgent _enemyAgent;
    private EnemyStats _enemyStats;

    private void Awake()
    {
        _playerTransform = FindObjectOfType<PlayerStateMachine>().transform;
        _enemyAgent = GetComponent<NavMeshAgent>();
        _enemyStats = GetComponent<EnemyStats>();
    }

    // Update is called once per frame
    private void Update()
    {
        ChasePlayer();
    }

    private void ChasePlayer()
    {
        _enemyAgent.speed = _enemyStats.CurrentMoveSpeed;
        _enemyAgent.destination = _playerTransform.position;
    }
}