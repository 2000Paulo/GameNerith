using UnityEngine;
using TMPro;

public class PontuacaoUI : MonoBehaviour
{
    public TextMeshProUGUI textoPontos;
    private int pontos = 0;

    void Start()
    {
        // Carrega pontuação salva, se existir
        pontos = EstadoDoJogador.pontuacaoAtual;
        AtualizarTexto();
    }

    public void AdicionarPontos(int quantidade)
    {
        pontos += quantidade;

        // Atualiza pontuação global
        EstadoDoJogador.pontuacaoAtual = pontos;

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
