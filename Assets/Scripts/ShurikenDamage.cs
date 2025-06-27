using UnityEngine;

public class ShurikenDamage : MonoBehaviour
{
    public int dano = 1;
    private bool jaCausouDano = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !jaCausouDano)
        {
            PlayerDamageReceiver receiver = collision.GetComponent<PlayerDamageReceiver>();
            if (receiver != null)
            {
                receiver.ApplyDamage(dano, "shuriken");
                jaCausouDano = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            jaCausouDano = false; // permite causar dano de novo se encostar depois
        }
    }
}
