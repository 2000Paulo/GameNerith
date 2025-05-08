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
            anim.SetBool("isWalking", false);
            waitCounter -= Time.deltaTime;

            if (waitCounter <= 0f)
            {
                isWaiting = false;
                SetNextTarget();
            }
        }
        else
        {
            // Inverter sprite conforme direção
            Vector3 scale = transform.localScale;
            if (targetPosition.x > transform.position.x)
                scale.x = Mathf.Abs(scale.x);
            else if (targetPosition.x < transform.position.x)
                scale.x = -Mathf.Abs(scale.x);
            transform.localScale = scale;

            anim.SetBool("isWalking", true);
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
        anim.SetTrigger("shoot"); // animação deve ter AnimationEvent chamando AtivarFlecha()
        yield return new WaitForSeconds(shootDelay);
        // AtivarFlecha será chamado pela animação
    }

    // Chamado na animação de ataque com Animation Event
    public void AtivarFlecha()
    {
        Debug.Log("➡️ Flecha instanciada em: " + pontoDeDisparo.position);

        GameObject novaFlecha = Instantiate(flechaPrefab, pontoDeDisparo.position, Quaternion.identity);

        Rigidbody2D rb = novaFlecha.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.simulated = true;

            Vector2 direcao = transform.localScale.x > 0 ? Vector2.right : Vector2.left;
            rb.AddForce(direcao * 10f, ForceMode2D.Impulse);

            Debug.Log("🡆 Força aplicada na direção: " + direcao);
        }
        else
        {
            Debug.LogWarning("⚠️ Rigidbody2D não encontrado na flecha instanciada.");
        }

        canShoot = false;
    }

    // Chamado pela flecha quando colide com o chão
    public void PermitirNovoTiro()
    {
        if (playerInSight)
            canShoot = true;

        isShooting = false;
    }

    // Chamado pelo objeto VisaoInimigo (filho com BoxCollider2D trigger)
    public void SetPlayerInSight(bool status)
    {
        playerInSight = status;
    }
}
