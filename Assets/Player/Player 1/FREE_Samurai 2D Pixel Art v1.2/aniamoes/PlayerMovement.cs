using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer sr;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        float move = Input.GetAxisRaw("Horizontal");

        // Movimento
        rb.linearVelocity = new Vector2(move * speed, rb.linearVelocity.y);

        // Animação
        animator.SetFloat("Speed", Mathf.Abs(move));

        // Flip (espelhar)
        if (move != 0)
            sr.flipX = move < 0;
    }
}
