using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponScriptableObject", menuName = "ScriptableObjects/Weapon")]
public class WeaponScriptableObject : ScriptableObject
{
    [SerializeField] private Sprite _icon;
    [SerializeField] private string _weaponName;
    [SerializeField] private string _weaponDescription;
    [SerializeField] private GameObject _weaponPrefab;
    [SerializeField] private float _damage;
    [SerializeField] private float _speed;
    [SerializeField] private float _cooldownDuration;
    [SerializeField] private int _pierce; // the amount of time the weapon can hit enemy before get destroyed
    [SerializeField] private int _level;
    [SerializeField] private GameObject _nextLevelPrefab; // The prefab of the next level i.e. what the object becomes when it levels up
                                                          // Not to be confused with the prefab to be spawned at the next level

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

    public int Level
    { get { return _level; } private set { _level = value; } }

    public Sprite Icon //not mean to be modified in game [only in editor]
    { get { return _icon; } private set { _icon = value; } }

    public GameObject NextLevelPrefab
    { get { return _nextLevelPrefab; } private set { _nextLevelPrefab = value; } }

    public string WeaponName
    { get { return _weaponName; } private set { _weaponName = value; } }

    public string WeaponDescription
    { get { return _weaponDescription; } private set { _weaponDescription = value; } }
}