using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private CharacterScriptableObject _characterData;
    [SerializeField] public float WeaponSpawnYPos = 0.5f;

    // Current Character Stats

    [HideInInspector] public float CurrentHealth;
    [HideInInspector] public float CurrentRecovery;
    [HideInInspector] public float CurrentMoveSpeed;
    [HideInInspector] public float CurrentMight;
    [HideInInspector] public float CurrentProjectileSpeed;
    [HideInInspector] public float CurrentMagnetRadius;

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

    private InventoryManager _inventory;
    public int WeaponIndex;
    public int PassiveItemIndex;

    public GameObject SecondWeaponTest;
    public GameObject FirstPassiveItemTest, SecondPassiveItemTest;

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

        _inventory = GetComponent<InventoryManager>();

        SpawnWeaponController(_characterData.StartingWeapon);
        SpawnWeaponController(SecondWeaponTest);
        SpawnPassiveItem(FirstPassiveItemTest);
        SpawnPassiveItem(SecondPassiveItemTest);
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
        // checking if the slots are full, and returning if it is
        if (WeaponIndex >= _inventory.WeaponSlots.Count - 1) // must be -1 because a list starts from 0
        {
            Debug.LogError("Inventory slots already full!");
            return;
        }

        // spawn the starting weapon
        Vector3 weaponControllerPos = new Vector3(transform.position.x, transform.position.y + WeaponSpawnYPos, transform.position.z);
        GameObject spawnedWeaponController = Instantiate(weaponController, weaponControllerPos, Quaternion.identity);
        spawnedWeaponController.transform.SetParent(transform); // Set the weapon controller to be child of the player
        _inventory.AddWeapon(WeaponIndex, spawnedWeaponController.GetComponent<WeaponController>()); // Add the weapon to it's inventory slot

        WeaponIndex++;
    }

    public void SpawnPassiveItem(GameObject passiveItem)
    {
        // checking if the slots are full, and returning if it is
        if (PassiveItemIndex >= _inventory.PassiveItemSlots.Count - 1) // must be -1 because a list starts from 0
        {
            Debug.LogError("Inventory slots already full!");
            return;
        }

        // spawn the starting weapon

        GameObject spawnedPassiveItem = Instantiate(passiveItem, transform.position, Quaternion.identity);
        spawnedPassiveItem.transform.SetParent(transform); // Set the weapon controller to be child of the player
        _inventory.AddPassiveItem(PassiveItemIndex, spawnedPassiveItem.GetComponent<PassiveItem>()); // Add the weapon to it's inventory slot

        PassiveItemIndex++;
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