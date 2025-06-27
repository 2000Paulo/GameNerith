using UnityEngine;

public class PlataformaMovel : MonoBehaviour
{
    public Vector2 direcao = Vector2.right; // Direção do movimento
    public float distancia = 4f;            // Distância total que vai percorrer
    public float velocidade = 2f;           // Velocidade de movimento

    private Vector3 pontoInicial;
    private Vector3 pontoFinal;
    private Vector3 destinoAtual;
    private Transform jogadorEmCima = null;

    void Start()
    {
        pontoInicial = transform.position;
        pontoFinal = pontoInicial + (Vector3)(direcao.normalized * distancia);
        destinoAtual = pontoFinal;
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, destinoAtual, velocidade * Time.deltaTime);

        if (Vector3.Distance(transform.position, destinoAtual) < 0.05f)
        {
            destinoAtual = destinoAtual == pontoInicial ? pontoFinal : pontoInicial;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            jogadorEmCima = other.transform;
            jogadorEmCima.SetParent(transform);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && jogadorEmCima != null)
        {
            jogadorEmCima.SetParent(null);
            jogadorEmCima = null;
        }
    }
}
