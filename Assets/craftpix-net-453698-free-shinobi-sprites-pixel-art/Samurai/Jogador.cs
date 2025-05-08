using UnityEngine;

public class Jogador : MonoBehaviour
{
    public float walkSpeed = 2f;
    public float runSpeed = 4f;

    private Rigidbody2D rb;
    private Animator anim;
    private float moveInput;
    private bool isRunning;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        moveInput = Input.GetAxisRaw("Horizontal");
        isRunning = Input.GetKey(KeyCode.LeftShift);

        anim.SetBool("isWalking", moveInput != 0);
        anim.SetBool("isRunning", isRunning);

        if (moveInput != 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = moveInput > 0 ? 1 : -1;
            transform.localScale = scale;
        }
    }

    private void FixedUpdate()
    {
        float speed = isRunning ? runSpeed : walkSpeed;
        rb.linearVelocity = new Vector2(moveInput * speed, rb.linearVelocity.y);
    }
}
