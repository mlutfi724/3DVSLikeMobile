using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;
using MoreMountains.Feedbacks;

public class EnemyStats : MonoBehaviour
{
    [SerializeField] private EnemyScriptableObject _enemyData;

    // enemy renderer
    [SerializeField] private GameObject _enemyRenderer;

    private Color _enemyOriginalColor;

    [HideInInspector] public float CurrentMoveSpeed;
    [HideInInspector] public float CurrentHealth;
    [HideInInspector] public float CurrentDamage;

    // Feedbacks variable
    private MMF_Player _feedbackFloatingText;

    private MMF_Player _feedbackCameraShake;

    public float DespawnDistance = 20f;

    [Header("Audio SFX")]
    public AudioClip HitSFX;

    public AudioClip DieSFX;

    private Transform _playerTransform;
    private EnemyChasing _enemyChasing;
    private Animator _enemyAnimator;

    // cache hash values
    private static readonly int ChaseState = Animator.StringToHash("Base Layer.Chase");

    private static readonly int HitState = Animator.StringToHash("Base Layer.Hit");
    private static readonly int AttackState = Animator.StringToHash("Base Layer.Attack");

    private void Awake()
    {
        CurrentMoveSpeed = _enemyData.MoveSpeed;
        CurrentHealth = _enemyData.MaxHealth;
        CurrentDamage = _enemyData.Damage;
    }

    private void Start()
    {
        _enemyOriginalColor = _enemyRenderer.GetComponent<SkinnedMeshRenderer>().material.GetColor("_MainColor");

        // Handle other reference
        _feedbackFloatingText = GameObject.Find("Feedbacks/FloatingText").GetComponent<MMF_Player>();
        _feedbackCameraShake = GameObject.Find("Feedbacks/CameraShake").GetComponent<MMF_Player>();
        _playerTransform = FindObjectOfType<PlayerStats>().transform;
        _enemyChasing = GetComponent<EnemyChasing>();
        _enemyAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, _playerTransform.position) > DespawnDistance)
        {
            ReturnEnemy();
        }
    }

    public void EnemyTakeDamage(float damage, Vector3 sourcePosition, float knockbackForce = 5f, float knockbackDuration = 0.2f)
    {
        PlaySFX(HitSFX, 420674, 0.6f);
        StartCoroutine(EnemyDamageFlashRoutine());
        CurrentHealth -= damage;
        _feedbackFloatingText.PlayFeedbacks(transform.position, damage);
        _feedbackCameraShake.PlayFeedbacks();

        _enemyAnimator.CrossFade(HitState, 0.1f, 0, 0);
        //Apply knockback if it is not zero.
        if (knockbackForce > 0)
        {
            // gets the direction of the knockback
            Vector3 dir = transform.position - sourcePosition;
            _enemyChasing.Knockback(dir.normalized * knockbackForce, knockbackDuration);
        }

        if (CurrentHealth <= 0)
        {
            PlaySFX(DieSFX, 471204, 1f);
            EnemyDied();
        }
    }

    private IEnumerator EnemyDamageFlashRoutine()
    {
        _enemyRenderer.GetComponent<SkinnedMeshRenderer>().material.SetColor("_MainColor", Color.red);
        yield return new WaitForSeconds(.5f);
        _enemyRenderer.GetComponent<SkinnedMeshRenderer>().material.SetColor("_MainColor", _enemyOriginalColor);
    }

    public void EnemyDied()
    {
        // return the audio source to the pool
        Destroy(gameObject);
    }

    private void ReturnEnemy()
    {
        EnemySpawner enemySpawner = FindObjectOfType<EnemySpawner>();
        transform.position = _playerTransform.position + enemySpawner.RelativeSpawnPoints[Random.Range(0, enemySpawner.RelativeSpawnPoints.Count)].position;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _enemyAnimator.CrossFade(AttackState, 0.1f, 0, 0);
            PlayerStats player = other.gameObject.GetComponent<PlayerStats>();
            player.PlayerTakeDamage(CurrentDamage);
        }
    }

    private void OnDestroy()
    {
        if (!gameObject.scene.isLoaded) // stops the spawning error from appearing when stop play mode
        {
            return;
        }

        EnemySpawner enemySpawner = FindObjectOfType<EnemySpawner>();
        enemySpawner.OnEnemyKilled();
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