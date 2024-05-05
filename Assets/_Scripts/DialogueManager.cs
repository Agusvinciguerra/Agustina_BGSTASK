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
        // Create a dialogue dictionary to store phrases with their corresponding keys
        dialogues = new Dictionary<string, string>
        {
            { "welcome", "Welcome, wanderer" },
            { "openShop", "What brings you here today?" },
            { "buy", "Great choice"},
            { "noMoney", "You don't have enough money"},
            { "sell", "What do you have for me today?" },
            { "exit", "Goodbye, wanderer"}
        };
    }

    // Call this method to get the corresponding dialogue
    public void SetLocalText(string key)
    {
        string text;
        if (dialogues.TryGetValue(key, out text))
        {
            //Debug.Log(text);
            dialogueText.text = text;
        }
        else
        {
            Debug.LogError("Key not found: " + key);
        }
    }

    public void SetShopText(string key)
    {
        string text;
        if (dialogues.TryGetValue(key, out text))
        {
            //Debug.Log(text);
            shopText.text = text;
        }
        else
        {
            Debug.LogError("Key not found: " + key);
        }
    } 
}
