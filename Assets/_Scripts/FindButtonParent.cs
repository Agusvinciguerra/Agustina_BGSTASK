using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ItemsSpace
{
    public class FindButtonParent : MonoBehaviour
    {
        [SerializeField] private Shop shop;
        [SerializeField] private Inventory inventory;
        public string buttonParentName;
        private string ogParentName;

        public void Awake() // Assign references to variables
        {
            ogParentName = transform.parent.name;
            shop = FindObjectOfType<Shop>();
            inventory = FindObjectOfType<Inventory>();
        }

        public void StartProcess() // Get the parent name of the button
        {
            buttonParentName = transform.parent.name;
        }

        public void SendBuyItem() // Send the button parent name to the shop script
        {
            shop.BuyItem(buttonParentName);
        }

        public void SendSelectItem() // Send the button parent name to the inventory script
        {
            inventory.SelectItem(buttonParentName);
        }

        public void ReCheck(string buttonName) // Check if the button parent name is the same as the item slot name
        {
            if (transform.parent.name == "ItemSlot" && gameObject.name == buttonName)
            {
                gameObject.GetComponent<Button>().enabled = false;
            } else 
            {
                gameObject.GetComponent<Button>().enabled = true;
            }
        }
    }
}
