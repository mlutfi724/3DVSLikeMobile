using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldOrbitingBehaviour : OrbitingWeaponBehaviour
{
    private ShieldController _shieldController;

    // Start is called before the first frame update
    protected override void Start()
    {
        _shieldController = FindObjectOfType<ShieldController>();
        base.Start();
        transform.position = _shieldController.transform.position;
    }

    protected override void Update()
    {
        base.Update();
    }
}