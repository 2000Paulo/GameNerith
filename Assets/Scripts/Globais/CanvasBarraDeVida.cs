using UnityEngine;
using UnityEngine.UI;
using DbHelpers;

public class CanvasBarraDeVida : MonoBehaviour
{
    public Slider SliderBarraDeVida;
    public Color CorVidaCheia;
    public Color CorVidaBaixa;
    public Vector3 Altura;

    private int ultimoComprimentoDeTela, ultimaAlturaDeTela;


    void Start()
    {
        AjustaTamanhoDaBarraDeVida();
    }


    void Update()
    {
        // Controle dara renderizar ou não a barra de vida
        var PosicaoInimigoOuPlayer = transform.parent.position;
        Vector3 NovaPosicaoSlider = PosicaoInimigoOuPlayer + Altura;
        Vector3 NovaPosicaoSlider2d = Camera.main.WorldToScreenPoint(NovaPosicaoSlider);
        SliderBarraDeVida.transform.position = NovaPosicaoSlider2d;

        Vector3 PosicaoInimigoOuPlayerCameraVisivel = Camera.main.WorldToViewportPoint(PosicaoInimigoOuPlayer);

        bool inimigoEstaVisivel =
            0 <= PosicaoInimigoOuPlayerCameraVisivel.x && PosicaoInimigoOuPlayerCameraVisivel.x <= 1 &&
            0 <= PosicaoInimigoOuPlayerCameraVisivel.y && PosicaoInimigoOuPlayerCameraVisivel.y <= 1;

        SliderBarraDeVida.gameObject.active = inimigoEstaVisivel;

        // caso o tamanho de tela mude, ajusta o tamanho da barra de vida
        int comprimentoDeTelaAtual = Screen.width;
        int alturaDeTelaAtual =  Screen.height;
        if (
            comprimentoDeTelaAtual != ultimoComprimentoDeTela ||
            alturaDeTelaAtual != ultimaAlturaDeTela
        ) {
            ultimoComprimentoDeTela = comprimentoDeTelaAtual;
            ultimaAlturaDeTela = alturaDeTelaAtual;
            AjustaTamanhoDaBarraDeVida();
        }
    }

    private void AjustaTamanhoDaBarraDeVida()
    {
        // Definindo, com base no tamanho da tela, o comprimento e a algura da barra de vida.
        RectTransform oTamanhoBarraDeVida = SliderBarraDeVida.GetComponent<RectTransform>();
        // Tais valores foram escolhidos pois, em uma tela de jogo de 816 x 468, o comprimento x altura que me agradaram foi : 30 x 7
        // Então para conseguir o comprimento equivalente para diferentes tipos de tela foi só dividir 816 / 30 (valor de comprimento que me agradou) = 27.2 
        // e consegui o divisor para os outros tamanhos de tela. A mesma coisa foi realizada para a altura.
        float NovoComprimento = Screen.width / 27.2f;
        float NovaAltura = Screen.height / 68f;

        oTamanhoBarraDeVida.sizeDelta = new Vector2(
            NovoComprimento, // width 
            NovaAltura // height
        );
    }

}
