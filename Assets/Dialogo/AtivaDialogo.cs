using UnityEngine;

public class AtivaDialogo : MonoBehaviour
{
    public GameObject dialogoPainel; // painel com o texto
    public TypeTextAnimation animacaoTexto; // script de animação
    private bool jogadorPerto = false;
    private bool dialogoAtivo = false;

    void Update()
    {
        if (jogadorPerto && !dialogoAtivo && Input.GetKeyDown(KeyCode.P))
        {
            dialogoPainel.SetActive(true); // ativa painel
            animacaoTexto.StartTyping();   // inicia digitação
            dialogoAtivo = true;
        }

        // Se o diálogo estiver ativo, ENTER fecha
        if (dialogoAtivo && Input.GetKeyDown(KeyCode.Return))
        {
            dialogoPainel.SetActive(false);
            dialogoAtivo = false;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            jogadorPerto = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            jogadorPerto = false;
        }
    }
}
