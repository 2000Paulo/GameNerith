using UnityEngine;

public class DanoPorLanca : MonoBehaviour
{
    public int dano = 15;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerDamageReceiver receiver = other.GetComponent<PlayerDamageReceiver>();
            if (receiver != null)
            {
                Debug.Log("Jogador atingido pela lança!");
                receiver.ApplyDamage(dano, "lança");
            }
        }
    }
}
