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

        public void Awake()
        {
            ogParentName = transform.parent.name;
            shop = FindObjectOfType<Shop>();
            inventory = FindObjectOfType<Inventory>();
        }

        public void StartProcess()
        {
            buttonParentName = transform.parent.name;
            Debug.Log("Entered parent search");
            Debug.Log(transform.parent.name);
        }

        public void SendBuyItem()
        {
            shop.BuyItem(buttonParentName);
        }

        public void SendSellItem()
        {
            shop.SellItem(buttonParentName);
        }

        public void SendSelectItem()
        {
            inventory.SelectItem(buttonParentName);
        }

        public void ReCheck(string buttonName)
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
