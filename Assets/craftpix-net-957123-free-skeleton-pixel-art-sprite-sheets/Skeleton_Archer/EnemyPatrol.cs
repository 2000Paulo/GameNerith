using UnityEngine;
using System.Collections;

public class EnemyPatrol : MonoBehaviour
{
    [Header("Patrulha")]
    public float speed = 1f;
    public float waitTime = 2f;
    public int rightSteps = 1;
    public int leftSteps = 2;

    [Header("Ataque")]
    public GameObject flechaPrefab;
    public Transform pontoDeDisparo;
    public float shootDelay = 1f;

    private Animator anim;
    private Vector2 startPosition;
    private Vector2 targetPosition;
    private float moveDirection = 0f;

    private bool movingRight = true;
    private bool isWaiting = false;
    private float waitCounter = 0f;

    private bool playerInSight = false;
    private bool isShooting = false;
    private bool canShoot = true;

    private void Start()
    {
        anim = GetComponent<Animator>();
        startPosition = transform.position;
        SetNextTarget();
    }

    private void Update()
    {
        if (playerInSight && canShoot && !isShooting)
        {
            StartCoroutine(Shoot());
            return;
        }

        if (isShooting) return;

        if (isWaiting)
        {
            moveDirection = 0f;
            anim.SetFloat("Speed", moveDirection);
            waitCounter -= Time.deltaTime;

            if (waitCounter <= 0f)
            {
                isWaiting = false;
                SetNextTarget();
            }
        }
        else
        {
            // Calcula a direção do movimento
            moveDirection = targetPosition.x > transform.position.x ? 1f : -1f;
            anim.SetFloat("Speed", Mathf.Abs(moveDirection));

            // Inverter sprite conforme direção
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x) * (moveDirection > 0 ? 1 : -1);
            transform.localScale = scale;

            transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

            if (Vector2.Distance(transform.position, targetPosition) < 0.01f)
            {
                isWaiting = true;
                waitCounter = waitTime;
                movingRight = !movingRight;
            }
        }
    }

    void SetNextTarget()
    {
        float direction = movingRight ? 1 : -1;
        float steps = movingRight ? rightSteps : leftSteps;
        targetPosition = startPosition + new Vector2(direction * steps, 0);
    }

    private IEnumerator Shoot()
    {
        isShooting = true;
        moveDirection = 0f;
        anim.SetFloat("Speed", moveDirection);
        anim.SetTrigger("shoot");
        yield return new WaitForSeconds(shootDelay);
    }

    public void AtivarFlecha()
    {
        GameObject novaFlecha = Instantiate(flechaPrefab, pontoDeDisparo.position, Quaternion.identity);

        Rigidbody2D rb = novaFlecha.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.simulated = true;
            Vector2 direcao = transform.localScale.x > 0 ? Vector2.right : Vector2.left;
            rb.AddForce(direcao * 10f, ForceMode2D.Impulse);
        }

        canShoot = false;
    }

    public void PermitirNovoTiro()
    {
        if (playerInSight)
            canShoot = true;

        isShooting = false;
    }

    public void SetPlayerInSight(bool status)
    {
        playerInSight = status;
    }
}
