using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItemsSpace
{
    public class Item : MonoBehaviour
    {
        private Items items;
        [SerializeField] private ItemManager itemManager;

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
            // Find the Items and Item scripts
            itemManager = FindObjectOfType<ItemManager>();
            item = FindObjectOfType<Item>();
        }

        public void OnTriggerEnter2D()
        {
            // Get the item name and send it to the Items script
            itemManager.GetType(gameObject.GetComponent<Item>().itemType.ToString(), gameObject.name);

            // Display UI
            buyUI.SetActive(true);
        }
    }
}