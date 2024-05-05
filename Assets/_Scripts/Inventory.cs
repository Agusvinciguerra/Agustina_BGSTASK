using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItemsSpace
{
    public class Inventory : MonoBehaviour
    {
        public float playerMoney = 10;
        public List<string> boughtItems = new List<string>();
        
        public void DebugInventory()
        {
            foreach (string item in boughtItems)
            {
                Debug.Log("Item in inventory: " + item);
            }
        }

        void AddToInventory()
        {

        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }
}