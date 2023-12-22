using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveItem : MonoBehaviour
{
    protected PlayerStats Player;
    public PassiveItemScriptableObject PassiveItemData;

    protected virtual void ApplyModifier()
    {
        // Apply the boost value to the appropriate stat in the child classes
    }

    // Start is called before the first frame update
    private void Start()
    {
        Player = FindObjectOfType<PlayerStats>();
        ApplyModifier();
    }
}