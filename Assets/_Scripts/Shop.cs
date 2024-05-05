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
        [SerializeField] private Sprite[] itemObjects;
        [SerializeField] private Transform[] itemSlots;
        [SerializeField] private TextMeshProUGUI[] itemText; 
        private Inventory inventory;
        private PlayerMovement playerMovement;
        private FindButtonParent findButtonParent;
        private string slotOGName;

        void Awake()
        {
            playerMovement = FindObjectOfType<PlayerMovement>();
            inventory = FindObjectOfType<Inventory>();
            findButtonParent = FindObjectOfType<FindButtonParent>();

            EnterShop();
        }

        public void CloseShop()
        {
            playerMovement.positionLocked = false;
        }

        // Sets the items in shop
        public void EnterShop()
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

            findButtonParent.StartProcess();
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
                }
                else
                {
                    Debug.Log("Not enough money!");
                }
            }
            else
            {
                Debug.Log("Item not found!");
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