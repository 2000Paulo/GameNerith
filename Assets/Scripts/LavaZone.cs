using UnityEngine;

public class LavaZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerDamageReceiver damageReceiver = other.GetComponent<PlayerDamageReceiver>();

            if (damageReceiver != null)
            {
                // Aplica dano mortal
                int danoFatal = other.GetComponent<PlayerHealth>().maxHealth;
                damageReceiver.ApplyDamage(danoFatal, "lava");

                Debug.Log("Jogador morreu na lava!");
            }
        }
    }
}
