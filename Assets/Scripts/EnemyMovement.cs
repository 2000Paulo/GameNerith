using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour
{
    [Header("Som")]
    public AudioClip shootSound;
    private AudioSource audioSource;

    [Header("Movimento")]
    public float speed = 2f;
    public Transform pointA;
    public Transform pointB;

    [Header("Ataque")]
    public GameObject arrowPrefab;
    public Transform shootPoint;
    public float shootInterval = 2f;

    [Header("Comportamento")]
    public float visionRange = 5f;
    public float idealMinDistance = 2.5f;
    public float idealMaxDistance = 4.5f;

    [Header("Dissolve Settings")]
    public Material dissolveMaterial;
    public Texture2D noiseTexture;
    public float dissolveDuration = 1f;

    private Material _matInstance;
    private Rigidbody2D rb;
    private Transform player;
    private SpriteRenderer sprite;
    private Animator animator;

    private float timer;
    private Vector2 patrolTarget;
    private Vector2 moveTarget;
    private bool isShooting = false;
    private bool isRetreating = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        sprite = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponent<Animator>();

        patrolTarget = pointB.position;
        moveTarget = patrolTarget;

        if (dissolveMaterial != null && noiseTexture != null)
        {
            _matInstance = Instantiate(dissolveMaterial);
            _matInstance.SetTexture("_NoiseTex", noiseTexture);
            _matInstance.SetFloat("_Cutoff", 0f);
        }
    }

    void Update()
    {
        if (player == null) return;

        if (Input.GetKeyDown(KeyCode.F))
        {
            TriggerDeath();
            return;
        }

        float distance = Vector2.Distance(transform.position, player.position);
        bool playerVisible = PlayerInSight();

        if (playerVisible && distance <= visionRange)
        {
            FlipSprite(player.position.x);

            if (!isShooting)
            {
                DetectAndShoot(distance);
                ActBasedOnDistance(distance);
            }
        }
        else
        {
            Patrol();
        }

        UpdateAnimations();
    }

    void FixedUpdate()
    {
        if (!isShooting)
        {
            rb.MovePosition(Vector2.MoveTowards(rb.position, moveTarget, speed * Time.fixedDeltaTime));
        }
    }

    void Patrol()
    {
        moveTarget = patrolTarget;

        if (Vector2.Distance(rb.position, patrolTarget) < 0.1f)
        {
            patrolTarget = patrolTarget == (Vector2)pointA.position ? pointB.position : pointA.position;
        }

        FlipSprite(patrolTarget.x);
    }

    void ActBasedOnDistance(float distance)
    {
        if (distance > idealMaxDistance)
        {
            moveTarget = new Vector2(player.position.x, rb.position.y);
        }
        else if (distance < idealMinDistance)
        {
            float dir = transform.position.x < player.position.x ? -1 : 1;
            moveTarget = new Vector2(transform.position.x + dir * 1.5f, rb.position.y);
        }
        else
        {
            moveTarget = rb.position;
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
            animator.SetFloat("Speed", 0f);
            isRetreating = false;
            moveTarget = rb.position;

            StartCoroutine(WaitForAnimationAndShoot("EnemyAttack", 0.4f));
            timer = 0f;
        }
    }

    IEnumerator WaitForAnimationAndShoot(string stateName, float threshold)
    {
        isShooting = true;
        yield return null;

        while (!animator.GetCurrentAnimatorStateInfo(0).IsName(stateName))
            yield return null;

        while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < threshold)
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
            Vector2 dir = player.position.x < transform.position.x ? Vector2.left : Vector2.right;
            GameObject arrow = Instantiate(arrowPrefab, shootPoint.position, Quaternion.identity);
            arrow.GetComponent<Arrow>().SetDirection(dir);
        }
    }

    void FlipSprite(float targetX)
    {
        sprite.flipX = isRetreating ? !(targetX < transform.position.x) : (targetX < transform.position.x);
    }

    bool PlayerInSight()
    {
        float distanceX = Mathf.Abs(transform.position.x - player.position.x);
        return distanceX <= visionRange;
    }

    void UpdateAnimations()
    {
        if (isShooting)
        {
            animator.SetFloat("Speed", 0f);
            return;
        }

        bool isMoving = Vector2.Distance(rb.position, moveTarget) > 0.01f;
        animator.SetFloat("Speed", isMoving ? 1f : 0f);
    }

    public void TriggerDeath()
    {
        StopAllCoroutines();
        isShooting = false;
        animator.SetFloat("Speed", 0f);
        rb.simulated = false;

        if (_matInstance != null)
            sprite.material = _matInstance;

        StartCoroutine(DissolveRoutine());
    }

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

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, visionRange);

        if (sprite != null)
        {
            float dirX = sprite.flipX ? -1f : 1f;
            Vector3 forwardEnd = transform.position + Vector3.right * dirX * visionRange;
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, forwardEnd);
        }
    }
}
