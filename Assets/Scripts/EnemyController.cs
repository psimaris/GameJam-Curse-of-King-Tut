using System.Collections;
using UnityEngine;
using UnityEngine.AI;


public class EnemyController : MonoBehaviour {
    public int maxHealth = 50;
    public float attackRange = 0.8f;
    public float attackCooldown = 1f;
    public int attackDamage = 10;
    public float speed = 2.0f;
    public GameObject lootPrefab;


    private int currentHealth;
    private float lastAttackTime;
    private Transform player;
    private Animator animator;
    private NavMeshAgent agent;
    private PlayerUI playerUI;

    public float chaseRange = 5f;  // Enemy won't chase until player is within this range
    private bool isChasing = false;
    private bool hasBeenAttacked = false;

    //Audio variables
    public AudioClip damageSound; // Assign in Inspector
    private AudioSource audioSource;
    public AudioClip deathSound;

    void Start() {
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
        player = GameObject.FindWithTag("Player")?.transform;
        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;
        playerUI = FindAnyObjectByType<PlayerUI>();
        audioSource = GetComponent<AudioSource>();

        if (playerUI == null) {
            Debug.LogError("PlayerUI script not found in the scene!");
        }

        // Prevent movement until hit
        agent.isStopped = true;
    }

    void Update() {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        // Start chasing only if player is within chase range or enemy has been attacked
        if ((distance <= chaseRange || hasBeenAttacked) && !isChasing) {
            isChasing = true;
            agent.isStopped = false;
        }

        if (!isChasing) {
            animator.SetBool("IsMoving", false);
            return;  // Exit early if not chasing
        }

        bool isPlayerInRange = distance <= attackRange;

        if (!isPlayerInRange) {
            agent.isStopped = false;
            agent.SetDestination(player.position);
            animator.SetBool("IsMoving", true);
        } 
        else {
            agent.isStopped = true;
            animator.SetBool("IsMoving", false);

            // Rotate toward the player continuously when in range
            Vector3 direction = (player.position - transform.position).normalized;
            direction.y = 0;
            if (direction != Vector3.zero) {
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
            }
        }

        // Attack when in range and cooldown is over
        if (isPlayerInRange && Time.time >= lastAttackTime + attackCooldown) {
            Attack();
        }
    }

    public void TakeDamage(int damage) {
        currentHealth -= damage;

        if (!hasBeenAttacked) {
            hasBeenAttacked = true;
            isChasing = true;         // Start chasing if attacked
            agent.isStopped = false; // Allow movement once hit
        }

        animator.SetTrigger("Hit");

        if (playerUI != null) {
            playerUI.SetEnemy(this);  // Show health bar after first hit
        }

        if (damageSound != null && audioSource != null) {
            audioSource.pitch = Random.Range(0.95f, 1.05f); // Optional: subtle variation
            audioSource.PlayOneShot(damageSound);
        }

        if (currentHealth <= 0) {
            Die();
        }
    }

    void Attack() {
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0;  // Prevent tilting up/down
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 1f); // instant face

        animator.SetTrigger("Attack");
        lastAttackTime = Time.time;
    }

    public void DealDamage() {
        
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        Debug.Log("Distance to player: " + distanceToPlayer);

        if (distanceToPlayer <= attackRange) {
            PlayerController playerController = player.GetComponent<PlayerController>();
            if (playerController != null) {
                // Deal damage to the player
                playerController.TakeDamage(attackDamage);
                Debug.Log("Enemy tried to damage player.");
            }
        }
    }

    void Die() {
        animator.SetTrigger("Die");

        if (deathSound != null && audioSource != null) {
            audioSource.PlayOneShot(deathSound);
        }

        // Disable collider and script after death animation plays
        GetComponent<Collider>().enabled = false;
        this.enabled = false;

        // Hide health bar
        if (playerUI != null) {
            playerUI.HideEnemyHealthBar();
        }

        if (lootPrefab != null) {
            Vector3 dropOffset = transform.forward * -1.0f + Vector3.up * 0.15f;  // Drop behind enemy and slightly above
            Instantiate(lootPrefab, transform.position + dropOffset, Quaternion.identity);
        }

        EnemySpawnManager spawnManager = FindAnyObjectByType<EnemySpawnManager>();
        if (spawnManager != null) {
            spawnManager.EnemyDefeated(this.gameObject);
        }

        if (gameObject.name == "Skeleton2") {
            StartCoroutine(SwitchCameraAndOpenDoorWithDelay(1.5f)); 
        }
    }

    private IEnumerator SwitchCameraAndOpenDoorWithDelay(float delay) {
        yield return new WaitForSeconds(delay);

        DoorController door = FindAnyObjectByType<DoorController>();
        CameraSwitcher cameraSwitcher = FindAnyObjectByType<CameraSwitcher>();

        if (cameraSwitcher != null) {
            cameraSwitcher.ShowDoorView();
        }

        if (door != null) {
            door.OpenDoor();
        }
    }

    public int getCurrentHealth() {
        return currentHealth;
    }

    
}