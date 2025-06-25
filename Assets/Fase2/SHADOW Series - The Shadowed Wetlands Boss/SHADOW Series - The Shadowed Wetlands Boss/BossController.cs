using UnityEngine;

public class BossController : MonoBehaviour
{
    public float velocidade = 2f;
    public float distanciaAtaque = 2f;
    public float intervaloAtaque = 2f;

    private Animator animator;
    private Transform jogador;
    private bool podeAtacar = true;
    private bool estaAndando = false;
    private bool jogadorDetectado = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (!jogadorDetectado || jogador == null) return;

        float distancia = Vector2.Distance(transform.position, jogador.position);

        if (distancia > distanciaAtaque)
        {
            if (!estaAndando)
            {
                animator.SetBool("IsWalking", true);
                estaAndando = true;
            }

            transform.position = Vector2.MoveTowards(transform.position, jogador.position, velocidade * Time.deltaTime);
        }
        else
        {
            if (podeAtacar)
            {
                estaAndando = false;
                animator.SetBool("IsWalking", false);
                AtacarAleatoriamente();
            }
        }
    }

    private void AtacarAleatoriamente()
    {
        podeAtacar = false;

        int tipo = Random.Range(0, 2);
        if (tipo == 0)
            animator.SetTrigger("Attack");
        else
            animator.SetTrigger("Attack2"); // "Attach"

        Invoke(nameof(HabilitarAtaque), intervaloAtaque);
    }

    private void HabilitarAtaque()
    {
        podeAtacar = true;
    }

    // 🎯 Detecta entrada na zona de visão
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            jogador = other.transform;
            jogadorDetectado = true;
        }
    }

    // ❌ Para de seguir se o jogador sair da visão (opcional)
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            jogadorDetectado = false;
            animator.SetBool("IsWalking", false);
        }
    }
}
