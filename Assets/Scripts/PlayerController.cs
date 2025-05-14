using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 2.0f;
    public float speed;
    public int strength;
    public int defense;

    public int maxHealth = 100;
    private int currentHealth;

    private Animator animator;
    private CharacterController controller;

    // Attack-related variables
    private bool isAttacking = false;
    public GameObject attackCollider;  
    // Cooldown variables
    public float baseAttackCooldown = 1f;  
    private float lastAttackTime = -999f;    

    //Audio variables
    public AudioClip attackSound;
    private AudioSource audioSource;

    void Start() {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        currentHealth = maxHealth;
        speed = 0;
        strength = 2;
        defense = 2;

        // Get the attack collider (assigned in the inspector)
        attackCollider.SetActive(false);  // Ensure it's initially disabled
        audioSource = GetComponent<AudioSource>();
    }

    void Update() {
        // Get input for horizontal (left/right) and vertical (up/down) movements
        float horizontal = Input.GetAxis("Horizontal"); // Left/Right
        float vertical = Input.GetAxis("Vertical");     // Up/Down

        // Move the character based on horizontal and vertical input
        Vector3 moveDir = new Vector3(horizontal, 0, vertical);
        float totalSpeed = moveSpeed * (1f + speed * 0.1f);
        

        controller.Move(moveDir * totalSpeed * Time.deltaTime);

        // Rotate the character based on horizontal input (left/right)
        if (horizontal != 0) {
            // Rotate to face left or right based on horizontal movement
            float rotationY = horizontal > 0 ? 90f : -90f; // Right (90°) or Left (-90°)
            transform.rotation = Quaternion.Euler(0f, rotationY, 0f);
        }
        
        // When moving forward or backward, keep the player facing their back/front
        else if (vertical != 0) {
            // Rotate to face the front or back based on vertical movement
            float rotationY = vertical < 0 ? 180f : 0f; // Forward (back - 180°) or Backward (front - 0°)
            transform.rotation = Quaternion.Euler(0f, rotationY, 0f);
        }

        // Update the animator parameters to drive the blend tree for proper animation transitions
        animator.SetFloat("Horizontal", horizontal);
        animator.SetFloat("Vertical", vertical);

         // Trigger attack
         if (Input.GetKeyDown(KeyCode.Space) && CanAttack()) {
            PerformAttack();
        }

         // Defend (hold to block)
        bool isDefending = Input.GetKey(KeyCode.LeftShift);
        animator.SetBool("IsDefending", isDefending);
    }

    bool CanAttack() {
        float cooldown = Mathf.Max(0.15f, baseAttackCooldown / Mathf.Max(speed, 1f)); // Avoid divide by zero
        return Time.time >= lastAttackTime + cooldown;
    }

    void PerformAttack() {
        lastAttackTime = Time.time;
        isAttacking = true;
        animator.SetTrigger("Attack");
        EnableAttackCollider(true);
    }

    // Enable the player's attack collider
    void EnableAttackCollider(bool enable) {
        if (attackCollider != null) {
            attackCollider.SetActive(enable);  // Enable the collider during the attack
            if (!enable) {
                isAttacking = false;  // Reset the attack flag once the collider is disabled
            }
        }
    }

     // Disable the attack collider (called by the animation event)
    public void DisableAttackCollider() {
        EnableAttackCollider(false);
    }

    public void TakeDamage(int damage) {
        int finalDamage = Mathf.Max(damage - defense, 1); 
        currentHealth -= finalDamage;
        animator.SetTrigger("Hit");

        if (currentHealth <= 0) {
            Die();
        }
    }

    public void Heal(int amount) {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
    }

    void Die() {
        animator.SetTrigger("Die");
        // Disable movement, combat, etc.
        this.enabled = false;
    }

    public int getCurrentHealth() {
        return currentHealth;
    }

    public void PlayAttackSound() {
        if (attackSound != null && audioSource != null) {
            audioSource.PlayOneShot(attackSound);
        }
    }
}