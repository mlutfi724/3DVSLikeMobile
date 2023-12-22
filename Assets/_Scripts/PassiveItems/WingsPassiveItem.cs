using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WingsPassiveItem : PassiveItem
{
    protected override void ApplyModifier()
    {
        Player.CurrentMoveSpeed *= 1 + PassiveItemData.Multiplier / 100f;
    }
}