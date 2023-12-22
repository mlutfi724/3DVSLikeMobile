using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordProjectileBehaviour : ProjectileWeaponBehaviour
{
    [SerializeField] private float _rotatingSpeed;
    protected PlayerStats Player;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        Player = FindObjectOfType<PlayerStats>();
        transform.position = new Vector3(Player.transform.position.x, Player.transform.position.y + 0.7f, Player.transform.position.z); // make the weapon spawn at the perfect height
    }

    // Update is called once per frame
    private void Update()
    {
        transform.position += Direction * CurrentSpeed * Time.deltaTime;
        transform.Rotate(Vector3.right * _rotatingSpeed * Time.deltaTime); // rotating the object
    }
}