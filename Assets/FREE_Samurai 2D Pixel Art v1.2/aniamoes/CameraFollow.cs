using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform Target; // o jogador
    public float followSpeed = 5f;

    private void LateUpdate()
    {
        if (Target == null) return;

        Vector3 oCameraPosicao = transform.position;

        Vector3 oTargetPosicao = new Vector3(
            Target.position.x,
            Target.position.y,
            oCameraPosicao.z
        );

        // A ideia é que a câmera siga o 'Target'
        // Bom, é possível a gente só colocar a posição da câmera como a posição do 'Target':

        // Vector3 oNovaCameraPosicao = oTargetPosicao;

        // porém, às vezes acontecem alguns tremores na tela. E o Unity já sabendo disso,
        // tem uma função que leva GRADUALMENTE de um ponto inicial A para um ponto final b.
        // O último parâmetro é esse gradiente. A regra é:
        // 0 : ele não segue a câmera
        // 1 : ele segue a câmera e não é absolutamente nada gradual.
        // range(0, 1) : gradual. É o meio mais ou menos que é o ideal.
        // No nosso caso, o ponto inicial será a posição da câmera e o ponto final será a posição do 'Target'

        // Vector3 oNovaCameraPosicao = Vector3.Lerp(
        //     oCameraPosicao,
        //     oTargetPosicao,
        //     0.78f
        // );

        // Bom, ainda sim ocorrem tremores. Então, como medida desesperada, consideraremos
        // a diferença de tempo do último frame para o frame atual quando definimos o gradiente

        var nDiferencaFrameAnteriorEAtual = Time.deltaTime;
        var nGradiente = followSpeed * nDiferencaFrameAnteriorEAtual;

        Vector3 oNovaCameraPosicao = Vector3.Lerp(
            oCameraPosicao,
            oTargetPosicao,
            nGradiente 
        );

        // Caramba, ainda ocorrem tremores. A último coisa que faltou considerar foram os pixels que utilizamos.
        // Como o jogo é apenas 2D, modificaremos o 'x' e o 'y' apenas.

        float nPixelPorUnidade = 0.03125f; // 1f / 32f

        var nValorAnterior = oNovaCameraPosicao.x;


        // Retorna o número INTEIRO mais próximo. Ex: 99.023482343897 -> 99
        oNovaCameraPosicao.x = Mathf.Round(oNovaCameraPosicao.x / nPixelPorUnidade) * nPixelPorUnidade;
        oNovaCameraPosicao.y = Mathf.Round(oNovaCameraPosicao.y / nPixelPorUnidade) * nPixelPorUnidade;


        transform.position = oNovaCameraPosicao;
    }
}
