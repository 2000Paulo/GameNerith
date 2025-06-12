using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class TrapArapuca : MonoBehaviour
{
    public int damageAmount = 30;
    public string animationTrigger = "Fechar";

    private Animator animator;
    private AudioSource audioSource;
    private bool ativada = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        // Garante que o som não toque automaticamente ao iniciar
        if (audioSource != null)
            audioSource.playOnAwake = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (ativada) return;

        if (other.CompareTag("Player"))
        {
            ativada = true;

            if (animator != null)
                animator.SetTrigger(animationTrigger);

            if (audioSource != null)
                audioSource.Play();

            PlayerDamageReceiver damageReceiver = other.GetComponent<PlayerDamageReceiver>();
            if (damageReceiver != null)
            {
                damageReceiver.ApplyDamage(damageAmount, "arapuca");
            }

            Debug.Log("Arapuca ativada: dano aplicado!");
        }
    }

    // ⚠️ Essa função será chamada pelo Animation Event no final da animação
    public void OnFecharFinalizado()
    {
        StartCoroutine(DesaparecerAposDelay());
    }

    private IEnumerator DesaparecerAposDelay()
    {
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false); // ou Destroy(gameObject);
    }
}
