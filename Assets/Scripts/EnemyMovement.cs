using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour
{
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
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        patrolTarget = pointB.position;
    }

    void Update()
    {
        if (player == null) return;

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
        {
            patrolTarget = (Vector2)patrolTarget == (Vector2)pointA.position ? pointB.position : pointA.position;
        }

        FlipSprite(patrolTarget.x);
    }

    void ActBasedOnDistance(float distance)
    {
        if (distance > idealMaxDistance)
        {
            moveTarget = new Vector2(player.position.x, rb.position.y); // Aproxima
        }
        else if (distance < idealMinDistance)
        {
            float direction = transform.position.x < player.position.x ? -1 : 1;
            moveTarget = new Vector2(transform.position.x + direction * 1.5f, rb.position.y); // Recuar
        }
        else
        {
            moveTarget = rb.position; // Parado
        }
    }

    void DetectAndShoot(float distance)
    {
        timer += Time.deltaTime;

        bool inRange = distance >= idealMinDistance && distance <= idealMaxDistance;

        if (inRange && timer >= shootInterval)
        {
            // Evita conflitos
            StopAllCoroutines();

            animator.SetTrigger("attackNow");
            isRetreating = false;
            moveTarget = rb.position;

            StartCoroutine(WaitForAnimationAndShoot("EnemyAttack", 0.4f)); // nome do estado e tempo
            timer = 0f;
        }
    }

    IEnumerator WaitForAnimationAndShoot(string stateName, float normalizedTimeThreshold)
    {
        yield return null; // aguarda 1 frame para a transição ocorrer

        // Aguarda a transição para o estado de ataque
        while (!animator.GetCurrentAnimatorStateInfo(0).IsName(stateName))
            yield return null;

        // Aguarda até atingir o ponto da flecha
        while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < normalizedTimeThreshold)
            yield return null;

        Shoot();
    }

    IEnumerator DelayedShoot(float delay)
    {
        isShooting = true;
        yield return new WaitForSeconds(delay);
        Shoot();
        yield return new WaitForSeconds(0.4f);
        isShooting = false;
        animator.SetBool("isWalking", true); // <-- reforça a volta para animação de andar
    }

    public void Shoot()
    {
        GameObject arrow = Instantiate(arrowPrefab, shootPoint.position, Quaternion.identity);
        Vector2 direction = (player.position.x < transform.position.x) ? Vector2.left : Vector2.right;
        arrow.GetComponent<Arrow>().SetDirection(direction);
    }

    void FlipSprite(float targetX)
    {
        sprite.flipX = isRetreating ? !(targetX < transform.position.x) : (targetX < transform.position.x);
    }

    void UpdateAnimations()
    {
        float distance = Vector2.Distance(rb.position, moveTarget);

        if (distance > 0.01f)
        {
            walkCooldown = 0.15f; // mantêm andando por +150ms
        }
        else
        {
            walkCooldown -= Time.deltaTime;
        }

        bool isMoving = walkCooldown > 0f;
        animator.SetBool("isWalking", isMoving);
    }

}
