using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject {
    public string itemName;
    public Sprite icon;
    public GameObject prefab; 

    public int healthBoost;
    public float speedBoost;
    public int strengthBoost;
    public int defenseBoost;

    public bool isConsumable; 

    [HideInInspector]
    public int quantity = 1;

    [HideInInspector]
    public bool isUsed = false;  

    public virtual void Use() {
        Debug.Log("Using " + name);
    }
}
