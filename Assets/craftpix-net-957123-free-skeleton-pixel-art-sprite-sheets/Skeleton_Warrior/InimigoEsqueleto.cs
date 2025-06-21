using UnityEngine;
using System.Collections;

public class InimigoEsqueleto : MonoBehaviour
{
    [Header("Vida e Dano")]
    public int vidaMaxima = 100;
    public int vidaAtual;
    public CanvasBarraDeVida canvasBarraDeVida;

    [Header("Patrulha")]
    public float speed = 1f;
    public float waitTime = 2f;
    public int leftSteps = 2;
    public int rightSteps = 2;

    [Header("Combate")]
    public float raioDeVisao = 3f;
    public LayerMask camadaPlayer;
    public float intervaloDeAtaque = 1.5f;

    [Header("Ataque Esqueleto")]
    public int danoCausadoPeloEsqueleto = 10;
    public LayerMask layerAlvoDano;
    public Transform pontoDeAtaque;
    public float raioDeDanoAtaque = 0.5f;

    [Header("Sons")]
    public AudioSource audioSourceAndar;
    public AudioSource audioSourceAcoes;
    public AudioClip somAndar;
    public AudioClip somAtaque;
    public AudioClip somMorte;

    private Animator anim;
    private Rigidbody2D rb;
    private Vector2 startPosition;
    private Vector2 targetPosition;

    private bool movingLeft = true;
    private bool isWaiting = false;
    private bool isDead = false;
    private float waitCounter = 0f;
    private float tempoDesdeUltimoAtaque = 0f;
    private float invincibilityTimer = 0f;
    private float invincibilityDuration = 0.5f;

    private void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        startPosition = transform.position;
        vidaAtual = vidaMaxima;
        SetNextTarget();
        tempoDesdeUltimoAtaque = intervaloDeAtaque;

        // Inicialização do pontoDeAtaque
        if (pontoDeAtaque == null)
        {
            GameObject novoPonto = new GameObject("PontoDeAtaque");
            novoPonto.transform.parent = this.transform;
            novoPonto.transform.localPosition = Vector2.zero;
            pontoDeAtaque = novoPonto.transform;
        }

        // Configuração dos AudioSources
        if (audioSourceAndar != null)
        {
            audioSourceAndar.loop = true;
            audioSourceAndar.playOnAwake = false;
        }
        if (audioSourceAcoes != null)
        {
            audioSourceAcoes.loop = false;
            audioSourceAcoes.playOnAwake = false;
        }
    }

    private void Update()
    {
        if (isDead) return;

        if (invincibilityTimer > 0)
        {
            invincibilityTimer -= Time.deltaTime;
        }

        Collider2D playerDetectado = Physics2D.OverlapCircle(transform.position, raioDeVisao, camadaPlayer);

        if (playerDetectado != null)
        {   
            Debug.Log("Esqueleto detectou o player: " + playerDetectado.name);
            ModoCombate(playerDetectado.transform);
        }
        else
        {
            ModoPatrulha();
        }
    }

    private void ModoCombate(Transform playerTransform)
    {
        anim.SetBool("isWalking", false);

        // Parar som de andar se estiver tocando
        if (audioSourceAndar != null && audioSourceAndar.isPlaying)
        {
            audioSourceAndar.Stop();
        }

        Vector3 scale = transform.localScale;
        scale.x = playerTransform.position.x < transform.position.x ? -1 : 1;
        transform.localScale = scale;

        tempoDesdeUltimoAtaque += Time.deltaTime;
        if (tempoDesdeUltimoAtaque >= intervaloDeAtaque)
        {
            tempoDesdeUltimoAtaque = 0f;
            anim.SetTrigger("attack"); 
        }
    }

    private void ModoPatrulha()
    {
        if(invincibilityTimer > 0) 
        {
            anim.SetBool("isWalking", false);
            // Parar som de andar se estiver tocando
            if (audioSourceAndar != null && audioSourceAndar.isPlaying)
            {
                audioSourceAndar.Stop();
            }
            return;
        }

        anim.ResetTrigger("attack");

        if (isWaiting)
        {
            anim.SetBool("isWalking", false);
            // Parar som de andar se estiver tocando
            if (audioSourceAndar != null && audioSourceAndar.isPlaying)
            {
                audioSourceAndar.Stop();
            }
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
            // Tocar som de andar se não estiver tocando
            if (audioSourceAndar != null && somAndar != null && !audioSourceAndar.isPlaying)
            {
                audioSourceAndar.clip = somAndar;
                audioSourceAndar.Play();
            }
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
    
    void OnTriggerEnter2D(Collider2D colliderDoAtaque)
    {
        if (colliderDoAtaque.gameObject.layer == LayerMask.NameToLayer("PlayerAttack") && invincibilityTimer <= 0 && !isDead)
        {
            invincibilityTimer = invincibilityDuration;
            vidaAtual -= 10;
            canvasBarraDeVida.AtualizaVida(vidaAtual, vidaMaxima);
            anim.SetTrigger("Hurt");

            if (vidaAtual <= 0)
            {
                Morrer();
            }
        }
    }

    public void AplicarDanoAtaque()
    {
        // Som de ataque
        if (audioSourceAcoes != null && somAtaque != null)
        {
            audioSourceAcoes.PlayOneShot(somAtaque);
        }

        Collider2D[] alvos = Physics2D.OverlapCircleAll(pontoDeAtaque.position, raioDeDanoAtaque, layerAlvoDano);
        foreach (Collider2D alvo in alvos)
        {
            PlayerDamageReceiver damageReceiver = alvo.GetComponent<PlayerDamageReceiver>();
            if (damageReceiver != null)
            {
                damageReceiver.ApplyDamage(danoCausadoPeloEsqueleto, "esqueleto");
                Debug.Log("Dano do Esqueleto aplicado!");
            }
        }
    }

    void Morrer()
    {
        isDead = true;
        anim.SetTrigger("Morte");

        // Som de morte
        if (audioSourceAcoes != null && somMorte != null)
        {
            audioSourceAcoes.PlayOneShot(somMorte);
        }

        this.enabled = false;
        GetComponent<Collider2D>().enabled = false;
        if(rb != null)
        {
            rb.simulated = false;
        }
        // Destroi após o som de morte ou 2 segundos (o que for maior)
        float tempoDestruir = (somMorte != null && somMorte.length > 2f) ? somMorte.length : 2f;
        Destroy(gameObject, tempoDestruir);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, raioDeVisao);

        // Gizmo para área de dano do ataque
        if (pontoDeAtaque != null)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(pontoDeAtaque.position, raioDeDanoAtaque);
        }
    }
}