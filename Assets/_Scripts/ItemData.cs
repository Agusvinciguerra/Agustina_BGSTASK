using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemData : MonoBehaviour
{
    public static ItemData instance;
    public Dictionary<string, Tuple<float, string, bool>> itemDictionary;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        itemDictionary = new Dictionary<string, Tuple<float, string, bool>>
            {
                { "hat", new Tuple<float, string, bool>(5, "Hat", false) },
                { "redShirt", new Tuple<float, string, bool>(10, "Shirt", false) },
                { "blueShirt", new Tuple<float, string, bool>(20, "Shirt", false) }
            };
    }
}
