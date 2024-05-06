using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    private Dictionary<string, string> dialogues;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private TextMeshProUGUI shopText;

    void Start()
    {
        dialogues = new Dictionary<string, string> // Create a dialogue dictionary to store phrases with their corresponding keys
        {
            { "welcome", "Welcome, wanderer" },
            { "openShop", "What brings you here today?" },
            { "buy", "Great choice"},
            { "noMoney", "You don't have enough money"},
            { "exit", "Goodbye, wanderer"}
        };
    }

    public void SetLocalText(string key) // Call this method to get the corresponding dialogue for the shopkeeper text
    {
        string text;
        if (dialogues.TryGetValue(key, out text))
        {
            dialogueText.text = text;
        }
        else
        {
            Debug.LogError("Key not found: " + key);
        }
    }

    public void SetShopText(string key) // Call this method to get the corresponding dialogue for the shop panel text
    {
        string text;
        if (dialogues.TryGetValue(key, out text))
        {
            shopText.text = text;
        }
        else
        {
            Debug.LogError("Key not found: " + key);
        }
    } 
}
