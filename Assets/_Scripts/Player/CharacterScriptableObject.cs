using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterScriptableObject", menuName = "ScriptableObjects/Character")]
public class CharacterScriptableObject : ScriptableObject
{
    [SerializeField] private GameObject _startingWeapon;
    [SerializeField] private float _maxHealth;
    [SerializeField] private float _recovery;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _might;
    [SerializeField] private float _projectileSpeed;
    [SerializeField] private float _magnetRadius;

    // Properties
    public GameObject StartingWeapon
    { get { return _startingWeapon; } private set { _startingWeapon = value; } }

    public float MaxHealth
    { get { return _maxHealth; } private set { _maxHealth = value; } }

    public float Recovery
    { get { return _recovery; } private set { _recovery = value; } }

    public float MoveSpeed
    { get { return _moveSpeed; } private set { _moveSpeed = value; } }

    public float Might
    { get { return _might; } private set { _might = value; } }

    public float ProjectileSpeed
    { get { return _projectileSpeed; } private set { _projectileSpeed = value; } }

    public float MagnetRadius
    { get { return _magnetRadius; } private set { _magnetRadius = value; } }
}