using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private CharacterScriptableObject _characterData;

    // Current Character Stats

    private float _currentHealth;
    private float _currentRecovery;
    private float _currentMoveSpeed;
    private float _currentMight;
    private float _currentProjectileSpeed;

    // Experience and level of the player
    [Header("Experience/Level")]
    public int Experience = 0;

    public int Level = 1;
    public int ExperienceCap;
    public List<LevelRange> LevelRanges;

    // I-Frames
    [Header("I-Frames")]
    public float InvincibilityDuration;

    private float _invincibilityTimer;
    private bool _isInvincible;

    //Class for defining a level range and the corresponding experience cap increase for that range
    [System.Serializable]
    public class LevelRange
    {
        public int StartLevel;
        public int EndLevel;
        public int ExperienceCapIncrease;
    }

    private void Awake()
    {
        _currentHealth = _characterData.MaxHealth;
        _currentRecovery = _characterData.Recovery;
        _currentMoveSpeed = _characterData.MoveSpeed;
        _currentMight = _characterData.Might;
        _currentProjectileSpeed = _characterData.ProjectileSpeed;
    }

    private void Start()
    {
        // initialize the experience cap as the first experience cap increase
        ExperienceCap = LevelRanges[0].ExperienceCapIncrease;
    }

    private void Update()
    {
        if (_invincibilityTimer > 0)
        {
            _invincibilityTimer -= Time.deltaTime;
        }
        else if (_isInvincible) // if the invincibility timer reaches 0 and currently still isInvincible
        {
            _isInvincible = false;
        }
    }

    public void IncreaseExperience(int amount)
    {
        Experience += amount;
        LevelUpChecker();
    }

    public void PlayerTakeDamage(float dmg)
    {
        // if the player are not in IFrame state
        if (!_isInvincible)
        {
            _currentHealth -= dmg;

            _invincibilityTimer = InvincibilityDuration;
            _isInvincible = true;

            if (_currentHealth <= 0)
            {
                PlayerDied();
            }
        }
    }

    private void PlayerDied()
    {
        Debug.Log("Player is dead!");
    }

    public void RestoreHealth(int amount)
    {
        //Only heal the player if the current health is less than the max health
        if (_currentHealth < _characterData.MaxHealth)
        {
            _currentHealth += amount;
            // makin sure the player's health doesn't exceed the Max health
            if (_currentHealth > _characterData.MaxHealth)
            {
                _currentHealth = _characterData.MaxHealth;
            }
        }
    }

    private void LevelUpChecker()
    {
        if (Experience >= ExperienceCap)
        {
            Level++;
            Experience -= ExperienceCap;

            int ExperienceCapIncrease = 0;
            foreach (LevelRange range in LevelRanges)
            {
                if (Level >= range.StartLevel && Level <= range.EndLevel)
                {
                    ExperienceCapIncrease = range.ExperienceCapIncrease;
                    break;
                }
            }
            ExperienceCap += ExperienceCapIncrease;
        }
    }
}