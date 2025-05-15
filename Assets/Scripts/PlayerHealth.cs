using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Configurações de Vida")]
    public int maxHealth = 100;           // Vida máxima do jogador
    private int currentHealth;            // Vida atual do jogador

    void Start()
    {
        currentHealth = maxHealth;       // Inicializa a vida do jogador com a vida máxima
        Debug.Log("Vida inicial: " + currentHealth);
    }

    // Método para aplicar dano ao jogador
    public void TakeDamage(int amount)
    {
        currentHealth -= amount;         // Diminui a vida do jogador
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);   // Garante que a vida não fique abaixo de 0
        Debug.Log("Dano recebido: " + amount + " | Vida atual: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();  // Chama o método para morrer se a vida chegar a 0
        }
    }

    // Método para curar o jogador
    public void Heal(int amount)
    {
        currentHealth += amount;         // Aumenta a vida do jogador
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);   // Garante que a vida não ultrapasse o máximo
        Debug.Log("Curado em: " + amount + " | Vida atual: " + currentHealth);
    }

    // Método chamado quando o jogador morre
    private void Die()
    {
        Debug.Log("Jogador morreu!");
        // Aqui você pode adicionar ações como desativar o jogador ou reiniciar o nível
        // Exemplo:
        // gameObject.SetActive(false);
    }

    // Método para obter a vida atual do jogador
    public int GetHealth()
    {
        return currentHealth;
    }
}
