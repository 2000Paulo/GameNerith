using UnityEngine;

public class InimigoAtaqueHitbox : MonoBehaviour
{
    public int dano = 20;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Algo entrou na zona de ataque: " + other.name);

        if (other.CompareTag("Player"))
        {
            // Busca o script mesmo se estiver no pai do objeto colidido
            PlayerDamageReceiver receiver = other.GetComponentInParent<PlayerDamageReceiver>();
            if (receiver != null)
            {
                receiver.ApplyDamage(dano, "inimigo voador");
                Debug.Log("Jogador atingido pela zona de ataque!");
            }
            else
            {
                Debug.LogWarning("PlayerDamageReceiver não encontrado no Player ou seus pais.");
            }
        }
    }
}
