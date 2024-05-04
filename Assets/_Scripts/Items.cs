using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Items : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI UIText;
    [SerializeField] private GameObject inventory;    
    [SerializeField] private Image[] images;
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private GameObject player;
    private float playerMoney = 50f;
    private string itemName;
    Dictionary <string, Tuple<float, bool>> itemDetails;
    private bool inventoryOpen = false;

    void Awake()
    {
        // Find player
        player = GameObject.Find("Player");

        // Add item names and prices to a dictionary
        itemDetails = new Dictionary<string, Tuple<float, bool>>
        {
            {"Scarf", new Tuple<float, bool>(10.0f, false)},
            {"Skirt", new Tuple<float, bool>(20.0f, false)}
        };
    }

    // Get the item name
    public void GetName(string name)
    {
        itemName = name;
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
        if (itemDetails.TryGetValue(itemName, out Tuple<float, bool> details)) // Check if the item exists in the dictionary
        {
            float price = details.Item1;
            bool bought = details.Item2;

            UIText.text = $"Buy {itemName} for {price}?"; // Update the UI tex
            Debug.Log($"Item: {itemName}, Price: {price}");
            Debug.Log($"Item bought: {bought}");
        } else // Execute if the item is not in the dictionary
        {
            UIText.text = "Sorry, this item is not for sale.";
        }
    }

    // Check for the player's money and act accordingly
    public void BuyItem()
    {
        if (itemDetails.TryGetValue(itemName, out Tuple<float, bool> details)) // Check if the item exists in the dictionary
        {
            float price = details.Item1;
            bool bought = details.Item2;

            if (playerMoney >= price) // Check if the item is not bought and the player has enough money
            {
                playerMoney -= price; // Take the price from the player's money
                Debug.Log($"You bought {itemName} for {price}!");
                Debug.Log($"{playerMoney} left.");

                // Update the item status to bought
                itemDetails[itemName] = new Tuple<float, bool>(price, true);
            } else // Execute if the player does not have enough money
            {
                Debug.Log("You do not have enough money to buy this item.");
            }
        }
    }

    public void SellItem()
    {
        if (itemDetails.TryGetValue(itemName, out Tuple<float, bool> details)) // Check if the item exists in the dictionary
        {
            float price = details.Item1;
            bool bought = details.Item2;

            if (bought) // Check if the item is bought
            {
                playerMoney += price; // Add the price to the player's money
                Debug.Log($"You sold {itemName} for {price}!");
                Debug.Log($"{playerMoney} left.");

                // Update the item status to not bought
                itemDetails[itemName] = new Tuple<float, bool>(price, false);

                // Disable the image
                Image image = Array.Find(images, img => img.name == itemName);
                if (image != null)
                {
                    image.gameObject.SetActive(false);
                }
            }
        }
    }

    public void EquipItem()
    {
        Sprite currentSprite = player.transform.Find("Shirt").GetComponent<SpriteRenderer>().sprite;
        Sprite sprite = Array.Find(sprites, spr => spr.name == itemName);

        Transform childTransform = player.transform.Find("Shirt");
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

    // Arrange the inventory
    void Inventory()
    {
        // Display the inventory
        inventory.SetActive(true);

        // Check for bought items
        foreach (KeyValuePair<string, Tuple<float, bool>> item in itemDetails)
        {
            if (item.Value.Item2)
            {
                // Get the item name
                string itemName = item.Key;

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
