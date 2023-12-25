using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAreaWeaponBehaviour : MeleeWeaponBehaviour
{
    protected override void Start()
    {
        base.Start();
    }

    //protected override void OnTriggerEnter(Collider other)
    //{
    //    base.OnTriggerEnter(other);
    //    if (other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("Prop"))
    //    {
    //        // enter attack animation state
    //        Player.Animator.SetBool(Player.IsAttackHash, true);
    //    }
    //}

    private void OnDestroy()
    {
        if (!gameObject.scene.isLoaded) // stops the spawning error from appearing when stop play mode
        {
            return;
        }
        // exit attack animation state
        Player.Animator.SetBool(Player.IsAttackHash, false);
    }
}