using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinachPassiveItem : PassiveItem
{
    protected override void ApplyModifier()
    {
        Player.CurrentMight *= 1 + PassiveItemData.Multiplier / 100f;
    }
}