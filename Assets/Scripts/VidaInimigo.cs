using UnityEngine;
using System.Collections;

public class VidaInimigo : MonoBehaviour
{
    public int vidaMaxima = 100;
    public int vidaAtual;

    private Animator animator;
    private Collider2D colisor;
    private PatrulhaInimigoVoador scriptPatrulha;
    private Rigidbody2D rb;

    void Start()
    {
        vidaAtual = vidaMaxima;
        animator = GetComponent<Animator>();
        colisor = GetComponent<Collider2D>();
        scriptPatrulha = GetComponent<PatrulhaInimigoVoador>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Função pública para que outros scripts possam causar dano
    public void LevarDano(int quantidadeDano)
{
    if (vidaAtual <= 0) return;

    vidaAtual -= quantidadeDano;

    // 4. Log para confirmar que o dano foi recebido e mostrar a vida.
    Debug.Log(gameObject.name + " levou dano! Vida restante: " + vidaAtual);

    StartCoroutine(FeedbackDeDano());

    if (vidaAtual <= 0)
    {
        // 5. Log crucial para saber se a condição de morte foi atingida.
        Debug.Log(gameObject.name + " deveria morrer agora!");
        Morrer();
    }
}

    private void Morrer()
    {
        // Chama método personalizado se existir (como no arqueiro)
        SendMessage("CustomDeath", SendMessageOptions.DontRequireReceiver);

        // Lógica padrão de morte – continua funcionando para inimigos comuns
        if (scriptPatrulha != null)
            scriptPatrulha.enabled = false;

        if (scriptPatrulha != null && scriptPatrulha.zonaDeAtaque != null)
            scriptPatrulha.zonaDeAtaque.SetActive(false);

        gameObject.layer = LayerMask.NameToLayer("InimigoMorto");

        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.gravityScale = 0.8f;
        }

        if (animator != null)
            animator.SetTrigger("Morrer");

        Destroy(gameObject, 1f);
    }

    // Coroutine para feedback visual de dano (opcional)
    private IEnumerator FeedbackDeDano()
    {
        // Pisca a cor do sprite para vermelho
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        if(sprite != null)
        {
            sprite.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            sprite.color = Color.white;
        }
    }
}