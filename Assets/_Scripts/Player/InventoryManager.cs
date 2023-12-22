using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    //Weapon

    public List<WeaponController> WeaponSlots = new List<WeaponController>(6);
    public int[] WeaponLevels = new int[6];
    public List<Image> WeaponUISlots = new List<Image>(6);

    //Passive items

    public List<PassiveItem> PassiveItemSlots = new List<PassiveItem>(6);
    public int[] PassiveItemLevels = new int[6];
    public List<Image> PassiveItemUISlots = new List<Image>(6);

    public void AddWeapon(int slotIndex, WeaponController weapon) // add a weapon to specific slot
    {
        WeaponSlots[slotIndex] = weapon;
        WeaponLevels[slotIndex] = weapon.WeaponStatsData.Level;
        WeaponUISlots[slotIndex].enabled = true; // enabled the image component
        WeaponUISlots[slotIndex].sprite = weapon.WeaponStatsData.Icon;
    }

    public void AddPassiveItem(int slotIndex, PassiveItem passiveItem) // add a passive item to specific slot
    {
        PassiveItemSlots[slotIndex] = passiveItem;
        PassiveItemLevels[slotIndex] = passiveItem.PassiveItemData.Level;
        PassiveItemUISlots[slotIndex].enabled = true; // enabled the image component
        PassiveItemUISlots[slotIndex].sprite = passiveItem.PassiveItemData.Icon;
    }

    public void LevelUpWeapon(int slotIndex)
    {
        if (WeaponSlots.Count > slotIndex)
        {
            WeaponController weapon = WeaponSlots[slotIndex];

            if (!weapon.WeaponStatsData.NextLevelPrefab) // check if there is a next level for current weapon
            {
                Debug.LogError("No next level for " + weapon.name);
                return;
            }

            GameObject upgradedWeapon = Instantiate(weapon.WeaponStatsData.NextLevelPrefab, transform.position, Quaternion.identity);
            upgradedWeapon.transform.SetParent(transform);
            AddWeapon(slotIndex, upgradedWeapon.GetComponent<WeaponController>());
            Destroy(weapon.gameObject);
            WeaponLevels[slotIndex] = upgradedWeapon.GetComponent<WeaponController>().WeaponStatsData.Level; // to make sure we have the correct weapon level
        }
    }

    public void LevelUpPassiveItem(int slotIndex)
    {
        if (PassiveItemSlots.Count > slotIndex)
        {
            PassiveItem passiveItem = PassiveItemSlots[slotIndex];

            if (!passiveItem.PassiveItemData.NextLevelPrefab) // check if there is a next level for current passive item
            {
                Debug.LogError("No next level for " + passiveItem.name);
                return;
            }

            GameObject upgradedPassiveItem = Instantiate(passiveItem.PassiveItemData.NextLevelPrefab, transform.position, Quaternion.identity);
            upgradedPassiveItem.transform.SetParent(transform);
            AddPassiveItem(slotIndex, upgradedPassiveItem.GetComponent<PassiveItem>());
            Destroy(passiveItem.gameObject);
            PassiveItemLevels[slotIndex] = upgradedPassiveItem.GetComponent<PassiveItem>().PassiveItemData.Level; // to make sure we have the correct weapon level
        }
    }
}