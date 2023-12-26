using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using UnityEngine;

public class MeleeWeaponBehaviour : MonoBehaviour
{
    [SerializeField] protected WeaponScriptableObject WeaponStatsData;
    [SerializeField] private float _destroyAfterSeconds;

    [Header("AudioSFX")]
    public AudioClip MeleeSFX;

    protected PlayerStats Player;

    // Feedbacks variable
    private MMF_Player _feedbackFloatingText;

    private MMF_Player _feedbackCameraShake;

    //Current Stats

    protected float CurrentDamage;
    protected float CurrentSpeed;
    protected float CurrentCooldownDuration;
    protected int CurrentPierce;

    private void Awake()
    {
        CurrentDamage = WeaponStatsData.Damage;
        CurrentSpeed = WeaponStatsData.Speed;
        CurrentCooldownDuration = WeaponStatsData.CooldownDuration;
        CurrentPierce = WeaponStatsData.Pierce;
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        Player = FindObjectOfType<PlayerStats>();
        _feedbackFloatingText = GameObject.Find("Feedbacks/FloatingText").GetComponent<MMF_Player>();
        _feedbackCameraShake = GameObject.Find("Feedbacks/CameraShake").GetComponent<MMF_Player>();
        Destroy(gameObject, _destroyAfterSeconds);
    }

    public float GetCurrentDamage()
    {
        return CurrentDamage *= Player.CurrentMight;
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        // Reference the script from the collided collider and deal damage using TakeDamage()
        if (other.CompareTag("Enemy"))
        {
            Player.Animator.SetBool(Player.IsAttackHash, true);
            PlaySFX(MeleeSFX, 35213, 0.2f);
            HandleFeedbacks();
            EnemyStats enemy = other.GetComponent<EnemyStats>();
            enemy.EnemyTakeDamage(GetCurrentDamage(), transform.position); // Using CurrentDamage instead of WeaponData.Damage for applying any damage multiplier
        }
        else if (other.CompareTag("Prop"))
        {
            if (other.gameObject.TryGetComponent(out BreakableProps breakable))
            {
                breakable.PropsTakeDamage(GetCurrentDamage());
            }
        }
    }

    private void PlaySFX(AudioClip sfxClip, int sfxId, float sfxVolume)
    {
        MMSoundManagerPlayOptions options;
        options = MMSoundManagerPlayOptions.Default;
        options.Loop = false;
        options.ID = sfxId;
        options.Volume = sfxVolume;
        options.DoNotAutoRecycleIfNotDonePlaying = false;

        MMSoundManagerSoundPlayEvent.Trigger(sfxClip, options);
    }

    private void HandleFeedbacks()
    {
        _feedbackFloatingText.PlayFeedbacks(this.transform.position, GetCurrentDamage() * 10);
        _feedbackCameraShake.PlayFeedbacks();
    }
}