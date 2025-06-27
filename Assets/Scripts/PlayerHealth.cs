using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour
{
    [Header("Configurações de Vida")]
    public int maxHealth = 100;
    public int currentHealth;

    [Header("Eventos")]
    public UnityEvent<int, int> OnHealthChanged; // (vida atual, vida máxima)

    void Start()
    {
        // Sincroniza com os valores do GameManager
        maxHealth = GameManager.instance.vidaMaxima;
        currentHealth = GameManager.instance.vidaAtual > 0 ? GameManager.instance.vidaAtual : maxHealth;

        Debug.Log("Vida inicial: " + currentHealth);
        OnHealthChanged?.Invoke(currentHealth, maxHealth); // Atualiza a UI no início
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        // Atualiza no GameManager
        GameManager.instance.vidaAtual = currentHealth;

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

        // Atualiza no GameManager
        GameManager.instance.vidaAtual = currentHealth;

        Debug.Log("Curado em: " + amount + " | Vida atual: " + currentHealth);
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    private void Die()
    {
        Debug.Log("Jogador morreu!");
        Invoke("ChamarGameOver", 1f);
        // gameObject.SetActive(false); // você pode ativar/desativar aqui se quiser
    }

    private void ChamarGameOver()
    {
        GameManager.instance.GameOver();
    }

    public int GetHealth()
    {
        return currentHealth;
    }
}
