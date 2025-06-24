using UnityEngine;

[RequireComponent(typeof(PlayerHealth), typeof(Rigidbody2D))]
public class PlayerDamageReceiver : MonoBehaviour
{
    private PlayerHealth playerHealth;
    private Rigidbody2D rb;

    [Header("Dano por Queda")]
    public float fallThreshold = -1f;         // Velocidade mínima para considerar dano por queda
    public float fallDamageMultiplier = 5f;   // Multiplicador de dano baseado na velocidade
    private float lastYVelocity;

    [HideInInspector] public bool estaInvulneravel = false;

    void Start()
    {
        playerHealth = GetComponent<PlayerHealth>();
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        // Salva a velocidade vertical do frame anterior
        lastYVelocity = rb.linearVelocity.y;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Dano por queda ao colidir com o chão
        if (collision.gameObject.CompareTag("Ground"))
        {
            if (lastYVelocity < fallThreshold)
            {
                int damage = Mathf.RoundToInt(Mathf.Abs(lastYVelocity) * fallDamageMultiplier);
                ApplyDamage(damage, "queda");
            }
        }

        // Aqui no futuro você pode adicionar outras verificações, como:
        // if (collision.gameObject.CompareTag("Armadilha")) { ... }
    }

    // Método genérico para aplicar dano
    public void ApplyDamage(int amount, string source)
    {
        if (estaInvulneravel)
        {
            Debug.Log("Jogador está invulnerável. Dano ignorado.");
            return;
        }

        Debug.Log($"Recebeu {amount} de dano por {source}.");
        playerHealth.TakeDamage(amount);
    }
}
