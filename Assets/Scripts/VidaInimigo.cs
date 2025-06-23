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
        // 1. Desativa a lógica de patrulha para que ele pare de se mover sozinho.
        if (scriptPatrulha != null)
        {
            scriptPatrulha.enabled = false;
        }

        // ADICIONE ESTE BLOCO AQUI
        // 2. Garante que a hitbox de ataque seja desativada.
        // Isso previne o bug de o inimigo causar dano enquanto já está morto.
        if (scriptPatrulha != null && scriptPatrulha.zonaDeAtaque != null)
        {
            scriptPatrulha.zonaDeAtaque.SetActive(false);
        }
        // FIM DO BLOCO ADICIONADO

        // 3. Muda a layer para "InimigoMorto" para não colidir com o jogador.
        gameObject.layer = LayerMask.NameToLayer("InimigoMorto");

        // 4. Ativa a física para o inimigo cair.
        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.gravityScale = 0.8f; // Usando o valor que ajustamos antes
        }
        
        // 5. Dispara a animação de morte.
        if (animator != null)
        {
            animator.SetTrigger("Morrer");
        }

        // 6. Destrói o objeto após um tempo.
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