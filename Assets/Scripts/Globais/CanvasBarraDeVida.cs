using UnityEngine;
using UnityEngine.UI;
using DbHelpers;

public class CanvasBarraDeVida : MonoBehaviour
{
    public Slider SliderBarraDeVida;
    public Color CorVidaCheia;
    public Color CorVidaBaixa;
    public Vector3 Altura;

    // private Camera oCameraPrincipal;

    // // Start is called once before the first execution of Update after the MonoBehaviour is created
    // void Start()
    // {
    //     oCameraPrincipal = Camera.main;
    // }

    // Update is called once per frame
    void Update()
    {
        var PosicaoInimigoOuPlayer = transform.parent.position;
        Vector3 NovaPosicaoSlider = PosicaoInimigoOuPlayer + Altura;
        Vector3 NovaPosicaoSlider2d = Camera.main.WorldToScreenPoint(NovaPosicaoSlider);
        SliderBarraDeVida.transform.position = NovaPosicaoSlider2d;

        // Debug.Log("ViewPort");
        // Xinicial = 0 Yinicial = 0 Xfinal = 1 Yfinal = 1
        Vector3 PosicaoInimigoOuPlayerCameraVisivel = Camera.main.WorldToViewportPoint(PosicaoInimigoOuPlayer);

        bool inimigoEstaVisivel =
            0 <= PosicaoInimigoOuPlayerCameraVisivel.x && PosicaoInimigoOuPlayerCameraVisivel.x <= 1 &&
            0 <= PosicaoInimigoOuPlayerCameraVisivel.y && PosicaoInimigoOuPlayerCameraVisivel.y <= 1;



        // SliderBarraDeVida.isActiveAndEnabled = inimigoEstaVisivel;
        SliderBarraDeVida.gameObject.active = inimigoEstaVisivel;

        // DbDebugger.DebugObject(SliderBarraDeVida.gameObject.active);


        // Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, Camera.main.transform.position.z);
        // Debug.Log("screenCenter "+screenCenter);

        // Vector3 screenHeight = new Vector3(Screen.width / 2, Screen.height, Camera.main.transform.position.z);
        // Debug.Log("screenHeight " + screenHeight);

        // Vector3 screenWidth = new Vector3(Screen.width, Screen.height/2, Camera.main.transform.position.z);
        // Debug.Log("screenWidth " + screenWidth);

        // Vector3 goscreen = Camera.main.WorldToScreenPoint(transform.position);
        // Debug.Log("GoPos " + goscreen);

        // float distX = Vector3.Distance(new Vector3(Screen.width / 2, 0f, 0f), new Vector3(goscreen.x, 0f,0f));
        // Debug.Log("distX " + distX);

        // float distY = Vector3.Distance(new Vector3(0f, Screen.height / 2, 0f), new Vector3(0f, goscreen.y, 0f));
        // Debug.Log("distY " + distY);

        // if(distX > Screen.width / 2 || distY > Screen.height / 2)
        // {
        //     Debug.Log("Visivel");
        // }

        // DbDebugger.DebugObject(Screen.width, Screen.height);
        // Debug.Log("Tela");
        // Debug.Log("Comprimento : " + Screen.width + " Altura: " + Screen.height);
        // Debug.Log("Inimigo");

        // Vector3 CameraVisivel = Camera.main.WorldToScreenPoint( Camera.main.transform.position);
        // Vector3 PosicaoPosicaoInimigoOuPlayer2d = Camera.main.WorldToScreenPoint(transform.parent.position);

        // Debug.Log("Camera Visivel");
        // Debug.Log(CameraVisivel);

        // Debug.Log("PosicaoPosicaoInimigoOuPlayer2d");
        // Debug.Log(PosicaoPosicaoInimigoOuPlayer2d);

        // Vector3 PosicaoCamera2d = Camera.main.WorldToScreenPoint(Camera.main.transform.position);
        // Debug.Log("PosicaoCamera2d");
        // Debug.Log(PosicaoCamera2d);
        // Debug.Log("PosicaoPosicaoInimigoOuPlayer2d");
        // Debug.Log(PosicaoPosicaoInimigoOuPlayer2d);
        // DbDebugger.DebugObject(PosicaoCamera);
    }

}
