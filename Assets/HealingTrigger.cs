using UnityEngine;

public class HealingTrigger : MonoBehaviour
{
    public int healAmount = 20;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealer healer = other.GetComponent<PlayerHealer>();
            if (healer != null)
            {
                healer.HealPlayer(healAmount, "Vaso de Cura");
            }

            Destroy(gameObject); // Remove o objeto após a cura
        }
    }
}
