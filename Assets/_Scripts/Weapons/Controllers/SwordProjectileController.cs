using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordProjectileController : WeaponController
{
    protected override void Start()
    {
        base.Start();
    }

    protected override void Attack()
    {
        base.Attack();
        SpawnProjectile();
    }

    private void SpawnProjectile()
    {
        GameObject spawnedProjectile = Instantiate(WeaponStatsData.WeaponPrefab);
        Vector3 lastPlayerMoveDirection = new Vector3(PlayerMovement.LastMovementInput.x, 0, PlayerMovement.LastMovementInput.y);
        spawnedProjectile.transform.position = transform.position; // assign the position to the parent transform.position
        spawnedProjectile.GetComponent<SwordProjectileBehaviour>().DirectionChecker(lastPlayerMoveDirection);
    }
}