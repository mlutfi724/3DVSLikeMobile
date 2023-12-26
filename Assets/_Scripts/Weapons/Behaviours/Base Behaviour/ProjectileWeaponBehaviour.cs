using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Base script for all projectile type weapon
public class ProjectileWeaponBehaviour : MonoBehaviour
{
    [SerializeField] protected WeaponScriptableObject WeaponStatsData;

    protected Vector3 Direction;
    [SerializeField] private float _destroyAfterSeconds;

    // Feedbacks variable
    private MMF_Player _feedbackFloatingText;

    private MMF_Player _feedbackCameraShake;

    [Header("AudioSFX")]
    public AudioClip HitSFX;

    //Current stats

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
        _feedbackFloatingText = GameObject.Find("Feedbacks/FloatingText").GetComponent<MMF_Player>();
        _feedbackCameraShake = GameObject.Find("Feedbacks/CameraShake").GetComponent<MMF_Player>();
        Destroy(gameObject, _destroyAfterSeconds);
    }

    public void DirectionChecker(Vector3 dir)
    {
        Direction = dir;
    }

    public float GetCurrentDamage()
    {
        return CurrentDamage *= FindObjectOfType<PlayerStats>().CurrentMight;
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        // Reference the script from the collided collider and deal damage using TakeDamage()
        if (other.CompareTag("Enemy"))
        {
            PlaySFX(HitSFX, 496191);
            HandleFeedbacks();
            EnemyStats enemy = other.GetComponent<EnemyStats>();
            enemy.EnemyTakeDamage(GetCurrentDamage(), transform.position); // Using CurrentDamage instead of WeaponData.Damage for applying any damage multiplier
            ReducePierce();
        }
        else if (other.CompareTag("Prop"))
        {
            PlaySFX(HitSFX, 496191);
            HandleFeedbacks();
            if (other.gameObject.TryGetComponent(out BreakableProps breakable))
            {
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

    private void PlaySFX(AudioClip sfx, int soundID)
    {
        MMSoundManagerPlayOptions options;
        options = MMSoundManagerPlayOptions.Default;
        options.Loop = false;
        options.Volume = 10f;
        options.ID = soundID;

        MMSoundManagerSoundPlayEvent.Trigger(sfx, options);
    }
}