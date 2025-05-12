using UnityEngine;

public class ShurikenMovel : MonoBehaviour
{
    public Vector2 direcao = Vector2.up;     // Direção do movimento: Vector2.up = sobe e desce
    public float distancia = 3f;             // Distância total que ela percorre
    public float velocidade = 2f;            // Velocidade do movimento

    private Vector3 pontoA;
    private Vector3 pontoB;
    private Vector3 destino;

    void Start()
    {
        pontoA = transform.position;
        pontoB = pontoA + (Vector3)(direcao.normalized * distancia);
        destino = pontoB;
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, destino, velocidade * Time.deltaTime);

        if (Vector3.Distance(transform.position, destino) < 0.05f)
        {
            destino = destino == pontoA ? pontoB : pontoA;
        }
    }
}
