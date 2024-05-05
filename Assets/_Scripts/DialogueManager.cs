using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    private Dictionary<string, string> dialogues;

    void Start()
    {
        // Create a dialogue dictionary to store phrases with their corresponding keys
        dialogues = new Dictionary<string, string>
        {
            { "welcome", "Welcome, wanderer" },
            { "openShop", "What brings you here today?" },
            { "buy", "What would you like to get?"},
            { "sell", "What do you have for me today?" },
            { "exit", "Goodbye, wanderer"}
        };
    }

    // Call this method to get the corresponding dialogue
    public void SetText(string key)
    {
        string text;
        if (dialogues.TryGetValue(key, out text))
        {
            Debug.Log(text);
        }
        else
        {
            Debug.LogError("Key not found: " + key);
        }
    }
}
