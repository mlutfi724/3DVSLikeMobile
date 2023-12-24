using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldOrbitingBehaviour : AreaWeaponBehaviour
{
    private Vector3 orbitPosition;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    private void Update()
    {
        // Calculate the desired position in a circular orbit
        orbitPosition = CalculateOrbitPosition();

        // Set the shield's position to the calculated orbit position
        transform.position = orbitPosition;

        // Rotate the object around the player
        OrbitingPlayer();
    }

    private Vector3 CalculateOrbitPosition()
    {
        // Calculate the desired position in a circular orbit
        float angle = Time.time * CurrentSpeed;
        float x = Player.transform.position.x + Mathf.Cos(angle) * 2f; // You can adjust the radius of the orbit
        float z = Player.transform.position.z + Mathf.Sin(angle) * 2f; // You can adjust the radius of the orbit
        float y = Player.transform.position.y;

        return new Vector3(x, y + Player.WeaponSpawnYPos, z);
    }

    private void OrbitingPlayer()
    {
        // Rotate the shield around the player
        transform.RotateAround(Player.transform.position, Vector3.up, CurrentSpeed * Time.deltaTime);
    }
}