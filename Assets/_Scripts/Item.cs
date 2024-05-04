using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemType itemType;
    public enum ItemType
    {
        Skirt, 
        Scarf,
        Sneakers
    }

    void Awake()
    {
        Debug.Log(itemType.ToString());
    }
}
