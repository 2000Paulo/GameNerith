using UnityEngine;

public class AtivadorDeLuminaria : MonoBehaviour
{
    public Rigidbody2D luminariaRb;

    private bool ativado = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!ativado && other.CompareTag("Player"))
        {
            ativado = true;

            if (luminariaRb != null)
            {
                Debug.Log("Luminária ativada!");
                luminariaRb.bodyType = RigidbodyType2D.Dynamic;
            }
        }
    }
}
