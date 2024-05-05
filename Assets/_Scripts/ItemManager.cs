using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace ItemsSpace
{
    public class ItemManager : MonoBehaviour
    {
        [Header("Serialized")]
        [SerializeField] private TextMeshProUGUI UIText;
        [SerializeField] private GameObject[] items;
        [SerializeField] private Image[] images;
        [SerializeField] private GameObject inventory;    
        
        [Header("Private")]
        Dictionary <GameObject, Tuple<float, bool, string>> itemDetails;
        private string type;
        private string itemName;
        private bool inventoryOpen = false;
        private float playerMoney = 50f;
        private int index;
        private int activeObjIndex;
        private GameObject activeObj;
        private GameObject player;

        void Awake()
        {
            // Find player
            player = GameObject.Find("Player");
        }

        public void GetType(string itemType, string name)
        {
            /*type = itemType;
            itemName = name;
            activeObj = Array.Find(items, item => item.name == itemName);
        
            itemDetails = new Dictionary<GameObject, Tuple<float, bool, string>>
            {
                {items[0], new Tuple<float, bool, string>(10.0f, false, type)},
                {items[1], new Tuple<float, bool, string>(20.0f, false, type)}
            };

            SetUI();*/

            type = itemType;
            itemName = name;
            activeObj = Array.Find(items, item => item.name == itemName);

            // Find the index of activeObj in the items array
            activeObjIndex = Array.IndexOf(items, activeObj);

            itemDetails = new Dictionary<GameObject, Tuple<float, bool, string>>
            {
                {items[0], new Tuple<float, bool, string>(10.0f, false, type)},
                {items[1], new Tuple<float, bool, string>(20.0f, false, type)}
            };

            // Check if activeObj is in the items array
            if (activeObjIndex != -1)
            {
                // Get the details of activeObj
                Tuple<float, bool, string> activeObjDetails = itemDetails[items[activeObjIndex]];

                // Now you can use activeObjDetails to do whatever you need
            }

            Debug.Log("item index is " + items[activeObjIndex]);

            SetUI();
        }

        void Update()
        {
            // Check for TAB input and open/close the inventory
            if (Input.GetKeyDown(KeyCode.Tab) && !inventoryOpen)
            {
                inventoryOpen = true;
                Inventory();
            } else if (Input.GetKeyDown(KeyCode.Tab) && inventoryOpen)
            {
                inventoryOpen = false;
                CloseInventory();
            }
        }

        // Set the UI text to state item name and price
        void SetUI()
        {
            if (itemDetails.TryGetValue(items[activeObjIndex], out Tuple<float, bool, string> details)) // Check if the item exists in the dictionary
            {
                float price = details.Item1;
                bool bought = details.Item2;
                string type = details.Item3;

                UIText.text = $"Buy {type} for {price}?"; // Update the UI tex
                Debug.Log($"Item: {type}, Price: {price}");
            } else // Execute if the item is not in the dictionary
            {
                UIText.text = "Sorry, this item is not for sale.";
                Debug.Log("Sorry, this item is not for sale.");
            }
        }

        // Check for the player's money and act accordingly
        public void BuyItem()
        {
            if (itemDetails.TryGetValue(items[activeObjIndex], out Tuple<float, bool, string> details)) // Check if the item exists in the dictionary
            {
                float price = details.Item1;
                bool bought = details.Item2;
                string type = details.Item3;

                if (playerMoney >= price) // Check if the item is not bought and the player has enough money
                {
                    playerMoney -= price; // Take the price from the player's money
                    
                    Debug.Log($"You bought {items[activeObjIndex]} for {price}!");
                    Debug.Log($"{playerMoney} left.");

                    // Update the item status to bought
                    itemDetails[items[activeObjIndex]] = new Tuple<float, bool, string>(price, true, type);
                    bought = itemDetails[items[activeObjIndex]].Item2; // Get the updated bought status
                    Debug.Log($"Item: {type}, Price: {price}, Bought: {bought}");
                } else // Execute if the player does not have enough money
                {
                    Debug.Log("You do not have enough money to buy this item.");
                }
            }
        }

        public void FindItemToSell(string buttonParentName)
        {
            var keys = new List<GameObject>(itemDetails.Keys);
            Debug.Log("entered find item to sell");

            foreach (GameObject key in keys)
            {
                var item = new KeyValuePair<GameObject, Tuple<float, bool, string>>(key, itemDetails[key]);
                if (item.Value.Item2 || item.Key.name == buttonParentName) 
                {
                    items[activeObjIndex] = item.Key; // Set items[activeObjIndex] to the GameObject that matches the conditions
                    Debug.Log("found the item to sell");
                    SellItem();
                }
            }
        }

        public void SellItem()
        {
            if (itemDetails.TryGetValue(items[activeObjIndex], out Tuple<float, bool, string> details)) // Check if the item exists in the dictionary
            {
                float price = details.Item1;
                bool bought = details.Item2;
                string type = details.Item3;

                Debug.Log($"Object name: {items[activeObjIndex].name}, Price: {price}, Bought: {bought}."); 
            
                playerMoney += price; // Add the price to the player's money
                Debug.Log($"You sold {items[activeObjIndex].name} for {price}!");
                Debug.Log($"{playerMoney} left.");

                // Update the item status to not bought
                itemDetails[items[activeObjIndex]] = new Tuple<float, bool, string>(price, false, type);

                // Disable the image
                Image image = Array.Find(images, img => img.name == items[activeObjIndex].name);
                if (image != null)
                {
                    image.gameObject.SetActive(false);
                }
                
            }
        }

        public void EquipItem(string buttonParentName)
        {   
            foreach (KeyValuePair<GameObject, Tuple<float, bool, string>> item in itemDetails)
            {
                if (item.Value.Item2 && item.Key.name == buttonParentName)
                {
                    Sprite currentSprite = player.transform.Find(item.Value.Item3).GetComponent<SpriteRenderer>().sprite;
                    Sprite sprite = item.Key.GetComponent<SpriteRenderer>().sprite;
                    Debug.Log("the new shirt is " + sprite); 

                    Transform childTransform = player.transform.Find(item.Value.Item3);
                    SpriteRenderer spriteRenderer = childTransform.GetComponent<SpriteRenderer>();
                    if (spriteRenderer != null)
                    {
                        spriteRenderer.sprite = sprite;
                    }
                
                    if (currentSprite == sprite)
                    {
                        Debug.Log("You are already wearing this item.");
                    } else {
                        Debug.Log($"You equipped {itemName}!");
                    }
                }
            }
        }

        // Arrange the inventory
        void Inventory()
        {
            // Display the inventory
            inventory.SetActive(true);

            // Check for bought items
            foreach (KeyValuePair<GameObject, Tuple<float, bool, string>> item in itemDetails)
            {
                if (item.Value.Item2)
                {
                    // Get the item name
                    string itemName = item.Key.name;
                    Debug.Log($"{itemName} is marked as bought.");

                    // Find the image within the array that has the same name as the bought item
                    Image image = Array.Find(images, img => img.name == itemName);
                    if (image != null)
                    {
                        image.gameObject.SetActive(true);
                        float price = item.Value.Item1;

                        TextMeshProUGUI priceText = image.GetComponentInChildren<TextMeshProUGUI>();
                        if (priceText != null)
                        {
                            priceText.text = $"${price}";
                        }
                    }
                }
            }


        }

        void CloseInventory()
        {
            inventory.SetActive(false);
        }
    }
}