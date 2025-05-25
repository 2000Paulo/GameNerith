using UnityEngine;

public class BatMovement : MonoBehaviour
{
    public float moveDistance = 10f; // Distância total de voo
    public float speed = 2f;         // Velocidade de voo
    public float pauseTime = 1f;     // Tempo de pausa nas extremidades

    private Vector3 pointA;
    private Vector3 pointB;
    private bool goingForward = true;
    private bool isPaused = false;

    private Animator animator;

    void Start()
    {
        pointA = transform.position;
        pointB = pointA + Vector3.right * moveDistance;
        animator = GetComponent<Animator>();
        FaceDirection();
    }

    void Update()
    {
        if (!isPaused)
        {
            Vector3 destination = goingForward ? pointB : pointA;
            transform.position = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, destination) < 0.05f)
            {
                StartCoroutine(SwitchDirectionAfterPause());
            }
        }
    }

    System.Collections.IEnumerator SwitchDirectionAfterPause()
    {
        isPaused = true;
        yield return new WaitForSeconds(pauseTime);

        goingForward = !goingForward;
        FaceDirection();

        isPaused = false;
    }

    void FaceDirection()
    {
        float direction = goingForward ? 1f : -1f;
        transform.localScale = new Vector3(direction, transform.localScale.y, transform.localScale.z);

        if (animator != null)
        {
            animator.SetFloat("Speed", speed); // opcional, se usar animação
        }
    }
}
