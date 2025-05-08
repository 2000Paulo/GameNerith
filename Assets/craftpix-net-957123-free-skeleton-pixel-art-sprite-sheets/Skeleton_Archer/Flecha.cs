using UnityEngine;

public class Flecha : MonoBehaviour
{
    private void Start()
    {
        // Ativa o corpo quando a flecha for disparada
        GetComponent<Rigidbody2D>().simulated = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        EnemyPatrol inimigo = FindObjectOfType<EnemyPatrol>();
        if (inimigo != null)
        {
            inimigo.PermitirNovoTiro();
        }

        Destroy(gameObject); // ou desative se quiser reaproveitar
    }
}
