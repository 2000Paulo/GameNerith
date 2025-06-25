using UnityEngine;

public class GuerreiroPatrulha : MonoBehaviour
{
    public Transform pontoA;
    public Transform pontoB;
    public float velocidade = 2f;
    public float distanciaAtaque = 1.2f;
    public float tempoEntreAtaques = 2f;
    public Transform player;

    private Animator animator;
    private Vector3 destinoAtual;
    private bool seguindoPlayer = false;
    private bool podeAtacar = true;
    private Vector3 ultimaPosicao;

    private MeleeAttackController ataqueMelee;
    private float distanciaVisao;

    void Start()
    {
        animator = GetComponent<Animator>();
        destinoAtual = pontoB.position;
        ultimaPosicao = transform.position;

        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player")?.transform;

        ataqueMelee = GetComponent<MeleeAttackController>();
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;

        // Calcula a distância de visão com base nos pontos A e B
        distanciaVisao = Vector2.Distance(pontoA.position, pontoB.position);
    }

    void Update()
    {
        if (player == null) return;

        float distanciaDoPlayer = Vector2.Distance(transform.position, player.position);

        // Raycast: vê o player apenas se não houver parede no caminho E dentro da faixa
        bool temVisaoLivre = false;

        RaycastHit2D hit = Physics2D.Raycast(
            transform.position,
            (player.position - transform.position).normalized,
            distanciaVisao,
            LayerMask.GetMask("Ground", "Player")
        );

        if (hit.collider != null && hit.collider.CompareTag("Player"))
        {
            temVisaoLivre = true;
        }

        if (distanciaDoPlayer <= distanciaVisao && temVisaoLivre)
        {
            seguindoPlayer = true;

            if (distanciaDoPlayer <= distanciaAtaque)
            {
                if (podeAtacar)
                {
                    animator.SetTrigger("Attack");

                    if (ataqueMelee != null)
                        ataqueMelee.TriggerAttack();

                    podeAtacar = false;
                    Invoke(nameof(ResetarAtaque), tempoEntreAtaques);
                }

                animator.SetFloat("Speed", 0);
                return;
            }

            MoverPara(player.position);
        }
        else
        {
            seguindoPlayer = false;
            Patrulhar();
        }

        float velocidadeFrame = (transform.position - ultimaPosicao).magnitude / Time.deltaTime;
        animator.SetFloat("Speed", velocidadeFrame);
        ultimaPosicao = transform.position;
    }

    void ResetarAtaque()
    {
        podeAtacar = true;
    }

    void Patrulhar()
    {
        MoverPara(destinoAtual);

        Vector2 pos2D = new Vector2(transform.position.x, transform.position.y);
        Vector2 destino2D = new Vector2(destinoAtual.x, destinoAtual.y);

        if (Vector2.Distance(pos2D, destino2D) < 0.1f)
        {
            if (Vector2.Distance(destino2D, pontoB.position) < 0.1f)
                destinoAtual = pontoA.position;
            else
                destinoAtual = pontoB.position;
        }
    }

    void MoverPara(Vector3 destino)
    {
        Vector3 direcao = (destino - transform.position).normalized;
        transform.position += direcao * velocidade * Time.deltaTime;

        if (direcao.x > 0)
            transform.localScale = new Vector3(1, 1, 1);
        else if (direcao.x < 0)
            transform.localScale = new Vector3(-1, 1, 1);
    }
}
