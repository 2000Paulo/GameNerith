using UnityEngine;

public class GuerreiroComVisao : MonoBehaviour
{
    [Header("Patrulha")]
    public float speed = 1f;
    public float waitTime = 2f;
    public int leftSteps = 2;
    public int rightSteps = 2;

    [Header("Visão")]
    public float raioDeVisao = 3f;
    public LayerMask camadaPlayer;

    [Header("Ataque")]
    public float intervaloDeAtaque = 1.5f;
    private float tempoDesdeUltimoAtaque = 0f;

    private Animator anim;
    private Vector2 startPosition;
    private Vector2 targetPosition;

    private bool movingLeft = true;
    private bool isWaiting = false;
    private float waitCounter = 0f;

    private void Start()
    {
        anim = GetComponent<Animator>();
        startPosition = transform.position;
        SetNextTarget();
    }

    private void Update()
    {
        // Detectar player dentro do raio
        Collider2D detectado = Physics2D.OverlapCircle(transform.position, raioDeVisao, camadaPlayer);
        bool playerDetectado = detectado != null;

        if (playerDetectado)
        {
            tempoDesdeUltimoAtaque += Time.deltaTime;

            if (tempoDesdeUltimoAtaque >= intervaloDeAtaque)
            {
                anim.SetTrigger("attack"); // Animação ativada com Trigger
                tempoDesdeUltimoAtaque = 0f;
            }

            anim.SetBool("isWalking", false);
            return;
        }
        else
        {
            anim.ResetTrigger("attack"); // reseta ataque se o player sair
            tempoDesdeUltimoAtaque = intervaloDeAtaque;
        }

        // ---------- Patrulha ----------
        if (isWaiting)
        {
            anim.SetBool("isWalking", false);
            waitCounter -= Time.deltaTime;

            if (waitCounter <= 0f)
            {
                isWaiting = false;
                movingLeft = !movingLeft;
                SetNextTarget();
            }
        }
        else
        {
            anim.SetBool("isWalking", true);

            Vector3 scale = transform.localScale;
            scale.x = targetPosition.x < transform.position.x ? -1 : 1;
            transform.localScale = scale;

            transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

            if (Vector2.Distance(transform.position, targetPosition) < 0.01f)
            {
                isWaiting = true;
                waitCounter = waitTime;
            }
        }
    }

    void SetNextTarget()
    {
        float direction = movingLeft ? -1 : 1;
        float steps = movingLeft ? leftSteps : rightSteps;
        targetPosition = startPosition + new Vector2(direction * steps, 0);
    }

    // Apenas para visualizar o raio de visão no editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, raioDeVisao);
    }
}
