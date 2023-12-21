using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponScriptableObject", menuName = "ScriptableObjects/Weapon")]
public class WeaponScriptableObject : ScriptableObject
{
    [SerializeField] private GameObject _weaponPrefab;
    [SerializeField] private float _damage;
    [SerializeField] private float _speed;
    [SerializeField] private float _cooldownDuration;
    [SerializeField] private int _pierce; // the amount of time the weapon can hit enemy before get destroyed

    // properties
    public GameObject WeaponPrefab
    { get { return _weaponPrefab; } private set { _weaponPrefab = value; } }

    public float Damage
    { get { return _damage; } private set { _damage = value; } }

    public float Speed
    { get { return _speed; } private set { _speed = value; } }

    public float CooldownDuration
    { get { return _cooldownDuration; } private set { _cooldownDuration = value; } }

    public int Pierce
    { get { return _pierce; } private set { _pierce = value; } }
}