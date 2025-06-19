using UnityEngine;

public class Moeda : MonoBehaviour
{
    public AudioClip somColeta;

    private AudioSource audioSource;
    private SpriteRenderer spriteRenderer;
    private Collider2D col;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Coletar();
        }
    }

    private void Coletar()
    {
        // Toca som
        if (somColeta != null)
        {
            audioSource.PlayOneShot(somColeta);
        }

        // Desativa a parte visual e o colisor
        spriteRenderer.enabled = false;
        col.enabled = false;

        // Destroi o objeto após o som acabar
        Destroy(gameObject, somColeta.length);
    }
}
