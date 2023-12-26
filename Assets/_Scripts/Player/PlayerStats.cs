using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using MoreMountains.Tools;
using MoreMountains.Feedbacks;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private CharacterScriptableObject _characterData;
    [SerializeField] public float WeaponSpawnYPos = 0.5f;
    public GameObject SecondWeaponTest;
    public GameObject FirstPassiveItemTest, SecondPassiveItemTest;
    private InventoryManager _inventory;
    public int WeaponIndex;
    public int PassiveItemIndex;

    [Header("Audio SFX")]
    public AudioClip HitSFX;

    private AudioSource _hitSFXAudioSource;

    [Header("Feedbacks")]
    public MMF_Player HitFeedback;

    private MMF_Player _feedbackFloatingText;

    [HideInInspector] public Animator Animator;

    // Hash for animator

    private int _isAttackHash;

    public int IsAttackHash
    { get { return _isAttackHash; } }

    private int _isHitHash;

    public int IsHitHash
    { get { return _isHitHash; } }

    private int _isDiedHash;

    public int IsDiedHash
    { get { return _isDiedHash; } }

    // Current Character Stats

    private float _currentHealth;
    private float _currentRecovery;
    private float _currentMoveSpeed;
    private float _currentMight;
    private float _currentProjectileSpeed;
    private float _currentMagnetRadius;

    #region Current Stats Properties

    public float CurrentHealth
    {
        get { return _currentHealth; }
        set
        {
            // check if the value has changed
            if (_currentHealth != value)
            {
                // Update the real time value of the stat
                _currentHealth = value;
                if (GameManager.instance != null)
                {
                    GameManager.instance.CurrentHealthDisplay.text = "Health: " + _currentHealth;
                }
                // Add any additional logic here that needs to be executed when the value changes
            }
        }
    }

    public float CurrentRecovery
    {
        get { return _currentRecovery; }
        set
        {
            // check if the value has changed
            if (_currentRecovery != value)
            {
                // Update the real time value of the stat
                _currentRecovery = value;
                if (GameManager.instance != null)
                {
                    GameManager.instance.CurrentRecoveryDisplay.text = "Recovery: " + _currentRecovery;
                }
                // Add any additional logic here that needs to be executed when the value changes
            }
        }
    }

    public float CurrentMoveSpeed
    {
        get { return _currentMoveSpeed; }
        set
        {
            // check if the value has changed
            if (_currentMoveSpeed != value)
            {
                // Update the real time value of the stat
                _currentMoveSpeed = value;
                if (GameManager.instance != null)
                {
                    GameManager.instance.CurrentMoveSpeedDisplay.text = "Move Speed: " + _currentMoveSpeed;
                }

                // Add any additional logic here that needs to be executed when the value changes
            }
        }
    }

    public float CurrentMight
    {
        get { return _currentMight; }
        set
        {
            // check if the value has changed
            if (_currentMight != value)
            {
                // Update the real time value of the stat
                _currentMight = value;
                if (GameManager.instance != null)
                {
                    GameManager.instance.CurrentMightDisplay.text = "Might: " + _currentMight;
                }
                // Add any additional logic here that needs to be executed when the value changes
            }
        }
    }

    public float CurrentProjectileSpeed
    {
        get { return _currentProjectileSpeed; }
        set
        {
            // check if the value has changed
            if (_currentProjectileSpeed != value)
            {
                // Update the real time value of the stat
                _currentProjectileSpeed = value;
                if (GameManager.instance != null)
                {
                    GameManager.instance.CurrentProjectileSpeedDisplay.text = "Projectile Speed: " + _currentProjectileSpeed;
                }
                // Add any additional logic here that needs to be executed when the value changes
            }
        }
    }

    public float CurrentMagnetRadius
    {
        get { return _currentMagnetRadius; }
        set
        {
            // check if the value has changed
            if (_currentMagnetRadius != value)
            {
                // Update the real time value of the stat
                _currentMagnetRadius = value;
                if (GameManager.instance != null)
                {
                    GameManager.instance.CurrentMagnetDisplay.text = "Magnet: " + _currentMagnetRadius;
                }
                // Add any additional logic here that needs to be executed when the value changes
            }
        }
    }

    #endregion Current Stats Properties

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

    [Header("UI")]
    public Image HealthBar;

    public Image ExpBar;
    public TextMeshProUGUI LevelText;

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

        Animator = GetComponent<Animator>();

        // set the parameter hash
        _isAttackHash = Animator.StringToHash("isAttack");
        _isHitHash = Animator.StringToHash("isHit");
        _isDiedHash = Animator.StringToHash("isDied");

        //SpawnPassiveItem(FirstPassiveItemTest);
        //SpawnPassiveItem(SecondPassiveItemTest);
        SpawnWeaponController(_characterData.StartingWeapon);
        //SpawnWeaponController(SecondWeaponTest);
    }

    private void Start()
    {
        // Get feedback reference
        _feedbackFloatingText = GameObject.Find("Feedbacks/FloatingText").GetComponent<MMF_Player>();

        // initialize the experience cap as the first experience cap increase
        ExperienceCap = LevelRanges[0].ExperienceCapIncrease;

        GameManager.instance.CurrentHealthDisplay.text = "Health: " + _currentHealth;
        GameManager.instance.CurrentRecoveryDisplay.text = "Recovery: " + _currentRecovery;
        GameManager.instance.CurrentMoveSpeedDisplay.text = "Move Speed: " + _currentMoveSpeed;
        GameManager.instance.CurrentMightDisplay.text = "Might: " + _currentMight;
        GameManager.instance.CurrentProjectileSpeedDisplay.text = "Projectile Speed: " + _currentProjectileSpeed;
        GameManager.instance.CurrentMagnetDisplay.text = "Magnet: " + _currentMagnetRadius;

        GameManager.instance.AssignChosenCharacterUI(_characterData);

        // Assigning values to UI
        UpdateHealthBarUI();
        UpdateExpBarUI();
        UpdateLevelTextUI();
    }

    private void Update()
    {
        HandleIFrame();
        Recover();
    }

    public void PlayerDied()
    {
        if (!GameManager.instance.IsGameOver)
        {
            GameManager.instance.AssignLevelReachedUI(Level);
            GameManager.instance.AssignChosenWeaponAndPassiveItemsUI(_inventory.WeaponUISlots, _inventory.PassiveItemUISlots);
            StartCoroutine(PlayerDiedGameOverRoutine());
        }
    }

    private IEnumerator PlayerDiedGameOverRoutine()
    {
        // Enter the player died animation state
        Animator.SetBool(_isDiedHash, true);
        // disable the player movement
        PlayerStateMachine playerMovement = GetComponent<PlayerStateMachine>();
        playerMovement.enabled = false;
        // return the walk sfx to the pool
        MMSoundManagerSoundControlEvent.Trigger(MMSoundManagerSoundControlEventTypes.Free, 270421, playerMovement.WalkSFXAudioSource);

        yield return new WaitForSeconds(1f);
        GameManager.instance.GameOver();
    }

    private void HandleIFrame()
    {
        if (_invincibilityTimer > 0)
        {
            _invincibilityTimer -= Time.deltaTime;
        }
        else if (_isInvincible) // if the invincibility timer reaches 0 and currently still isInvincible
        {
            // exit the hit animation state
            Animator.SetBool(_isHitHash, false);
            // return the audio source to the pool
            MMSoundManagerSoundControlEvent.Trigger(MMSoundManagerSoundControlEventTypes.Free, 413182, _hitSFXAudioSource);
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
            UpdateHealthBarUI();
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
            UpdateHealthBarUI();
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

            UpdateLevelTextUI();

            GameManager.instance.StartLevelUp();
        }
    }

    private void UpdateExpBarUI()
    {
        ExpBar.fillAmount = (float)Experience / ExperienceCap;
    }

    private void UpdateLevelTextUI()
    {
        LevelText.text = "LV " + Level.ToString();
    }

    public void IncreaseExperience(int amount)
    {
        Experience += amount;
        LevelUpChecker();
        UpdateExpBarUI();
    }

    public void PlayerTakeDamage(float dmg)
    {
        // if the player are not in IFrame state
        if (!_isInvincible)
        {
            CurrentHealth -= dmg;

            _invincibilityTimer = InvincibilityDuration;
            _isInvincible = true;

            // Handle hit animation
            Animator.SetBool(_isHitHash, true);
            HitFeedback?.PlayFeedbacks();
            _feedbackFloatingText.PlayFeedbacks(this.transform.position, dmg * -1);
            PlaySFX(HitSFX, 413182, _hitSFXAudioSource);

            if (CurrentHealth <= 0)
            {
                PlayerDied();
            }

            UpdateHealthBarUI();
        }
    }

    private void UpdateHealthBarUI()
    {
        HealthBar.fillAmount = CurrentHealth / _characterData.MaxHealth;
    }

    private void PlaySFX(AudioClip sfx, int soundID, AudioSource sfxAudioSource)
    {
        MMSoundManagerPlayOptions options;
        options = MMSoundManagerPlayOptions.Default;
        options.Loop = false;
        options.Volume = 0.7f;
        options.ID = soundID;

        sfxAudioSource = MMSoundManagerSoundPlayEvent.Trigger(sfx, options);
    }
}