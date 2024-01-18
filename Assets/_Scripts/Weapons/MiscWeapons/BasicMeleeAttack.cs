using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMeleeAttack : MeleeWeaponBehaviour
{
    private MeshCollider _meshCollider;

    private void Awake()
    {
        CurrentDamage = WeaponStatsData.Damage;
        CurrentSpeed = WeaponStatsData.Speed;
        CurrentCooldownDuration = WeaponStatsData.CooldownDuration;
        CurrentPierce = WeaponStatsData.Pierce;
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        Player = FindObjectOfType<PlayerStats>();
        _meshCollider = GetComponent<MeshCollider>();
        _meshCollider.enabled = false;
    }

    protected override void OnTriggerEnter(Collider other)
    {
        // Reference the script from the collided collider and deal damage using TakeDamage()
        if (other.CompareTag("Enemy"))
        {
            PlaySFX(MeleeSFX, 35213, 0.2f);
            EnemyStats enemy = other.GetComponent<EnemyStats>();
            enemy.EnemyTakeDamage(GetCurrentDamage(), transform.position); // Using CurrentDamage instead of WeaponData.Damage for applying any damage multiplier
        }
        else if (other.CompareTag("Prop"))
        {
            if (other.gameObject.TryGetComponent(out BreakableProps breakable))
            {
                PlaySFX(MeleeSFX, 35213, 0.2f);
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

    public void EnableCollider()
    {
        _meshCollider.enabled = true;
    }

    public void DisableCollider()
    {
        _meshCollider.enabled = false;
    }
}