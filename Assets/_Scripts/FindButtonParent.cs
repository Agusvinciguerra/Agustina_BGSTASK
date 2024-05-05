using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItemsSpace
{
    public class FindButtonParent : MonoBehaviour
    {
        [SerializeField] Shop shop;
        public string buttonParentName;

        public void StartProcess()
        {
            shop = FindObjectOfType<Shop>();
            buttonParentName = transform.parent.name;
        }

        public void SendBuyItem()
        {
            shop.BuyItem(buttonParentName);
        }

        public void SendSellItem()
        {
            shop.BuyItem(buttonParentName);
        }
    }
}
