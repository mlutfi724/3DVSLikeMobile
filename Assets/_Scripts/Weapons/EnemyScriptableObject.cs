using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyScriptableObject", menuName = "ScriptableObjects/Enemy")]
public class EnemyScriptableObject : ScriptableObject
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _maxHealth;
    [SerializeField] private float _damage;

    public float MoveSpeed
    { get { return _moveSpeed; } set { _moveSpeed = value; } }

    public float MaxHealth
    { get { return _maxHealth; } set { _maxHealth = value; } }

    public float Damage
    { get { return _damage; } set { _damage = value; } }
}