using UnityEngine;

public class AtaqueJogador : MonoBehaviour
{
    public int danoDoAtaque = 25; // Defina o dano que este ataque causa

    // Esta função é chamada quando o colisor entra em contato com outro colisor (trigger)
    private void OnTriggerEnter2D(Collider2D other)
{
    // 1. Log para ver se a colisão foi detectada.
    Debug.Log("Hitbox colidiu com: " + other.name);

    if (other.gameObject.CompareTag("Inimigo"))
    {
        // 2. Log para ver se a tag "Inimigo" foi reconhecida.
        Debug.Log("Objeto com a tag Inimigo detectado!");

        // Linha atualizada para buscar "VidaInimigo".
        VidaInimigo vidaInimigo = other.GetComponent<VidaInimigo>();

        if (vidaInimigo != null)
        {
            // 3. Log para confirmar que o dano está sendo enviado.
            Debug.Log("Enviando " + danoDoAtaque + " de dano para " + other.name);
            vidaInimigo.LevarDano(danoDoAtaque);
        }
        else
        {
            // Log de erro caso o script VidaInimigo não seja encontrado.
            Debug.LogError("ERRO: O objeto " + other.name + " tem a tag Inimigo, mas não tem o script VidaInimigo anexado!");
        }
    }
}
}