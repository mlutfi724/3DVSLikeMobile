using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;

public class Pickup : MonoBehaviour, ICollectible
{
    protected bool HasBeenCollected = false;
    public AudioClip CollectSFX;

    public virtual void Collect()
    {
        HasBeenCollected = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlaySFX();
            Destroy(gameObject);
        }
    }

    private void PlaySFX()
    {
        MMSoundManagerPlayOptions options;
        options = MMSoundManagerPlayOptions.Default;
        options.ID = 402766;
        options.DoNotAutoRecycleIfNotDonePlaying = false;
        options.Volume = 1f;
        MMSoundManagerSoundPlayEvent.Trigger(CollectSFX, options);
    }
}