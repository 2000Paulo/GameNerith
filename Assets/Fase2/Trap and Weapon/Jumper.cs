using UnityEngine;

public class Jumper : MonoBehaviour
{
    public float forcaDoPulo = 15f;
    private Animator animator;

    private bool jogadorNoJumper = false;
    private Rigidbody2D rbDoJogador;
    private Animator animDoJogador;
    private bool pulouNoJumper = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (jogadorNoJumper && rbDoJogador != null && animDoJogador != null)
        {
            if ((animDoJogador.GetBool("pulando") || animDoJogador.GetCurrentAnimatorStateInfo(0).IsTag("Jump")) && !pulouNoJumper)
            {
                Debug.Log("Jumper ativado com pulo do jogador");

                animator.SetTrigger("Ativar");

                rbDoJogador.linearVelocity = new Vector2(rbDoJogador.linearVelocity.x, 0f);
                rbDoJogador.AddForce(Vector2.up * forcaDoPulo, ForceMode2D.Impulse);

                // 👉 Ativa animação de pulo no jogador
                animDoJogador.SetTrigger("podePular");
                animDoJogador.SetBool("pulando", true);
                animDoJogador.SetBool("noChao", false);

                pulouNoJumper = true;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            jogadorNoJumper = true;
            rbDoJogador = other.GetComponent<Rigidbody2D>();
            animDoJogador = other.GetComponent<Animator>();
            pulouNoJumper = false;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            jogadorNoJumper = false;
            rbDoJogador = null;
            animDoJogador = null;
            pulouNoJumper = false;
            animator.ResetTrigger("Ativar");
        }
    }
}
