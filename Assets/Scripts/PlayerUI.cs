using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    [Header("Player UI")]
    public Slider playerHealthBar;
    public TMP_Text playerSpeedText;
    public TMP_Text playerStrengthText;
    public TMP_Text playerDefenseText;

    [Header("Enemy UI")]
    public Slider enemyHealthBar;                    
    public GameObject enemyHealthBarContainer;      
    private EnemyController currentEnemy;

    private PlayerController player;

    void Start() {
        player = FindFirstObjectByType<PlayerController>();

        if (player != null) {
            playerHealthBar.maxValue = player.maxHealth;
            playerHealthBar.value = player.getCurrentHealth();
            UpdateStats();
        }

        if (enemyHealthBarContainer != null) {
        enemyHealthBarContainer.SetActive(false);  // Hide initially
        }

        if (enemyHealthBarContainer != null) {
            enemyHealthBarContainer.SetActive(false);
        }
    }

    void Update() {
        if (player != null)
        {
            playerHealthBar.value = player.getCurrentHealth();
            UpdateStats();
        }

        if (currentEnemy != null) {
            float distance = Vector3.Distance(player.transform.position, currentEnemy.transform.position);

            // Check if the player is close enough
            if (distance <= 1.5f) {
                enemyHealthBar.value = currentEnemy.getCurrentHealth();
            }
            else {
                // Hide health bar if the player is out of range
                if (enemyHealthBarContainer != null) {
                    enemyHealthBarContainer.SetActive(false);
                }
            }
        }
    }


    void UpdateStats() {
        playerSpeedText.text = "Speed: " + (player.moveSpeed + player.speed);
        playerStrengthText.text = "Strength: " + player.strength;
        playerDefenseText.text = "Defense: " + player.defense;
    }

    public void SetEnemy(EnemyController enemy) {
        currentEnemy = enemy;

        if (enemyHealthBar != null) {
            enemyHealthBar.maxValue = enemy.maxHealth;
            enemyHealthBar.value = enemy.getCurrentHealth();
        }

        if (enemyHealthBarContainer != null) {
            enemyHealthBarContainer.SetActive(true); // Show health bar only if within range
        }
        else {
            enemyHealthBar.gameObject.SetActive(true);  
        }
    }

    public void UpdateEnemyHealth(int currentHealth) {
        if (enemyHealthBar != null) {
            enemyHealthBar.value = currentHealth;
        }
    }

    public void HideEnemyHealthBar() {
        if (enemyHealthBarContainer != null) {
            enemyHealthBarContainer.SetActive(false);
        }
        else {
            enemyHealthBar.gameObject.SetActive(false);  // fallback
        }
    }

    public void ShowEnemyHealthBar() {
        if (enemyHealthBarContainer != null) {
            enemyHealthBarContainer.SetActive(true);
        }
        else {
            enemyHealthBar.gameObject.SetActive(true);  // fallback
        }
    }

    public EnemyController getCurrentEnemy() {
        return currentEnemy;
    }
}