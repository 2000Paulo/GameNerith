using UnityEngine;
using TMPro;

public class PontuacaoUI : MonoBehaviour
{
    public TextMeshProUGUI textoPontos;
    private int pontos = 0;

    void Start()
    {
        AtualizarTexto();
    }

    public void AdicionarPontos(int quantidade)
    {
        pontos += quantidade;
        AtualizarTexto();
    }

    private void AtualizarTexto()
    {
        textoPontos.text = "Pontos: " + pontos;
    }

    public int GetPontos()
    {
        return pontos;
    }
}
