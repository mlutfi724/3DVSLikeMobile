using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableProps : MonoBehaviour
{
    public float Health;

    public void PropsTakeDamage(float dmg)
    {
        Health -= dmg;
        if (Health <= 0)
        {
            PropsBroken();
        }
    }

    public void PropsBroken()
    {
        Destroy(gameObject);
    }
}