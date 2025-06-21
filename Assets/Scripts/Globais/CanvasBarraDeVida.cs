using UnityEngine;
using UnityEngine.UI;

public class CanvasBarraDeVida : MonoBehaviour
{
    public Slider SliderBarraDeVida;
    // As variáveis de cor podem ser usadas no futuro, vamos mantê-las.
    public Color CorVidaCheia;
    public Color CorVidaBaixa;

    void Start()
    {
        // Configuração inicial do Slider
        if (SliderBarraDeVida != null)
        {
            SliderBarraDeVida.wholeNumbers = true;
            SliderBarraDeVida.minValue = 0;
        }
    }

    // A única função que realmente precisamos é esta:
    // para atualizar o valor da vida quando o inimigo toma dano.
    public void AtualizaVida(int vidaAtual, int vidaMaxima)
    {
        if (SliderBarraDeVida != null)
        {
            SliderBarraDeVida.maxValue = vidaMaxima;
            SliderBarraDeVida.value = vidaAtual;
        }
    }

    // O método Update() e AjustaTamanhoDaBarraDeVida() foram removidos 
    // pois a lógica deles era para Screen Space e não é mais necessária.
}