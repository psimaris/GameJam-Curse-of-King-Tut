using UnityEngine;

public class PlayerAttackCollider : MonoBehaviour {
    private PlayerController player;

    void Start() {
        player = GetComponentInParent<PlayerController>();
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Enemy")) {
            EnemyController enemy = other.GetComponent<EnemyController>();
            if (enemy != null && player != null) {
                int totalDamage = Mathf.Max(player.strength, 1); // Use player strength
                enemy.TakeDamage(totalDamage);
                Debug.Log("Hit enemy with " + totalDamage + " damage");
            }
        }

        if (other.CompareTag("Breakable")) {
            BreakableVase breakable = other.GetComponent<BreakableVase>();
            if (breakable != null) {
                breakable.Break();
                Debug.Log("Broke breakable object!");
            }
        }
    }
}
