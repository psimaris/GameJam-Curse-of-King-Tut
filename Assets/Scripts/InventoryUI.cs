using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryUI : MonoBehaviour {

    private bool inventoryOpen = false;

    public bool InventoryOpen => inventoryOpen;
    public GameObject inventoryParent;
    public GameObject inventoryTab;
    // public GameObject craftingTab;

    private List<ItemSlot> itemSlotList = new List<ItemSlot>();
    public GameObject itemSlotPrefab;
    public Transform inventoryItemTransform;

    private void Start() {
        Inventory.instance.onItemChange += UpdateInventoryUI;
        UpdateInventoryUI();
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.I)) {
            if (inventoryOpen) {
                CloseInventory();
            }
            else {
                OpenInventory();
            }
        }
    }

    private void UpdateInventoryUI() {
        // Clear old item slots
        foreach (var slot in itemSlotList) {
            Destroy(slot.gameObject);
        }
        itemSlotList.Clear();

        // Re-add all current items from the inventory
        foreach (var item in Inventory.instance.inventoryItemList) {
            GameObject slotGO = Instantiate(itemSlotPrefab, inventoryItemTransform);
            ItemSlot slot = slotGO.GetComponent<ItemSlot>();
            slot.AddItem(item);
            itemSlotList.Add(slot);

             // Hook up the button
            Button btn = slotGO.GetComponent<Button>();
            if (btn != null) {
                btn.onClick.AddListener(slot.UseItem);
            }
        }
    }

    private void AddItemSlots(int currentItemCount) {
        int amount = currentItemCount - itemSlotList.Count;

        for (int i = 0; i < amount; ++i) {
            GameObject GO = Instantiate(itemSlotPrefab, inventoryItemTransform);
            ItemSlot newSlot = GO.GetComponent<ItemSlot>();
            itemSlotList.Add(newSlot);
        }
    }

    private void OpenInventory() {
        inventoryOpen = true;
        inventoryParent.SetActive(true);
    }

    private void CloseInventory() {
        inventoryOpen = false;
        inventoryParent.SetActive(false);
    }

    // public void OnCraftingTabClicked() {
    //     craftingTab.SetActive(true);
    //     inventoryTab.SetActive(false);
    // }

    // public void OnInventoryTabClicked() {
    //     craftingTab.SetActive(false);
    //     inventoryTab.SetActive(true);
    // }
}