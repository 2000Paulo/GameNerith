using UnityEngine;
using System.Collections;

public class GuerreiroPatrulha : MonoBehaviour
{
    public float speed = 1f;
    public float waitTime = 2f;
    public int leftSteps = 2;
    public int rightSteps = 2;

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

            // Direção visual
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
}
