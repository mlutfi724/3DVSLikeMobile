using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;

public class SwordProjectileBehaviour : ProjectileWeaponBehaviour
{
    [SerializeField] private float _rotatingSpeed;
    protected PlayerStats Player;

    [Header("AudioSFX")]
    public AudioClip ProjectileSFX;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        Player = FindObjectOfType<PlayerStats>();
        PlaySFX(ProjectileSFX, 222608, 0.5f);
        transform.position = new Vector3(Player.transform.position.x, Player.transform.position.y + 0.7f, Player.transform.position.z); // make the weapon spawn at the perfect height
    }

    // Update is called once per frame
    private void Update()
    {
        transform.position += Direction * CurrentSpeed * Time.deltaTime;
        transform.Rotate(Vector3.right * _rotatingSpeed * Time.deltaTime); // rotating the object
    }

    private void PlaySFX(AudioClip sfx, int soundID, float volume)
    {
        MMSoundManagerPlayOptions options;
        options = MMSoundManagerPlayOptions.Default;
        options.Loop = false;
        options.Volume = volume;
        options.ID = soundID;

        MMSoundManagerSoundPlayEvent.Trigger(sfx, options);
    }
}