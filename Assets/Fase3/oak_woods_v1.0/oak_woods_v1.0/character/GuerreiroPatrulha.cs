using UnityEngine;

public class GuerreiroPatrulha : MonoBehaviour
{
    public Transform pontoA;
    public Transform pontoB;
    public float velocidade = 2f;
    public float distanciaVisao = 5f;
    public float distanciaAtaque = 1.2f;
    public float tempoEntreAtaques = 2f;
    public Transform player;

    private Animator animator;
    private Vector3 destinoAtual;
    private bool seguindoPlayer = false;
    private bool podeAtacar = true;
    private Vector3 ultimaPosicao;

    // NOVO: referência ao MeleeAttackController
    private MeleeAttackController ataqueMelee;

    void Start()
    {
        animator = GetComponent<Animator>();
        destinoAtual = pontoB.position;
        ultimaPosicao = transform.position;

        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player")?.transform;

        // NOVO: pega o MeleeAttackController no mesmo objeto
        ataqueMelee = GetComponent<MeleeAttackController>();
    }

    void Update()
    {
        if (player == null) return;

        float distanciaDoPlayer = Vector2.Distance(transform.position, player.position);

        if (distanciaDoPlayer <= distanciaVisao)
        {
            seguindoPlayer = true;

            if (distanciaDoPlayer <= distanciaAtaque)
            {
                // Ataca o player se puder
                if (podeAtacar)
                {
                    animator.SetTrigger("Attack");

                    // NOVO: ativa o ataque melee
                    if (ataqueMelee != null)
                        ataqueMelee.TriggerAttack();

                    podeAtacar = false;
                    Invoke(nameof(ResetarAtaque), tempoEntreAtaques);
                }

                // Para o movimento durante ataque
                animator.SetFloat("Speed", 0);
                return;
            }

            // Persegue o player se não estiver na distância de ataque
            MoverPara(player.position);
        }
        else
        {
            seguindoPlayer = false;
            Patrulhar();
        }

        // Animação de movimento
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

        if (Vector2.Distance(transform.position, destinoAtual) < 0.1f)
        {
            destinoAtual = destinoAtual == pontoA.position ? pontoB.position : pontoA.position;
        }
    }

    void MoverPara(Vector3 destino)
    {
        Vector3 direcao = (destino - transform.position).normalized;
        transform.position += direcao * velocidade * Time.deltaTime;

        // Flip
        if (direcao.x > 0)
            transform.localScale = new Vector3(1, 1, 1);
        else if (direcao.x < 0)
            transform.localScale = new Vector3(-1, 1, 1);
    }
}
