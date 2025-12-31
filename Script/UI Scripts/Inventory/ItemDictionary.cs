using System.Collections.Generic;
using UnityEngine;

public class ItemDictionary : MonoBehaviour
{
    public List<Item> itemsPrefabs;
    private Dictionary<int, GameObject> itemDictionary;

    void Awake()
    {
        itemDictionary = new Dictionary<int, GameObject>();
        for(int i = 0 ; i < itemsPrefabs.Count; i++)
        {
            if(itemsPrefabs[i] != null)
            {
                itemsPrefabs[i].itemId = i+1; //assign unique ID based on index
            }
        }
        foreach(Item item in itemsPrefabs)
        {
            itemDictionary[item.itemId] = item.gameObject;
        }
    }
    public GameObject GetItemPrefabs(int itemId)
    {
        itemDictionary.TryGetValue(itemId, out GameObject prefab);
        if(prefab == null)
        {
            Debug.LogWarning("Item ID " + itemId + " not found in dictionary.");
        }
        return prefab;
    }
}
