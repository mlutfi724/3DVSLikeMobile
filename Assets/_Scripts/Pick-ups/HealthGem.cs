using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthGem : Pickup, ICollectible
{
    public int HealthToRestore;

    public void Collect()
    {
        PlayerStats player = FindObjectOfType<PlayerStats>();
        player.RestoreHealth(HealthToRestore);
    }
}