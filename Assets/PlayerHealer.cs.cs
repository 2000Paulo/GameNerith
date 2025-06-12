using UnityEngine;

[RequireComponent(typeof(PlayerHealth))]
public class PlayerHealer : MonoBehaviour
{
    private PlayerHealth playerHealth;

    void Start()
    {
        playerHealth = GetComponent<PlayerHealth>();
    }

    // Método central para curar
    public void HealPlayer(int amount, string source)
    {
        Debug.Log($"Cura recebida de {source}: {amount}");
        playerHealth.Heal(amount);
    }
}
