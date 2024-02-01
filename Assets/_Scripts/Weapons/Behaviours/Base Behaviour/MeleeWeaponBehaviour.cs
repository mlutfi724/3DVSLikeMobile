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
            StartCoroutine(AttackAnimationRoutine());
        }
        else if (other.CompareTag("Prop"))
        {
            if (other.gameObject.TryGetComponent(out BreakableProps breakable))
            {
                StartCoroutine(AttackAnimationRoutine());
            }
        }
    }

    private IEnumerator AttackAnimationRoutine()
    {
        Player.Animator.SetBool(Player.IsAttackHash, true);
        yield return new WaitForSeconds(0.533f);
        Player.Animator.SetBool(Player.IsAttackHash, false);
    }
}