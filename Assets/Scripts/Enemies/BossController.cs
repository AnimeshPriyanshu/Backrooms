using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BossController : MonoBehaviour
{
    [Header("Settings")]
    public int maxHealth = 500;
    private int currentHealth;
    
    public float moveSpeed = 6f;
    public int attackDamage = 40;

    private Transform playerTransform;
    private Rigidbody2D rb;

    private void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
    }

    private void FixedUpdate()
    {
        if (playerTransform == null || (GameManager.Instance != null && GameManager.Instance.isGameOver)) return;

        // Boss relentlessly charges the player
        Vector2 direction = (playerTransform.position - transform.position).normalized;
        rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("Boss Health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth health = collision.gameObject.GetComponent<PlayerHealth>();
            if (health != null)
            {
                health.TakeDamage(attackDamage);
            }
        }
    }

    private void Die()
    {
        Debug.Log("Boss Defeated! You escaped the Backrooms.");
        Destroy(gameObject);
        
        if (GameManager.Instance != null)
        {
            GameManager.Instance.NextLevel(); // Triggers the win condition
        }
    }
}
