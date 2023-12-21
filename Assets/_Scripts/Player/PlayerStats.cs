using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private CharacterScriptableObject _characterData;
    [SerializeField] private float _weaponSpawnYPos;

    // Current Character Stats

    [HideInInspector] public float CurrentHealth;
    [HideInInspector] public float CurrentRecovery;
    [HideInInspector] public float CurrentMoveSpeed;
    [HideInInspector] public float CurrentMight;
    [HideInInspector] public float CurrentProjectileSpeed;
    [HideInInspector] public float CurrentMagnetRadius;

    // Spawned Weapon
    public List<GameObject> SpawnedWeapons;

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
        CurrentHealth = _characterData.MaxHealth;
        CurrentRecovery = _characterData.Recovery;
        CurrentMoveSpeed = _characterData.MoveSpeed;
        CurrentMight = _characterData.Might;
        CurrentProjectileSpeed = _characterData.ProjectileSpeed;
        CurrentMagnetRadius = _characterData.MagnetRadius;

        SpawnWeaponController(_characterData.StartingWeapon);
    }

    private void Start()
    {
        // initialize the experience cap as the first experience cap increase
        ExperienceCap = LevelRanges[0].ExperienceCapIncrease;
    }

    private void Update()
    {
        HandleIFrame();
        Recover();
    }

    private void PlayerDied()
    {
        Debug.Log("Player is dead!");
    }

    private void HandleIFrame()
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

    public void SpawnWeaponController(GameObject weaponController)
    {
        // spawn the starting weapon
        Vector3 weaponControllerPos = new Vector3(transform.position.x, transform.position.y + _weaponSpawnYPos, transform.position.z);
        GameObject spawnedWeaponController = Instantiate(weaponController, weaponControllerPos, Quaternion.identity);
        spawnedWeaponController.transform.SetParent(transform); // Set the weapon controller to be child of the player
        SpawnedWeapons.Add(spawnedWeaponController); // add to the list of the spawned weapons
    }

    private void Recover()
    {
        if (CurrentHealth < _characterData.MaxHealth)
        {
            CurrentHealth += CurrentRecovery * Time.deltaTime;

            if (CurrentHealth > _characterData.MaxHealth) // making sure the player health doesn't exceed max health
            {
                CurrentHealth = _characterData.MaxHealth;
            }
        }
    }

    public void RestoreHealth(float amount)
    {
        //Only heal the player if the current health is less than the max health
        if (CurrentHealth < _characterData.MaxHealth)
        {
            CurrentHealth += amount;
            // makin sure the player's health doesn't exceed the Max health
            if (CurrentHealth > _characterData.MaxHealth)
            {
                CurrentHealth = _characterData.MaxHealth;
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
            CurrentHealth -= dmg;

            _invincibilityTimer = InvincibilityDuration;
            _isInvincible = true;

            if (CurrentHealth <= 0)
            {
                PlayerDied();
            }
        }
    }
}