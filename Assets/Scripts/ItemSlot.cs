using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour {

    public Image icon;
    private Item item;
    public TextMeshProUGUI quantityText;

    public void AddItem(Item newItem) {
        item = newItem;
        icon.sprite = newItem.icon;
        icon.enabled = true;

        if (quantityText != null) {
            quantityText.text = item.quantity > 1 ? item.quantity.ToString() : "";
        }

        Button button = GetComponent<Button>();
        button.onClick.RemoveAllListeners();     // Important when reusing slots
        button.onClick.AddListener(UseItem);     // connects the click to UseItem()

        //Handle used non-consumables on slot creation
        if (!item.isConsumable && item.isUsed) {
            button.interactable = false;
            icon.color = new Color(1, 1, 1, 0.90f); // gray out
        } 
        else {
            button.interactable = true;
            icon.color = Color.white;
        }
    }

    public void UseItem() {
        Debug.Log("UseItem called on: " + item.itemName);

        if (item != null) {
            Inventory.instance.UseItem(item); 

            if (!item.isConsumable) {
                item.isUsed = true;

                // Disable button or grey out the slot
                Button btn = GetComponent<Button>();
                btn.interactable = false; 
                icon.color = new Color(1, 1, 1, 0.90f); 
            }
        }
    }

    public void DestroySlot() {
        Destroy(gameObject);
    }
}
