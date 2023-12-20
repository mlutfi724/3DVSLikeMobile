using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordProjectileBehaviour : ProjectileWeaponBehaviour
{
    private SwordProjectileController _projectileController;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        _projectileController = FindObjectOfType<SwordProjectileController>();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        transform.position += Direction * _projectileController.Speed * Time.deltaTime;
    }
}