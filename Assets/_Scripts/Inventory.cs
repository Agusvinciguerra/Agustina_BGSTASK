using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

namespace ItemsSpace
{
    public class Inventory : MonoBehaviour
    {
        [SerializeField] private GameObject inventory;
        [SerializeField] private GameObject[] customPart;
        [SerializeField] private TextMeshProUGUI itemDetails;
        [SerializeField] private TextMeshProUGUI panelText;
        [SerializeField] private TextMeshProUGUI moneyText;
        [SerializeField] private Button sellButton;
        [SerializeField] private Button equipButton;
        [SerializeField] private Sprite transparent;
        [SerializeField] private Sprite[] customSprites; 
        [SerializeField] private Transform[] inventorySlots;
        public float playerMoney = 10;
        public List<string> removedItems = new List<string>();
        public List<string> boughtItems = new List<string>();
        private List<string> equippedItems = new List<string>();

        private PlayerMovement playerMovement;
        private Shop shop;
        private FindButtonParent[] findButtonParent;
        private bool inventoryOpen = false;
        
        void Awake() // Assign and initialize variables
        {
            shop = FindObjectOfType<Shop>();
            playerMovement = FindObjectOfType<PlayerMovement>();
            moneyText.text = $"${playerMoney}";
        }

        void Update() // Check for tab input to open and close inventory panel
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                if (!inventoryOpen && !shop.isShop)
                {
                    inventoryOpen = true;
                    OpenInventory();
                }
                else
                {
                    inventoryOpen = false;
                    CloseInventory();
                }
            }
        }

        public void UpdateUI() // Update money amount in UI 
        {
            moneyText.text = $"${playerMoney}";
        }

        void OpenInventory() // Open and set up the inventory panel
        {
            inventory.SetActive(true);
            playerMovement.positionLocked = true;
            findButtonParent = FindObjectsOfType<FindButtonParent>();
            itemDetails.text = "Select\nitem";
            panelText.text = "This is your inventory. Select an item to inspect.";

            foreach (var slot in inventorySlots)
            {
                slot.gameObject.GetComponent<Image>().sprite = transparent;
            }

            for (int i = 0; i < boughtItems.Count; i++)
            {
                var itemName = boughtItems[i];
                var itemSprite = shop.itemObjects.FirstOrDefault(sprite => sprite.name == itemName) ?? transparent;
                inventorySlots[i].gameObject.GetComponent<Image>().sprite = itemSprite;
                inventorySlots[i].gameObject.transform.parent.name = itemSprite.name;
            }

            foreach (var buttonParent in findButtonParent)
            {
                buttonParent.StartProcess();
            }
        }

        public void SelectItem(string itemName) // Select an item to view details
        {
            
            if (boughtItems.Contains(itemName) && ItemData.instance.itemDictionary.TryGetValue(itemName, out var details))
            {
                itemDetails.text = $"{details.Item2}\n${details.Item1}";
                
        
                sellButton.onClick.RemoveAllListeners();
                sellButton.onClick.AddListener(() => SellItem(itemName));

                equipButton.onClick.RemoveAllListeners();
                equipButton.onClick.AddListener(() => EquipItems(itemName));
            }
        }

        public void SellItem(string itemName) // Sell an available item
        {
            if (!equippedItems.Contains(itemName))
            {
                var itemIndex = boughtItems.IndexOf(itemName);
                if (itemIndex != -1)
                {
                    removedItems.Add(itemName);
                    playerMoney += ItemData.instance.itemDictionary[itemName].Item1;
                    itemDetails.text = "Select\nitem";
                    inventorySlots[itemIndex].gameObject.GetComponent<Image>().sprite = transparent;
                }
                itemName = string.Empty;
                UpdateUI();
            } else 
            {
                panelText.text = "You cannot sell equipped items!";
            }
        }

        public void EquipItems(string itemName) // Equip item to the player
        {
            if (!equippedItems.Contains(itemName))
            {
                var itemIndex = boughtItems.IndexOf(itemName);
                if (itemIndex != -1)
                {
                    var itemSprite = FindSpriteByItemName(itemName, customSprites);
                    var itemType = ItemData.instance.itemDictionary[itemName].Item2;
                    var matchingObject = FindObjectByItemType(itemType, customPart);
                    matchingObject.GetComponent<SpriteRenderer>().sprite = itemSprite;

                    equippedItems.Add(itemName);
                    panelText.text = $"You equipped {itemType}!";
                    itemDetails.text = $"{itemType}\nEquipped";
                }
                itemName = string.Empty;
            } else
            {
                panelText.text = "You are already wearing this item!";
            }

        }

        GameObject FindObjectByItemType(string itemType, GameObject[] customArray) // Find the object that matches the item type
        {
            return customArray.FirstOrDefault(obj => obj.name == itemType);
        }

        Sprite FindSpriteByItemName(string itemName, Sprite[] spriteArray) // Find the sprite that matches the item name
        {
            return spriteArray.FirstOrDefault(spr => spr.name == itemName);
        }

        void CloseInventory() // Close the inventory panel
        {
            removedItems.ForEach(item => boughtItems.Remove(item));
            removedItems.Clear();
            inventory.SetActive(false);
            playerMovement.positionLocked = false;
        }
    }
}