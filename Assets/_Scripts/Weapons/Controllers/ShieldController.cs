using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldController : WeaponController
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    protected override void Attack()
    {
        base.Attack();
        SpawnShield();
    }

    private void SpawnShield()
    {
        GameObject spawnedShield = Instantiate(WeaponStatsData.WeaponPrefab);
        spawnedShield.transform.position = transform.position;
    }
}