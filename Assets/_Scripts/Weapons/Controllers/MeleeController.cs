using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeController : WeaponController
{
    protected override void Start()
    {
        base.Start();
    }

    protected override void Attack()
    {
        base.Attack();
        SpawnMelee();
    }

    private void SpawnMelee()
    {
        GameObject spawnedMelee = Instantiate(WeaponStatsData.WeaponPrefab);
        spawnedMelee.transform.parent = transform;
        spawnedMelee.transform.position = transform.position;
    }
}