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
        public bool isShop = false;
        [SerializeField] private Transform[] itemSlots;
        [SerializeField] private TextMeshProUGUI[] itemText; 
        [SerializeField] private FindButtonParent[] findButtonParent;
        [SerializeField] private Sprite transparent;
        private PlayerMovement playerMovement;
        private Inventory inventory;
        private DialogueManager dialogueManager; 
        private string slotOGName;

        void Awake() // Assign references to variables
        {
            playerMovement = FindObjectOfType<PlayerMovement>();
            inventory = FindObjectOfType<Inventory>();
            dialogueManager = FindObjectOfType<DialogueManager>();
        }

        public void CloseShop() // Close shop
        {
            playerMovement.positionLocked = false;
            isShop = false;
        }

        public void EnterShop() // Open shop and display items
        {
            playerMovement.positionLocked = true;
            findButtonParent = FindObjectsOfType<FindButtonParent>();
            isShop = true;

            dialogueManager.SetShopText("openShop");
            List<Sprite> matchedSprites = new List<Sprite>();
            List<float> matchedPrices = new List<float>();
            List<string> matchedType = new List<string>();

            Dictionary<string, (Sprite sprite, float price, string type)> itemData = new Dictionary<string, (Sprite sprite, float price, string type)>(); // Create a new dictionary to update items
            for (int i = 0; i < itemObjects.Length; i++)
            {
                string itemName = itemObjects[i].name;
                float itemPrice = ItemData.instance.itemDictionary[itemName].Item1;
                string itemType = ItemData.instance.itemDictionary[itemName].Item2;
                itemData[itemName] = (itemObjects[i], itemPrice, itemType);
            }

            foreach (KeyValuePair<string, Tuple<float, string, bool>> item in ItemData.instance.itemDictionary) // Check if the item is bought
            {
                if (itemData.TryGetValue(item.Key, out var data) && !item.Value.Item3)
                {
                    matchedSprites.Add(data.sprite);
                    matchedPrices.Add(data.price);
                    matchedType.Add(data.type);
                }
            }
            
            int c = 0;
            foreach (Transform slot in itemSlots) // Display items in shop making sure image matches text
            {
                if (c < matchedSprites.Count)
                {
                    Sprite currentSprite = matchedSprites[c];

                    slot.gameObject.GetComponent<Image>().sprite = matchedSprites[c];
                    itemText[c].text = matchedType[c] + "\n$" + matchedPrices[c];
                    c++;
                    
                    slotOGName = slot.transform.parent.name;

                    slot.transform.parent.name = currentSprite.name;
                }
            }
            
            foreach (FindButtonParent findButtonParent in findButtonParent) // Find the button parent name 
            {
                findButtonParent.StartProcess();
            }
        }

        public void BuyItem(string itemName) // Buy item from shop
        {
            if (ItemData.instance.itemDictionary.ContainsKey(itemName))
            {
                var item = ItemData.instance.itemDictionary[itemName];
                if (inventory.playerMoney >= item.Item1) // Check if player has the money
                {
                    inventory.playerMoney -= item.Item1; // Take price from player's money
                    ItemData.instance.itemDictionary[itemName] = new Tuple<float, string, bool>(item.Item1, item.Item2, true); // Set bool's condition to true
                    inventory.UpdateUI(); // Update money amount in UI
                    
                    inventory.boughtItems.Add(itemName); // Add item to inventory
                    
                    dialogueManager.SetShopText("buy");
                    ClearSlot(itemName);
                }
                else
                {
                    dialogueManager.SetShopText("noMoney");
                }
            }
        }

        void ClearSlot(string clearedItem) // Clear shop slot after buying an item
        {
            for (int i = 0; i < itemSlots.Length; i++)
            {
                Transform slot = itemSlots[i];
                if (slot.transform.parent.name == clearedItem)
                {
                    slot.transform.parent.name = slotOGName;
                    itemText[i].text = "Bought" + "\n" + "item"; // Clear the corresponding item text
                    slot.gameObject.GetComponent<Image>().sprite = transparent; // Clear the item image
                    foreach (FindButtonParent findButtonParent in findButtonParent)
                    {
                        findButtonParent.ReCheck("Buy");
                    }
                    break;
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