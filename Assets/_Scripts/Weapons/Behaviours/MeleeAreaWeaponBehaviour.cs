using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAreaWeaponBehaviour : MeleeWeaponBehaviour
{
    private bool _canAttack;

    [SerializeField] private float _destroyAfterSeconds;

    [Header("AudioSFX")]
    public AudioClip MeleeSFX;

    protected override void Start()
    {
        base.Start();
        _canAttack = true;

        Destroy(gameObject, _destroyAfterSeconds);
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        // start the attack animation when the enemies are nearby the player
        if (other.CompareTag("Enemy") || other.CompareTag("Prop") && _canAttack)
        {
            StartCoroutine(AttackAnimationRoutine());
        }
    }

    private IEnumerator AttackAnimationRoutine()
    {
        _canAttack = false;
        Player.Animator.SetBool(Player.IsAttackHash, true);
        PlaySFX(MeleeSFX, 35213, 0.2f);
        yield return new WaitForSeconds(0.533f); // the seconds value is the same as duration of the attack animation
        _canAttack = true;
        Player.Animator.SetBool(Player.IsAttackHash, false);
    }

    private void OnDestroy()
    {
        if (!gameObject.scene.isLoaded) // stops the spawning error from appearing when stop play mode
        {
            return;
        }
        // exit attack animation state
        Player.Animator.SetBool(Player.IsAttackHash, false);
    }

    private void PlaySFX(AudioClip sfx, int soundID, float volume)
    {
        MMSoundManagerPlayOptions options;
        options = MMSoundManagerPlayOptions.Default;
        options.Loop = false;
        options.Volume = volume;
        options.ID = soundID;
        options.DoNotAutoRecycleIfNotDonePlaying = false;

        MMSoundManagerSoundPlayEvent.Trigger(sfx, options);
    }
}