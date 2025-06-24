using UnityEngine;

public class VidaMorcego : MonoBehaviour
{
    public int vidaMaxima = 100;
    private int vidaAtual;

    private Animator animator;
    private BatEnimie batScript;
    private SpriteRenderer spriteRenderer;
    private Collider2D colisor;

    private bool morto = false;

    void Start()
    {
        vidaAtual = vidaMaxima;
        animator = GetComponent<Animator>();
        batScript = GetComponent<BatEnimie>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        colisor = GetComponent<Collider2D>();
    }

    public void LevarDano(int quantidade)
    {
        if (morto) return;

        vidaAtual -= quantidade;
        Debug.Log($"{gameObject.name} levou {quantidade} de dano! Vida restante: {vidaAtual}");

        if (vidaAtual > 0)
        {
            animator.SetTrigger("Hit");
        }
        else
        {
            Morrer();
        }
    }

    private void Morrer()
    {
        morto = true;
        Debug.Log($"{gameObject.name} morreu!");

        if (batScript != null) batScript.enabled = false;
        if (colisor != null) colisor.enabled = false;

        animator.SetTrigger("Die");
        Destroy(gameObject, 1.5f);
    }
}
