using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PassiveItemScriptableObject", menuName = "ScriptableObjects/Passive Item")]
public class PassiveItemScriptableObject : ScriptableObject
{
    [SerializeField] private Sprite _icon;
    [SerializeField] private string _passiveItemName;
    [SerializeField] private string _passiveItemDescription;
    [SerializeField] private float _multiplier;
    [SerializeField] private int _level;
    [SerializeField] private GameObject _nextLevelPrefab; // The prefab of the next level i.e. what the object becomes when it levels up
                                                          // Not to be confused with the prefab to be spawned at the next level

    public float Multiplier
    { get { return _multiplier; } set { _multiplier = value; } }

    public int Level
    { get { return _level; } private set { _level = value; } }

    public Sprite Icon //not mean to be modified in game [only in editor]
    { get { return _icon; } private set { _icon = value; } }

    public GameObject NextLevelPrefab
    { get { return _nextLevelPrefab; } private set { _nextLevelPrefab = value; } }

    public string PassiveItemName
    { get { return _passiveItemName; } set { _passiveItemName = value; } }

    public string PassiveItemDescription
    { get { return _passiveItemDescription; } set { _passiveItemDescription = value; } }
}