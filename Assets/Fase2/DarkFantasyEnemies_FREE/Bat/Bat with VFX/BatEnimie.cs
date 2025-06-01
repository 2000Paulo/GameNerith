using UnityEngine;
using System.Collections;

public class BatEnimie : MonoBehaviour
{
    public float visionRange = 6f;
    public float speed = 2f;
    public float dashSpeed = 6f;
    public float patrolHoverTime = 1.5f;
    public float timeBeforeAttack = 1f;
    public float returnToSleepTime = 1.5f;

    private Vector3 sleepPosition;
    private Transform player;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    private bool isAwake = false;
    private bool isAttacking = false;
    private bool playerInSight = false;
    private bool sleepingCoroutineStarted = false;

    private void Start()
    {
        sleepPosition = transform.position;
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        animator.Play("BatSleep");
    }

    private void Update()
    {
        if (isAttacking || player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);
        playerInSight = distance <= visionRange;

        if (playerInSight)
        {
            if (!isAwake)
                StartCoroutine(WakeAndFly());
            else if (!sleepingCoroutineStarted)
                StartCoroutine(FlyAndAttackLoop());
        }
    }

    private IEnumerator WakeAndFly()
    {
        isAwake = true;

        // ⏱️ Delay antes de acordar
        yield return new WaitForSeconds(1f);

        animator.SetTrigger("Wake");
        yield return new WaitForSeconds(0.5f); // tempo da animação BatWake

        animator.Play("BatFly");

        // ⏱️ NOVO: espera mais 2 segundos antes de iniciar ataques
        yield return new WaitForSeconds(2f);

        if (playerInSight)
        {
            StartCoroutine(FlyAndAttackLoop());
        }
        else
        {
            StartCoroutine(ReturnToSleep());
        }
    }

    private IEnumerator FlyAndAttackLoop()
    {
        sleepingCoroutineStarted = true;

        while (playerInSight)
        {
            yield return new WaitForSeconds(patrolHoverTime);

            if (!playerInSight) break;

            yield return StartCoroutine(Attack());
        }

        sleepingCoroutineStarted = false;

        yield return new WaitForSeconds(returnToSleepTime);

        if (!playerInSight)
        {
            StartCoroutine(ReturnToSleep());
        }
        else
        {
            StartCoroutine(FlyAndAttackLoop());
        }
    }

    private IEnumerator Attack()
    {
        isAttacking = true;

        Vector3 attackTarget = player.position;

        // ✅ Corrigido: trava a direção corretamente antes do rasante
        bool goingLeft = attackTarget.x < transform.position.x;
        spriteRenderer.flipX = goingLeft;

        yield return new WaitForSeconds(0.3f); // hover antes do ataque

        while (Vector3.Distance(transform.position, attackTarget) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, attackTarget, dashSpeed * Time.deltaTime);

            // ❌ NÃO atualiza mais a direção aqui, ela já está travada
            yield return null;
        }

        // ✅ Garante que ele ainda está virado na direção certa
        spriteRenderer.flipX = goingLeft;

        animator.SetTrigger("Attack");
        yield return new WaitForSeconds(0.4f); // tempo da animação de ataque

        // Volta para cima após ataque
        animator.Play("BatFly");
        while (Vector3.Distance(transform.position, sleepPosition) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, sleepPosition, speed * Time.deltaTime);
            spriteRenderer.flipX = sleepPosition.x < transform.position.x;
            yield return null;
        }

        isAttacking = false;
    }

    private IEnumerator ReturnToSleep()
    {
        animator.SetTrigger("Sleep");
        yield return new WaitForSeconds(0.3f);
        animator.Play("BatSleep");

        isAwake = false;
        sleepingCoroutineStarted = false;
    }
}
