using UnityEngine;

public class Moeda : MonoBehaviour
{
    public AudioClip somColeta;

    private AudioSource audioSource;
    private SpriteRenderer spriteRenderer;
    private Collider2D col;

    private bool foiColetada = false; // ✅ Proteção contra múltiplas coletas

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (foiColetada) return; // ✅ Ignora se já foi coletada

        if (other.CompareTag("Player"))
        {
            foiColetada = true;
            Coletar();
        }
    }

    private void Coletar()
    {
        var contador = Object.FindFirstObjectByType<PontuacaoUI>();
        if (contador != null)
        {
            contador.AdicionarPontos(100);
        }

        if (somColeta != null)
        {
            audioSource.PlayOneShot(somColeta);
        }

        spriteRenderer.enabled = false;
        col.enabled = false;

        Destroy(gameObject, somColeta != null ? somColeta.length : 0.1f);
    }
}
