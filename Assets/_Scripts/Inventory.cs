using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace ItemsSpace
{
    public class Inventory : MonoBehaviour
    {
        [SerializeField] private Transform[] inventorySlots;
        [SerializeField] private GameObject inventory;
        public float playerMoney = 10;
        public List<string> boughtItems = new List<string>();
        [SerializeField] private Shop shop;
        private PlayerMovement playerMovement;
        private FindButtonParent[] findButtonParent;
        private bool inventoryOpen = false;
        [SerializeField] private TextMeshProUGUI itemDetails;
        [SerializeField] private Button sellButton;

        void Awake()
        {
            shop = FindObjectOfType<Shop>();
            playerMovement = FindObjectOfType<PlayerMovement>();
        }

        public void DebugInventory()
        {
            foreach (string item in boughtItems)
            {
                Debug.Log("Item in inventory: " + item);
            }
        }

        // Update is called once per frame
        void Update()
        {
            // Check for TAB input and open/close the inventory
            if (Input.GetKeyDown(KeyCode.Tab) && !inventoryOpen)
            {
                inventoryOpen = true;
                OpenInventory();
            } else if (Input.GetKeyDown(KeyCode.Tab) && inventoryOpen)
            {
                inventoryOpen = false;
                CloseInventory();
            }
        }

        void OpenInventory()
        {
            // Open the inventory
            inventory.SetActive(true);
            playerMovement.positionLocked = true;

            findButtonParent = FindObjectsOfType<FindButtonParent>();

            itemDetails.text = $"Select\nitem";

            // Clear the inventory slots
            foreach (Transform slot in inventorySlots)
            {
                slot.gameObject.GetComponent<Image>().sprite = null;
            }

            // Populate the inventory slots with the bought items
            for (int i = 0; i < boughtItems.Count; i++)
            {
                string itemName = boughtItems[i];
                Sprite itemSprite = null;

                // Iterate over the itemSprites array to find the sprite that matches the item name
                foreach (Sprite sprite in shop.itemObjects) // Use the shop reference here
                {
                    if (sprite.name == itemName)
                    {
                        itemSprite = sprite;
                        break;
                    }
                }

                if (itemSprite != null)
                {
                    inventorySlots[i].gameObject.GetComponent<Image>().sprite = itemSprite;
                    inventorySlots[i].gameObject.transform.parent.name = itemSprite.name;
                }
            }
  
            foreach (FindButtonParent findButtonParent in findButtonParent)
            {
                Debug.Log("Entered parent search");
                findButtonParent.StartProcess();
            }
        }

        public void SelectItem(string itemName)
        {
            // Get the item details
            if (boughtItems.Contains(itemName))
            {       
                if (ItemData.instance.itemDictionary.TryGetValue(itemName, out Tuple<float, string, bool> details))
                {
                    float price = details.Item1;
                    string type = details.Item2;
                    bool bought = details.Item3;

                    itemDetails.text = $"{type}\n${price}";
                }
                else
                {
                    Debug.Log($"Item not found: {itemName}");
                }
                Debug.Log("Item selected: " + itemName);
                sellButton.onClick.AddListener(() => SellItem(itemName));
            }
        }

        /*public void SellItem(string itemName)
        {
            // Check if the item is in the inventory
            if (boughtItems.Contains(itemName))
            {
                // Remove the item from the inventory
                boughtItems.Remove(itemName);
                playerMoney += ItemData.instance.itemDictionary[itemName].Item1;
                Debug.Log("Item sold: " + itemName);
                Debug.Log("Player money: " + playerMoney);
                
                // Clear the inventory slots
                itemDetails.text = $"Select\nitem";
            }

            /*if (ItemData.instance.itemDictionary.TryGetValue(itemName, out Tuple<float, string, bool> details))
            {
                float price = details.Item1;
                string type = details.Item2;
                bool bought = details.Item3;

                itemDetails.text = $"{type}\n${price}";
            }
            else
            {
                Debug.Log($"Item not found: {itemName}");
            }
        }*/

        public void SellItem(string itemName)
        {
            // Check if the item is in the inventory
            int itemIndex = boughtItems.IndexOf(itemName);
            if (itemIndex != -1)
            {
                // Remove the item from the inventory
                boughtItems.RemoveAt(itemIndex);
                playerMoney += ItemData.instance.itemDictionary[itemName].Item1;
                Debug.Log("Item sold: " + itemName);
                Debug.Log("Player money: " + playerMoney);
                
                // Clear the inventory slots
                itemDetails.text = $"Select\nitem";

                // Set the slot sprite to null
                inventorySlots[itemIndex].gameObject.GetComponent<Image>().sprite = null;
                Debug.Log(inventorySlots[itemIndex]);
            }
            itemName = null;
        }

        void CloseInventory()
        {
            // Close the inventory
            inventory.SetActive(false);
            playerMovement.positionLocked = false;
        }
    }
}