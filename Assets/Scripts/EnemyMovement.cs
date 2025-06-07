using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour
{
    [Header("Som")]
    public AudioClip shootSound;
    private AudioSource audioSource;

    private float walkCooldown = 0f;
    private bool isRetreating = false;

    [Header("Movimento")]
    public float speed = 2f;
    public Transform pointA;
    public Transform pointB;

    [Header("Ataque")]
    public GameObject arrowPrefab;
    public Transform shootPoint;
    public float shootInterval = 2f;

    [Header("Comportamento")]
    public float visionRange = 4f;
    public float idealMinDistance = 2f;
    public float idealMaxDistance = 4f;

    // ─── Adicionado ──────────────────────────────────────────────────────────────
    [Header("Dissolve Settings")]
    public Material dissolveMaterial;   // material usando Custom/SpriteDissolve.shader
    public Texture2D noiseTexture;      // sua noise mask
    public float dissolveDuration = 1f; // duração do efeito

    private Material _matInstance;      // instância do material
    // ───────────────────────────────────────────────────────────────────────────────

    private Rigidbody2D rb;
    private Transform player;
    private SpriteRenderer sprite;
    private Animator animator;
    private float timer;
    private Vector2 patrolTarget;
    private Vector2 moveTarget;
    private bool isShooting = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        patrolTarget = pointB.position;

        // ─── Instancia o material de dissolve ───────────────────────────────────
        if (dissolveMaterial != null && noiseTexture != null)
        {
            _matInstance = Instantiate(dissolveMaterial);
            _matInstance.SetTexture("_NoiseTex", noiseTexture);
            _matInstance.SetFloat("_Cutoff", 0f);
        }
        // ─────────────────────────────────────────────────────────────────────────
    }

    void Update()
    {
        if (player == null) return;

        // --------- TESTE DE MORTE COM "F" ------------
        if (Input.GetKeyDown(KeyCode.F))
        {
            TriggerDeath();
            return; // não processa mais
        }
        // ---------------------------------------------

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= visionRange)
        {
            FlipSprite(player.position.x);
            if (!isShooting)
            {
                DetectAndShoot(distance);
                ActBasedOnDistance(distance);
            }
            UpdateAnimations();
        }
        else
        {
            Patrol();
        }

        UpdateAnimations();
    }

    void FixedUpdate()
    {
        rb.MovePosition(Vector2.MoveTowards(rb.position, moveTarget, speed * Time.fixedDeltaTime));
    }

    void Patrol()
    {
        moveTarget = patrolTarget;

        if (Vector2.Distance(rb.position, patrolTarget) < 0.1f)
            patrolTarget = (Vector2)patrolTarget == (Vector2)pointA.position ? pointB.position : pointA.position;

        FlipSprite(patrolTarget.x);
        animator.SetBool("isWalking", true);
    }

    void ActBasedOnDistance(float distance)
    {
        if (distance > idealMaxDistance)
        {
            moveTarget = new Vector2(player.position.x, rb.position.y);
            animator.SetBool("isWalking", true);
        }
        else if (distance < idealMinDistance)
        {
            float direction = transform.position.x < player.position.x ? -1 : 1;
            moveTarget = new Vector2(transform.position.x + direction * 1.5f, rb.position.y);
            animator.SetBool("isWalking", true);
        }
        else
        {
            moveTarget = rb.position;
            animator.SetBool("isWalking", false);
        }
    }

    void DetectAndShoot(float distance)
    {
        timer += Time.deltaTime;
        bool inRange = distance >= idealMinDistance && distance <= idealMaxDistance;

        if (inRange && timer >= shootInterval)
        {
            StopAllCoroutines();
            animator.SetTrigger("attackNow");
            isRetreating = false;
            moveTarget = rb.position;

            StartCoroutine(WaitForAnimationAndShoot("EnemyAttack", 0.4f));
            timer = 0f;
        }
    }

    IEnumerator WaitForAnimationAndShoot(string stateName, float normalizedTimeThreshold)
    {
        isShooting = true;
        yield return null;

        while (!animator.GetCurrentAnimatorStateInfo(0).IsName(stateName))
            yield return null;

        while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < normalizedTimeThreshold)
            yield return null;

        Shoot();

        while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
            yield return null;

        isShooting = false;
    }

    public void Shoot()
    {
        if (shootSound != null && audioSource != null)
            audioSource.PlayOneShot(shootSound);

        if (arrowPrefab != null && shootPoint != null)
        {
            GameObject arrow = Instantiate(arrowPrefab, shootPoint.position, Quaternion.identity);
            Vector2 direction = (player.position.x < transform.position.x) ? Vector2.left : Vector2.right;
            arrow.GetComponent<Arrow>().SetDirection(direction);
        }
    }

    void FlipSprite(float targetX)
    {
        sprite.flipX = isRetreating ? !(targetX < transform.position.x) : (targetX < transform.position.x);
    }

    void UpdateAnimations()
    {
        if (isShooting)
        {
            animator.SetBool("isWalking", false);
            return;
        }

        bool isMoving = Vector2.Distance(rb.position, moveTarget) > 0.01f;
        animator.SetBool("isWalking", isMoving);
    }

    public void TriggerDeath()
    {
        // Cancela qualquer ação
        StopAllCoroutines();
        isShooting = false;
        animator.SetBool("isWalking", false);

        // Troca para o material de dissolve (se estiver configurado)
        if (_matInstance != null)
            sprite.material = _matInstance;

        // Desativa física imediata
        rb.simulated = false;

        // Inicia o efeito de dissolve
        StartCoroutine(DissolveRoutine());
    }

    // ─── coroutine de dissolve ────────────────────────────────────────────────
    private IEnumerator DissolveRoutine()
    {
        if (_matInstance == null)
        {
            Destroy(gameObject);
            yield break;
        }

        float t = 0f;
        while (t < dissolveDuration)
        {
            _matInstance.SetFloat("_Cutoff", Mathf.Lerp(0f, 1f, t / dissolveDuration));
            t += Time.deltaTime;
            yield return null;
        }
        Destroy(gameObject);
    }
    // ───────────────────────────────────────────────────────────────────────────
}
