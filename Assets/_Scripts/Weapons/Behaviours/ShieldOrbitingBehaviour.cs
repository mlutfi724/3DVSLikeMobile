using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldOrbitingBehaviour : AreaWeaponBehaviour
{
    // Start is called before the first frame update
    protected override void Start()
    {
        Player = FindObjectOfType<PlayerStats>();
        base.Start();
        transform.position = new Vector3(Player.transform.position.x + 2f, Player.transform.position.y + 0.7f, Player.transform.position.z);
    }

    private void Update()
    {
        OrbitingPlayer();
    }

    private void OrbitingPlayer()
    {
        transform.RotateAround(Player.transform.position, Vector3.up, CurrentSpeed * Time.deltaTime);
    }
}