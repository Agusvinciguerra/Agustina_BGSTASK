using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItemsSpace
{
    public class FindButtonParent : MonoBehaviour
    {
        [SerializeField] ItemManager itemManager;
        public string buttonParentName;

        void Awake()
        {
            itemManager = FindObjectOfType<ItemManager>();
            buttonParentName = transform.parent.name;
        }

        public void SendEquipItem()
        {
            itemManager.EquipItem(buttonParentName);
        }

        public void SendSellItem()
        {
            itemManager.FindItemToSell(buttonParentName);
        }
    }
}
