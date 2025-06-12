using UnityEngine;
using System.Collections;

public class FallingPlatform : MonoBehaviour
{
    public float tremorDuration = 1f; // Tempo que treme antes de cair
    public float tremorIntensity = 0.05f; // Intensidade do tremor
    public float fallDelayBetweenPieces = 0.2f; // Tempo entre cada pedaço cair

    private Vector3 originalPosition;
    private bool playerOnPlatform = false;

    void Start()
    {
        originalPosition = transform.position;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player") && !playerOnPlatform)
        {
            playerOnPlatform = true;
            StartCoroutine(HandleFalling());
        }
    }

    IEnumerator HandleFalling()
    {
        // Fase de tremor
        float timer = 0f;
        while (timer < tremorDuration)
        {
            Vector3 tremorOffset = new Vector3(
                Random.Range(-tremorIntensity, tremorIntensity),
                Random.Range(-tremorIntensity, tremorIntensity),
                0f
            );

            transform.position = originalPosition + tremorOffset;
            timer += Time.deltaTime;
            yield return null;
        }

        transform.position = originalPosition;

        // Desintegrar pedaço por pedaço
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform piece = transform.GetChild(i);
            piece.gameObject.SetActive(false); // "Desintegra" o pedaço
            yield return new WaitForSeconds(fallDelayBetweenPieces);
        }

        // Destroi o objeto principal depois
        Destroy(gameObject);
    }
}
