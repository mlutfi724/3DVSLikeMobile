using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyChasing : MonoBehaviour
{
    private Transform _playerTransform;

    private EnemyStats _enemyStats;

    private Vector3 _knockbackVelocity;
    private float _knockbackDuration;

    private void Awake()
    {
        _playerTransform = FindObjectOfType<PlayerStateMachine>().transform;
        _enemyStats = GetComponent<EnemyStats>();
    }

    // Update is called once per frame
    private void Update()
    {
        // if we are currently being knocked back, then process the knockback
        if (_knockbackDuration > 0)
        {
            transform.position += _knockbackVelocity * Time.deltaTime;
            _knockbackDuration -= Time.deltaTime;
        }
        else
        {
            ChasePlayer();
        }
    }

    private void ChasePlayer()
    {
        transform.position = Vector3.MoveTowards(transform.position, _playerTransform.position, _enemyStats.CurrentMoveSpeed * Time.deltaTime);
        transform.LookAt(_playerTransform.position);
    }

    //This is meant to be called from other scripts to create knockback
    public void Knockback(Vector3 velocity, float duration)
    {
        // Ignore the knockback if the duration greater than 0
        if (_knockbackDuration > 0) return;

        //begins the knockback
        _knockbackVelocity = velocity;
        _knockbackDuration = duration;
    }
}