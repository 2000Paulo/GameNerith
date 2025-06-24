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
            AbrirDialogo();
        }

        if (dialogoAtivo && Input.GetKeyDown(KeyCode.Return))
        {
            FecharDialogo();
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

            if (dialogoAtivo)
            {
                FecharDialogo();
            }
        }
    }

    void AbrirDialogo()
    {
        dialogoPainel.SetActive(true);
        animacaoTexto.StartTyping();
        dialogoAtivo = true;
    }

    void FecharDialogo()
    {
        dialogoPainel.SetActive(false);
        dialogoAtivo = false;
    }
}
