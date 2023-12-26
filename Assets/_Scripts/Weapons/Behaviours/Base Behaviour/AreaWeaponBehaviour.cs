using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Base script for all the orbiting type behaviours weapon [place in the orbiting weapon prefab]
public class AreaWeaponBehaviour : MonoBehaviour
{
    [SerializeField] protected WeaponScriptableObject WeaponStatsData;
    [SerializeField] private float _destroyAfterSeconds;

    [Header("AudioSFX")]
    public AudioClip HitSFX;

    // Feedbacks variable
    private MMF_Player _feedbackFloatingText;

    private MMF_Player _feedbackCameraShake;

    protected PlayerStats Player;

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
            PlaySFX(HitSFX, 420674, 0.5F);
            HandleFeedbacks();
            EnemyStats enemy = other.GetComponent<EnemyStats>();
            enemy.EnemyTakeDamage(GetCurrentDamage(), transform.position); // Using CurrentDamage instead of WeaponData.Damage for applying any damage multiplier
            ReducePierce();
        }
        else if (other.CompareTag("Prop"))
        {
            if (other.gameObject.TryGetComponent(out BreakableProps breakable))
            {
                PlaySFX(HitSFX, 420674, 0.5F);
                HandleFeedbacks();
                breakable.PropsTakeDamage(GetCurrentDamage());
                ReducePierce();
            }
        }
    }

    private void ReducePierce() // destroy the weapon once the pierce reaches 0
    {
        CurrentPierce--;
        if (CurrentPierce <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void HandleFeedbacks()
    {
        _feedbackFloatingText.PlayFeedbacks(this.transform.position, GetCurrentDamage() * 10);
        _feedbackCameraShake.PlayFeedbacks();
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
}