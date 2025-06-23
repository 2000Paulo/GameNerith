using DbHelpers;
using UnityEngine;
using UnityEngine.UI;

public class CanvasBarraDeVida : MonoBehaviour
{
    public Slider BarraDeVida;
    public Color CorVidaCheia;
    public Color CorVidaBaixa;
    private int ultimoComprimentoDeTela, ultimaAlturaDeTela;

    void Start()
    {
        BarraDeVida.wholeNumbers = true;
        BarraDeVida.minValue = 0;
    }

    void Update()
    {

        AtivaOuDesativaBarraDeVida();
        // AjustaTamanhoBarraDeVida();

    }

    public void AtualizaVida(int vidaAtual, int vidaMaxima)
    {
        BarraDeVida.maxValue = vidaMaxima;
        BarraDeVida.value = vidaAtual;
    }

    private void AtivaOuDesativaBarraDeVida()
    {
        // se o inimigo está dentro da câmera do Jogo (Main Camera) então
        // renderiza a barra de vida
        BarraDeVida.gameObject.SetActive(
            0 <= Camera.main.WorldToViewportPoint(this.transform.parent.position).x && Camera.main.WorldToViewportPoint(this.transform.parent.position).x <= 1 &&
            0 <= Camera.main.WorldToViewportPoint(this.transform.parent.position).y && Camera.main.WorldToViewportPoint(this.transform.parent.position).y <= 1
        );
    }

    private void AjustaTamanhoBarraDeVida()
    {

        // se a resolução de tela muda então o comprimento da tela mudou ou a altura da tela mudou
        // então o tamanho da barra de vida muda e o tamanho de tela respeita a proporção:
        // 816 / 30 (valor de comprimento que me agradou) = 27.2
        if (
            Camera.main.pixelWidth  != ultimoComprimentoDeTela ||
            Camera.main.pixelHeight != ultimaAlturaDeTela
        )
        {
            BarraDeVida.GetComponent<RectTransform>().sizeDelta =
                new Vector2(
                    Camera.main.pixelWidth  / 27.2f,
                    Camera.main.pixelHeight / 68f
                );
            ultimoComprimentoDeTela = Camera.main.pixelWidth;
            ultimaAlturaDeTela      = Camera.main.pixelHeight;
        }
    }

}