using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyChase : MonoBehaviour
{
    private Transform _playerTransform;
    private NavMeshAgent _enemyAgent;

    // Start is called before the first frame update
    private void Start()
    {
        _playerTransform = FindObjectOfType<PlayerStateMachine>().transform;
        _enemyAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    private void Update()
    {
        _enemyAgent.destination = _playerTransform.position;
    }
}