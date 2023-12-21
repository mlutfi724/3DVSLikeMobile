using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthGem : MonoBehaviour, ICollectible
{
    public int HealthToRestore;

    public void Collect()
    {
        PlayerStats player = FindObjectOfType<PlayerStats>();
        player.RestoreHealth(HealthToRestore);
        Destroy(gameObject);
    }
}