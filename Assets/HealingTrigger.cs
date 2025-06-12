using UnityEngine;

public class HealingTrigger : MonoBehaviour
{
    public int healAmount = 20;
    public Sprite spriteCheio;
    public Sprite spriteVazio;

    private bool usado = false;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteCheio != null)
            spriteRenderer.sprite = spriteCheio;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (usado) return;

        if (other.CompareTag("Player"))
        {
            PlayerHealer healer = other.GetComponent<PlayerHealer>();
            PlayerHealth health = other.GetComponent<PlayerHealth>();

            if (healer != null && health != null && health.GetHealth() < health.maxHealth)
            {
                healer.HealPlayer(healAmount, "Vaso de Cura");

                usado = true;

                if (spriteVazio != null)
                    spriteRenderer.sprite = spriteVazio;

                // Desativa o efeito visual
                HealingVisualFeedback feedback = GetComponent<HealingVisualFeedback>();
                if (feedback != null)
                {
                    feedback.DesativarFeedback();
                }

                Debug.Log("Vaso usado! Agora está vazio.");
            }
        }
    }
}
