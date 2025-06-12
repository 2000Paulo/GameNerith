using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour
{
    [Header("Configurações de Vida")]
    public int maxHealth = 100;
    private int currentHealth;

    [Header("Eventos")]
    public UnityEvent<int, int> OnHealthChanged; // (vida atual, vida máxima)

    void Start()
    {
        currentHealth = maxHealth;
        Debug.Log("Vida inicial: " + currentHealth);
        OnHealthChanged?.Invoke(currentHealth, maxHealth); // Atualiza a UI no início
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        Debug.Log("Dano recebido: " + amount + " | Vida atual: " + currentHealth);
        OnHealthChanged?.Invoke(currentHealth, maxHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        Debug.Log("Curado em: " + amount + " | Vida atual: " + currentHealth);
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    private void Die()
    {
        Debug.Log("Jogador morreu!");
        // gameObject.SetActive(false);
    }

    public int GetHealth()
    {
        return currentHealth;
    }
}
