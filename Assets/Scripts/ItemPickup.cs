using UnityEngine;

public class ItemPickup : MonoBehaviour {

    public Item itemToGive;
    private bool pickedUp = false;

    private void OnTriggerEnter(Collider other) {

        if (pickedUp) return; 

        if (other.CompareTag("Player")) {
            pickedUp = true; 
            Inventory.instance.AddItem(Inventory.instance.CloneItem(itemToGive));
            Destroy(gameObject);
        }
    }
}