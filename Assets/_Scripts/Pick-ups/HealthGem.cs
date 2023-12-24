using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthGem : Pickup
{
    public int HealthToRestore;

    public override void Collect()
    {
        if (HasBeenCollected)
        {
            return;
        }
        else
        {
            base.Collect();
        }

        PlayerStats player = FindObjectOfType<PlayerStats>();
        player.RestoreHealth(HealthToRestore);
    }
}