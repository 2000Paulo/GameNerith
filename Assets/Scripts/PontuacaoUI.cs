using UnityEngine;
using TMPro;

public class PontuacaoUI : MonoBehaviour
{
    public TextMeshProUGUI textoPontos;

    void Start()
    {
        AtualizarTexto();
    }

    public void AdicionarPontos(int quantidade)
    {
        GameManager.instance.AdicionarPontos(quantidade);
        AtualizarTexto();
    }

    private void AtualizarTexto()
    {
        textoPontos.text = "Pontos: " + GameManager.instance.pontuacaoAtual;
    }

    public int GetPontos()
    {
        return GameManager.instance.pontuacaoAtual;
    }
}
