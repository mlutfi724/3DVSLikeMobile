using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropRateManager : MonoBehaviour
{
    public List<Drops> DropsList;

    [System.Serializable]
    public class Drops
    {
        public string Name;
        public GameObject ItemPrefab;
        public float DropRate;
    }

    private void OnDestroy()
    {
        if (!gameObject.scene.isLoaded) // stops the spawning error from appearing when stop play mode
        {
            return;
        }
        float randomNumber = Random.Range(0f, 100f);
        List<Drops> possibleDrops = new List<Drops>();
        foreach (Drops rate in DropsList)
        {
            if (randomNumber <= rate.DropRate)
            {
                possibleDrops.Add(rate);
            }
        }

        // check if there are possible drops
        if (possibleDrops.Count > 0)
        {
            Drops drops = possibleDrops[Random.Range(0, possibleDrops.Count)];
            Instantiate(drops.ItemPrefab, transform.position, Quaternion.identity);
        }
    }
}