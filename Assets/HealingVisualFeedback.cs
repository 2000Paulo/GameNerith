using UnityEngine;

public class HealingVisualFeedback : MonoBehaviour
{
    public Transform pulseTarget;
    public SpriteRenderer spriteRenderer;

    public float pulseSpeed = 2f;
    public float pulseAmount = 0.05f;
    public Color fullHealthColor = new Color(1f, 1f, 1f, 0.5f);
    public Color lowHealthColor = new Color(1f, 1f, 1f, 1f);

    private Vector3 initialScale;
    private GameObject player;
    private PlayerHealth playerHealth;

    private bool feedbackAtivo = true;

    void Start()
    {
        if (pulseTarget == null)
            pulseTarget = transform;

        initialScale = pulseTarget.localScale;

        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerHealth = player.GetComponent<PlayerHealth>();
        }

        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (!feedbackAtivo || playerHealth == null)
        {
            pulseTarget.localScale = initialScale;
            spriteRenderer.color = fullHealthColor;
            return;
        }

        bool precisaDeCura = playerHealth.GetHealth() < playerHealth.maxHealth;

        if (precisaDeCura)
        {
            float scaleFactor = 1 + Mathf.Sin(Time.time * pulseSpeed) * pulseAmount;
            pulseTarget.localScale = initialScale * scaleFactor;
            spriteRenderer.color = lowHealthColor;
        }
        else
        {
            pulseTarget.localScale = initialScale;
            spriteRenderer.color = fullHealthColor;
        }
    }

    public void DesativarFeedback()
    {
        feedbackAtivo = false;
        pulseTarget.localScale = initialScale;

        if (spriteRenderer != null)
            spriteRenderer.color = fullHealthColor;

        Debug.Log("Feedback visual desativado.");
    }
}
