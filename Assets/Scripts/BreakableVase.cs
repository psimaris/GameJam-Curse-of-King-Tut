using UnityEngine;

public class BreakableVase : MonoBehaviour {
    public GameObject potionPrefab;        
    public GameObject breakEffectPrefab;   
    public Item potionItem;              
     private GameObject spawnedPotion;     
     public float pickupDistance = 3f;     


    public void Break() {
        if (breakEffectPrefab)
            Instantiate(breakEffectPrefab, transform.position, Quaternion.identity);

        if (potionPrefab) {
            spawnedPotion = Instantiate(potionPrefab, transform.position + Vector3.up * 0.5f, Quaternion.identity);
        }

        Destroy(gameObject);
    }

}