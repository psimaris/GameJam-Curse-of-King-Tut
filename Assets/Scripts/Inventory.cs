using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class Inventory : MonoBehaviour {

    #region singleton

    public static Inventory instance;

    private void Awake() {
        if (instance == null) {
            instance = this;
        }
    }

    #endregion

    public delegate void OnItemChange();

    public OnItemChange onItemChange = delegate {};

    public List<Item> inventoryItemList = new List<Item>();
    private HashSet<Item> equippedItems = new HashSet<Item>(); // Track equipped items

    public void AddItem(Item item) {

        // If consumable, try to stack
        if (item.isConsumable) {
            foreach (Item existingItem in inventoryItemList) {
                if (existingItem.itemName == item.itemName) {
                    existingItem.quantity++;
                    onItemChange.Invoke();
                    return;
                }
            }

            // If not found, clone and add
            Item clone = CloneItem(item);
            clone.quantity = 1;
            inventoryItemList.Add(clone);
        }
        else {
            // If non-consumable or not found in stack
            if (!inventoryItemList.Contains(item)) {
                inventoryItemList.Add(item);
            }
        }
        onItemChange.Invoke();
    }

    public Item CloneItem(Item original) {
        Item clone = Instantiate(original); // makes a copy in memory
        clone.quantity = 1;
        return clone;
    }

    public void UseItem(Item item) {
        PlayerController player = FindAnyObjectByType<PlayerController>();
        if (player == null) return;

        if (item.isConsumable) {
            if (player.getCurrentHealth() >= player.maxHealth) {
                Debug.Log("Health is full. Cannot use potion.");
                return; 
            }

            player.Heal(item.healthBoost);
            item.quantity--;

            if (item.quantity <= 0) {
                inventoryItemList.Remove(item); 
            }
        } 
        else {
            if (!equippedItems.Contains(item)) {
                player.speed += item.speedBoost;
                player.strength += item.strengthBoost;
                player.defense += item.defenseBoost;
                equippedItems.Add(item);
            }
        }

        onItemChange.Invoke(); // Refresh UI visuals
    }
}
