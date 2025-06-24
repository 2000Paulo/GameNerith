using UnityEngine;

public class BatAttackHitbox : MonoBehaviour
{
    public int dano = 20;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("BatAttackHitbox colidiu com: " + other.name); // ← Verifica se houve colisão

        if (other.CompareTag("Player"))
        {
            Debug.Log("Colidiu com o Player!");

            PlayerDamageReceiver receiver = other.GetComponent<PlayerDamageReceiver>();
            if (receiver != null)
            {
                Debug.Log("PlayerDamageReceiver encontrado. Aplicando dano.");
                receiver.ApplyDamage(dano, "ataque do morcego");
            }
            else
            {
                Debug.LogError("PlayerDamageReceiver NÃO encontrado no player!");
            }
        }
    }
}
