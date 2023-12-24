using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

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

    [System.Serializable]
    public class WeaponUpgrade
    {
        public int WeaponUpgradeIndex;
        public GameObject InitialWeapon;
        public WeaponScriptableObject WeaponData;
    }

    [System.Serializable]
    public class PassiveItemUpgrade
    {
        public int PassiveItemUpgradeIndex;
        public GameObject InitialPassiveItem;
        public PassiveItemScriptableObject PassiveItemData;
    }

    [System.Serializable]
    public class UpgradeUI
    {
        public TextMeshProUGUI UpgradeNameDisplay;
        public TextMeshProUGUI UpgradeDescriptionDisplay;
        public Image UpgradeIcon;
        public Button UpgradeButton;
    }

    public List<WeaponUpgrade> WeaponUpgradeOptions = new List<WeaponUpgrade>(); //List of upgrade options for weapons
    public List<PassiveItemUpgrade> PassiveItemUpgradeOptions = new List<PassiveItemUpgrade>(); // List of upgrade options for passive items
    public List<UpgradeUI> UpgradeUIOptions = new List<UpgradeUI>(); // List of ui for upgrade options present in the scene

    private PlayerStats _player;

    private void Start()
    {
        _player = GetComponent<PlayerStats>();
    }

    public void AddWeapon(int slotIndex, WeaponController weapon) // add a weapon to specific slot
    {
        WeaponSlots[slotIndex] = weapon;
        WeaponLevels[slotIndex] = weapon.WeaponStatsData.Level;
        WeaponUISlots[slotIndex].enabled = true; // enabled the image component
        WeaponUISlots[slotIndex].sprite = weapon.WeaponStatsData.Icon;

        if (GameManager.instance != null && GameManager.instance.IsChoosingUpgrade)
        {
            GameManager.instance.EndLevelUp();
        }
    }

    public void AddPassiveItem(int slotIndex, PassiveItem passiveItem) // add a passive item to specific slot
    {
        PassiveItemSlots[slotIndex] = passiveItem;
        PassiveItemLevels[slotIndex] = passiveItem.PassiveItemData.Level;
        PassiveItemUISlots[slotIndex].enabled = true; // enabled the image component
        PassiveItemUISlots[slotIndex].sprite = passiveItem.PassiveItemData.Icon;

        if (GameManager.instance != null && GameManager.instance.IsChoosingUpgrade)
        {
            GameManager.instance.EndLevelUp();
        }
    }

    public void LevelUpWeapon(int slotIndex, int upgradeIndex)
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

            WeaponUpgradeOptions[upgradeIndex].WeaponData = upgradedWeapon.GetComponent<WeaponController>().WeaponStatsData;

            if (GameManager.instance != null && GameManager.instance.IsChoosingUpgrade)
            {
                GameManager.instance.EndLevelUp();
            }
        }
    }

    public void LevelUpPassiveItem(int slotIndex, int upgradeIndex)
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

            PassiveItemUpgradeOptions[upgradeIndex].PassiveItemData = upgradedPassiveItem.GetComponent<PassiveItem>().PassiveItemData;

            if (GameManager.instance != null && GameManager.instance.IsChoosingUpgrade)
            {
                GameManager.instance.EndLevelUp();
            }
        }
    }

    private void ApplyUpgradeOptions()
    {
        List<WeaponUpgrade> availableWeaponUpgrades = new List<WeaponUpgrade>(WeaponUpgradeOptions);
        List<PassiveItemUpgrade> availablePassiveItemUpgrades = new List<PassiveItemUpgrade>(PassiveItemUpgradeOptions);

        foreach (var upgradeOption in UpgradeUIOptions)
        {
            if (availableWeaponUpgrades.Count == 0 && availablePassiveItemUpgrades.Count == 0)
            {
                return;
            }

            int upgradeType;

            if (availableWeaponUpgrades.Count == 0)
            {
                upgradeType = 2;
            }
            else if (availablePassiveItemUpgrades.Count == 0)
            {
                upgradeType = 1;
            }
            else
            {
                upgradeType = Random.Range(1, 3); // Choose between weapon and passive items
            }

            if (upgradeType == 1) // Weapon type upgrade
            {
                WeaponUpgrade chosenWeaponUpgrade = availableWeaponUpgrades[Random.Range(0, availableWeaponUpgrades.Count)];

                availableWeaponUpgrades.Remove(chosenWeaponUpgrade);

                if (chosenWeaponUpgrade != null)
                {
                    EnableUpgradeUI(upgradeOption);

                    bool newWeapon = false;
                    for (int i = 0; i < WeaponSlots.Count; i++)
                    {
                        if (WeaponSlots[i] != null && WeaponSlots[i].WeaponStatsData == chosenWeaponUpgrade.WeaponData)
                        {
                            newWeapon = false;
                            if (!newWeapon)
                            {
                                if (!chosenWeaponUpgrade.WeaponData.NextLevelPrefab)
                                {
                                    DisableUpgradeUI(upgradeOption);
                                    break;
                                }

                                upgradeOption.UpgradeButton.onClick.AddListener(() => LevelUpWeapon(i, chosenWeaponUpgrade.WeaponUpgradeIndex)); // Apply button functionality
                                                                                                                                                 // set the description and name to be that of the next level
                                upgradeOption.UpgradeDescriptionDisplay.text = chosenWeaponUpgrade.WeaponData.NextLevelPrefab.GetComponent<WeaponController>().WeaponStatsData.WeaponDescription;
                                upgradeOption.UpgradeNameDisplay.text = chosenWeaponUpgrade.WeaponData.NextLevelPrefab.GetComponent<WeaponController>().WeaponStatsData.WeaponName;
                            }
                            break;
                        }
                        else
                        {
                            newWeapon = true;
                        }
                    }
                    if (newWeapon) //spawn a new weapon
                    {
                        upgradeOption.UpgradeButton.onClick.AddListener(() => _player.SpawnWeaponController(chosenWeaponUpgrade.InitialWeapon)); // Apply button functionality
                                                                                                                                                 //Apply initial description and name
                        upgradeOption.UpgradeDescriptionDisplay.text = chosenWeaponUpgrade.WeaponData.WeaponDescription;
                        upgradeOption.UpgradeNameDisplay.text = chosenWeaponUpgrade.WeaponData.WeaponName;
                    }
                    upgradeOption.UpgradeIcon.sprite = chosenWeaponUpgrade.WeaponData.Icon;
                }
            }
            else if (upgradeType == 2) // Passive item type upgrade
            {
                PassiveItemUpgrade chosenPassiveItemUpgrade = availablePassiveItemUpgrades[Random.Range(0, availablePassiveItemUpgrades.Count)];
                availablePassiveItemUpgrades.Remove(chosenPassiveItemUpgrade);

                if (chosenPassiveItemUpgrade != null)
                {
                    EnableUpgradeUI(upgradeOption);

                    bool newPassive = false;
                    for (int i = 0; i < PassiveItemSlots.Count; i++)
                    {
                        if (PassiveItemSlots[i] != null && PassiveItemSlots[i].PassiveItemData == chosenPassiveItemUpgrade.PassiveItemData)
                        {
                            newPassive = false;
                            if (!newPassive)
                            {
                                if (!chosenPassiveItemUpgrade.PassiveItemData.NextLevelPrefab)
                                {
                                    DisableUpgradeUI(upgradeOption);
                                    break;
                                }

                                upgradeOption.UpgradeButton.onClick.AddListener(() => LevelUpPassiveItem(i, chosenPassiveItemUpgrade.PassiveItemUpgradeIndex)); // Apply button functionality
                                                                                                                                                                // set the description and name to be that of the next level
                                upgradeOption.UpgradeDescriptionDisplay.text = chosenPassiveItemUpgrade.PassiveItemData.NextLevelPrefab.GetComponent<PassiveItem>().PassiveItemData.PassiveItemDescription;
                                upgradeOption.UpgradeNameDisplay.text = chosenPassiveItemUpgrade.PassiveItemData.NextLevelPrefab.GetComponent<PassiveItem>().PassiveItemData.PassiveItemName;
                            }
                            break;
                        }
                        else
                        {
                            newPassive = true;
                        }
                    }
                    if (newPassive) // spawn a new passive
                    {
                        upgradeOption.UpgradeButton.onClick.AddListener(() => _player.SpawnPassiveItem(chosenPassiveItemUpgrade.InitialPassiveItem)); // Apply button functionality
                                                                                                                                                      //Apply initial description and name
                        upgradeOption.UpgradeDescriptionDisplay.text = chosenPassiveItemUpgrade.PassiveItemData.PassiveItemDescription;
                        upgradeOption.UpgradeNameDisplay.text = chosenPassiveItemUpgrade.PassiveItemData.PassiveItemName;
                    }
                    upgradeOption.UpgradeIcon.sprite = chosenPassiveItemUpgrade.PassiveItemData.Icon;
                }
            }
        }
    }

    private void RemoveUpgradeOptions()
    {
        foreach (var upgradeOptions in UpgradeUIOptions)
        {
            upgradeOptions.UpgradeButton.onClick.RemoveAllListeners();
            DisableUpgradeUI(upgradeOptions); //Call the disable upgrade ui method here to disable all UI options before applying upgrades to them
        }
    }

    public void RemoveAndApplyUpgrades()
    {
        RemoveUpgradeOptions();
        ApplyUpgradeOptions();
    }

    private void DisableUpgradeUI(UpgradeUI ui)
    {
        ui.UpgradeNameDisplay.transform.parent.gameObject.SetActive(false);
    }

    private void EnableUpgradeUI(UpgradeUI ui)
    {
        ui.UpgradeNameDisplay.transform.parent.gameObject.SetActive(true);
    }
}