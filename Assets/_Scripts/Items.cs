using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Items : MonoBehaviour
{
    public Dictionary<string, float> itemPrices; 
    [SerializeField] private TextMeshProUGUI UIText;
    private float playerMoney = 50.5f;
    private string itemName;

    void Awake()
    {
        // Add item names and prices to a dictionary
        itemPrices = new Dictionary<string, float>
        {
            {"Scarf", 10.0f},
            {"Skirt", 20.0f}
        };
    }

    // Get the item name
    public void GetName(string name)
    {
        itemName = name;
        SetUI();
    }

    // Set the UI text to state item name and price
    public void SetUI()
    {
        if (itemPrices.TryGetValue(itemName, out float price)) // Check if the item exists in the dictionary
        {
            UIText.text = $"Buy {itemName} for {price}?"; // Update the UI text
            Debug.Log($"Item: {itemName}, Price: {price}");
        }
        else // Execute if the item is not in the dictionary
        {
            UIText.text = "Sorry, this item is not for sale.";
        }
    }

    // Check for the player's money and act accordingly
    public void BuyItem()
    {
        if (itemPrices.TryGetValue(itemName, out float price)) // Check if the item exists in the dictionary
        {
            if (playerMoney >= price) // Check if the player has enough money
            {
                playerMoney -= price; // Subtract the price from the player's money
                Debug.Log($"You bought {itemName} for {price}!");
                Debug.Log($"{playerMoney} left.");
            }
            else // Execute if the player does not have enough money
            {
                Debug.Log("You do not have enough money to buy this item.");
            }
        }
    }
}
