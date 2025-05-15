using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class FallDamage : MonoBehaviour
{
    public float fallThreshold = -1f;      // Velocidade para causar dano
    public float damageMultiplier = 5f;     // Multiplicador de dano

    private Rigidbody2D rb;
    private float lastYVelocity = 0f;
    private PlayerHealth playerHealth;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerHealth = GetComponent<PlayerHealth>();

        if (playerHealth == null)
        {
            Debug.LogError("FallDamage: PlayerHealth não encontrado no GameObject.");
        }
    }

    void FixedUpdate()
    {
        lastYVelocity = rb.linearVelocity.y;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            if (lastYVelocity < fallThreshold)
            {
                int damage = Mathf.RoundToInt(Mathf.Abs(lastYVelocity) * damageMultiplier);
                playerHealth.TakeDamage(damage);
                Debug.Log("Dano por queda: " + damage);
            }
        }
    }
}
