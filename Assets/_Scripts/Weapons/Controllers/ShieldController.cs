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
        GameObject spawnedShield = Instantiate(WeaponPrefab);
        //spawnedShield.transform.position = transform.position;
        //spawnedShield.transform.parent = transform;
    }
}