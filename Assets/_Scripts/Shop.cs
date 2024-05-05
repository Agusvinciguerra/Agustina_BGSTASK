using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace ItemsSpace
{
    public class Shop : MonoBehaviour
    {
        public Sprite[] itemObjects;
        [SerializeField] private Transform[] itemSlots;
        [SerializeField] private TextMeshProUGUI[] itemText; 
        private Inventory inventory;
        [SerializeField] private PlayerMovement playerMovement;
        private DialogueManager dialogueManager; 
        [SerializeField] private FindButtonParent[] findButtonParent;
        private string slotOGName;

        void Awake()
        {
            playerMovement = FindObjectOfType<PlayerMovement>();
            inventory = FindObjectOfType<Inventory>();
            dialogueManager = FindObjectOfType<DialogueManager>();
        }

        public void CloseShop()
        {
            playerMovement.positionLocked = false;
        }

        // Sets the items in shop
        /*public void EnterShop()
        {
            playerMovement.positionLocked = true;
            List<Sprite> matchedSprites = new List<Sprite>();
            List<float> matchedPrices = new List<float>();
            List<string> matchedType = new List<string>();

            foreach (KeyValuePair<string, Tuple<float, string, bool>> item in ItemData.instance.itemDictionary)
            {
                for (int i = 0; i < itemObjects.Length; i++)
                {
                    if (itemObjects[i].name == item.Key)
                    {
                        //Debug.Log("Found matching Sprite for key: " + item.Key + " at index: " + i + " with name: " + itemObjects[i].name);
                        //Debug.Log("Price of the item: " + item.Value.Item1);
                        //Debug.Log("Type of the item: " + item.Value.Item2);
                        matchedSprites.Add(itemObjects[i]);
                        matchedPrices.Add(item.Value.Item1);
                        matchedType.Add(item.Value.Item2);
                    }
                }
            }

            int c = 0;
            foreach (Transform slot in itemSlots)
            {
                if (c < matchedSprites.Count)
                {
                    Sprite currentSprite = matchedSprites[c];
                    //Debug.Log("Current sprite name: " + currentSprite.name);

                    slot.gameObject.GetComponent<Image>().sprite = matchedSprites[c];
                    itemText[c].text = matchedType[c] + "\n$" + matchedPrices[c];
                    c++;
                    
                    slotOGName = slot.transform.parent.name;

                    slot.transform.parent.name = currentSprite.name;
                }
            }
            
            foreach (FindButtonParent findButtonParent in findButtonParent)
            {
                findButtonParent.StartProcess();
            }
        }*/

        public void EnterShop()
        {
            playerMovement.positionLocked = true;
            findButtonParent = FindObjectsOfType<FindButtonParent>();

            dialogueManager.SetShopText("openShop");
            List<Sprite> matchedSprites = new List<Sprite>();
            List<float> matchedPrices = new List<float>();
            List<string> matchedType = new List<string>();

            // Create a dictionary that maps item names to their corresponding sprites, prices, and types
            Dictionary<string, (Sprite sprite, float price, string type)> itemData = new Dictionary<string, (Sprite sprite, float price, string type)>();
            for (int i = 0; i < itemObjects.Length; i++)
            {
                string itemName = itemObjects[i].name;
                float itemPrice = ItemData.instance.itemDictionary[itemName].Item1;
                string itemType = ItemData.instance.itemDictionary[itemName].Item2;
                itemData[itemName] = (itemObjects[i], itemPrice, itemType);
            }

            // Now you can look up the sprite, price, and type for an item in constant time
            foreach (KeyValuePair<string, Tuple<float, string, bool>> item in ItemData.instance.itemDictionary)
            {
                if (itemData.TryGetValue(item.Key, out var data) && !item.Value.Item3)
                {
                    matchedSprites.Add(data.sprite);
                    matchedPrices.Add(data.price);
                    matchedType.Add(data.type);
                }
            }
            
            int c = 0;
            foreach (Transform slot in itemSlots)
            {
                if (c < matchedSprites.Count)
                {
                    Sprite currentSprite = matchedSprites[c];
                    //Debug.Log("Current sprite name: " + currentSprite.name);

                    slot.gameObject.GetComponent<Image>().sprite = matchedSprites[c];
                    itemText[c].text = matchedType[c] + "\n$" + matchedPrices[c];
                    c++;
                    
                    slotOGName = slot.transform.parent.name;

                    slot.transform.parent.name = currentSprite.name;
                    Debug.Log("Reached this point");
                }
            }
            
            foreach (FindButtonParent findButtonParent in findButtonParent)
            {
                Debug.Log("Entered parent search");
                findButtonParent.StartProcess();
            }
        }


        public void BuyItem(string itemName)
        {
            if (ItemData.instance.itemDictionary.ContainsKey(itemName))
            {
                var item = ItemData.instance.itemDictionary[itemName];
                if (inventory.playerMoney >= item.Item1) // Check if player has the money
                {
                    inventory.playerMoney -= item.Item1; // Take price from player's money
                    ItemData.instance.itemDictionary[itemName] = new Tuple<float, string, bool>(item.Item1, item.Item2, true); // Set bool's condition to true
                    
                    inventory.boughtItems.Add(itemName); // Add item to inventory
                    inventory.DebugInventory();
                    
                    Debug.Log("Item bought!");
                    dialogueManager.SetShopText("buy");
                    ClearSlot(itemName);
                }
                else
                {
                    Debug.Log("Not enough money!");
                    dialogueManager.SetShopText("noMoney");
                }
            }
            else
            {
                Debug.Log("Item not found!");
            }
        }

        public void SellItem(string itemName)
        {
            if (ItemData.instance.itemDictionary.ContainsKey(itemName))
            {
                var item = ItemData.instance.itemDictionary[itemName];
                if (item.Item3) // Check if player has the item
                {
                    inventory.playerMoney += item.Item1; // Add price to player's money
                    ItemData.instance.itemDictionary[itemName] = new Tuple<float, string, bool>(item.Item1, item.Item2, false); // Set bool's condition to false
                    
                    inventory.boughtItems.Remove(itemName); // Remove item from inventory
                    inventory.DebugInventory();
                    
                    Debug.Log("Item sold!");
                }
                else
                {
                    Debug.Log("Item not found in inventory!");
                }
            }
            else
            {
                Debug.Log("Item not found!");
            }
        }

        void ClearSlot(string clearedItem)
        {
            for (int i = 0; i < itemSlots.Length; i++)
            {
                Transform slot = itemSlots[i];
                if (slot.transform.parent.name == clearedItem)
                {
                    slot.transform.parent.name = slotOGName;
                    itemText[i].text = "Bought" + "\n" + "item"; // Clear the corresponding item text
                    slot.gameObject.GetComponent<Image>().sprite = null; // Clear the item image if needed
                    //slot.transform.parent.gameObject.SetActive(false);
                    foreach (FindButtonParent findButtonParent in findButtonParent)
                    {
                        findButtonParent.ReCheck("Buy");
                    }
                    break; // Exit the loop if you only expect one match
                }
            }
        }

        // Randomizes item appearance in shop
        /*public void EnterShop()
        {
            playerMovement.positionLocked = true;
            List<Sprite> matchedSprites = new List<Sprite>();
            List<float> matchedPrices = new List<float>();

            foreach (KeyValuePair<string, Tuple<float, string, bool>> item in itemDictionary)
            {
                for (int i = 0; i < itemObjects.Length; i++)
                {
                    if (itemObjects[i].name == item.Key)
                    {
                        Debug.Log("Found matching Sprite for key: " + item.Key + " at index: " + i + " with name: " + itemObjects[i].name);
                        Debug.Log("Price of the item: " + item.Value.Item1);
                        matchedSprites.Add(itemObjects[i]);
                        matchedPrices.Add(item.Value.Item1);
                    }
                }
            }

            // Shuffle matchedSprites and matchedPrices together
            int n = matchedSprites.Count;
            System.Random randomize = new System.Random();
            while (n > 1)
            {
                n--;
                int k = randomize.Next(n + 1);
                Sprite value = matchedSprites[k];
                matchedSprites[k] = matchedSprites[n];
                matchedSprites[n] = value;

                float price = matchedPrices[k];
                matchedPrices[k] = matchedPrices[n];
                matchedPrices[n] = price;
            }

            int index = 0;
            foreach (Transform slot in itemSlots)
            {
                if (index < matchedSprites.Count)
                {
                    slot.gameObject.GetComponent<Image>().sprite = matchedSprites[index];
                    itemText[index].text = "$" + matchedPrices[index];
                    index++;
                }
            }
        }*/
    }
}