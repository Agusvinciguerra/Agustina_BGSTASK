using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    private Items items;
    private Item item;
    public ItemType itemType;
    public enum ItemType
    {
        Skirt, 
        Scarf,
        Sneakers
    }
    [SerializeField] private GameObject buyUI;

    void Awake()
    {
        Debug.Log(itemType.ToString());
        
        // Find the Items and Item scripts
        items = FindObjectOfType<Items>();
        item = FindObjectOfType<Item>();
    }

    public void OnTriggerEnter2D()
    {
        // Get the item name and send it to the Items script
        items.GetName(gameObject.GetComponent<Item>().itemType.ToString());

        // Display UI
        buyUI.SetActive(true);
    }
}
