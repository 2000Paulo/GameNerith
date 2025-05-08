using UnityEngine;

public class VisaoInimigo : MonoBehaviour
{
    private EnemyPatrol inimigo;

    private void Start()
    {
        inimigo = GetComponentInParent<EnemyPatrol>();
        if (inimigo == null)
        {
            Debug.LogError("EnemyPatrol não encontrado no objeto pai!");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            inimigo.SetPlayerInSight(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            inimigo.SetPlayerInSight(false);
        }
    }
}
