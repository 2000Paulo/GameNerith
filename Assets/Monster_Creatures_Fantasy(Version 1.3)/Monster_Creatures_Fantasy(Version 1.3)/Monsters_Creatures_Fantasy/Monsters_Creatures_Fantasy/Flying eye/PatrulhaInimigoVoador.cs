using UnityEngine;
using System.Collections;

public class PatrulhaInimigoVoador : MonoBehaviour
{
    public float speed = 2f;
    public float patrolDistance = 3f;
    public float visionRange = 5f;
    public float dashSpeed = 5f;
    public float hoverTime = 0.5f;
    public float dashCooldown = 2f;
    public int damageAmount = 20;

    [Header("Referência da zona de ataque")]
    public GameObject zonaDeAtaque;

    private Vector3 startPosition;
    private Vector3 leftTarget;
    private Vector3 rightTarget;
    private Vector3 currentTarget;
    private bool isAttacking = false;

    private Transform player;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private Vector3 lockedTargetPosition;
    private bool playerWasVisible = false;

    private void Start()
    {
        startPosition = transform.position;
        leftTarget = startPosition + Vector3.left * patrolDistance;
        rightTarget = startPosition + Vector3.right * patrolDistance;
        currentTarget = rightTarget;

        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (zonaDeAtaque != null)
            zonaDeAtaque.SetActive(false); // hitbox começa desativada
    }

    private void Update()
    {
        if (player == null || isAttacking) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        bool playerVisible = distanceToPlayer <= visionRange;

        if (playerVisible && !playerWasVisible)
        {
            lockedTargetPosition = player.position;
            StartCoroutine(AttackSequence());
        }

        playerWasVisible = playerVisible;

        if (!playerVisible && !isAttacking)
        {
            Patrol();
        }
    }

    private void Patrol()
    {
        animator.Play("Fly");

        transform.position = Vector3.MoveTowards(transform.position, currentTarget, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, currentTarget) < 0.01f)
        {
            if (currentTarget == rightTarget)
                currentTarget = startPosition;
            else if (currentTarget == startPosition)
                currentTarget = leftTarget;
            else
                currentTarget = rightTarget;
        }

        spriteRenderer.flipX = currentTarget.x < transform.position.x;
    }

    private IEnumerator AttackSequence()
    {
        isAttacking = true;

        animator.Play("Fly");
        spriteRenderer.flipX = lockedTargetPosition.x < transform.position.x;

        yield return new WaitForSeconds(hoverTime);

        while (Vector3.Distance(transform.position, lockedTargetPosition) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, lockedTargetPosition, dashSpeed * Time.deltaTime);
            spriteRenderer.flipX = lockedTargetPosition.x < transform.position.x;
            yield return null;
        }

        animator.Play("Attack");

        if (zonaDeAtaque != null)
            zonaDeAtaque.SetActive(true); // ativa a hitbox

        yield return new WaitForSeconds(0.4f); // tempo da hitbox ativa

        if (zonaDeAtaque != null)
            zonaDeAtaque.SetActive(false); // desativa após ataque

        animator.Play("Fly");

        while (Vector3.Distance(transform.position, startPosition) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, startPosition, speed * Time.deltaTime);
            spriteRenderer.flipX = startPosition.x < transform.position.x;
            yield return null;
        }

        transform.position = startPosition;
        yield return new WaitForSeconds(dashCooldown);

        isAttacking = false;
        playerWasVisible = false;
    }
}
