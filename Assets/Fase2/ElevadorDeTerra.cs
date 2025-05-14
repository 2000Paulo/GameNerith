using System.Collections;
using UnityEngine;

public class ElevadorDeTerra : MonoBehaviour
{
    public int alturaMaxima = 15;
    public float intervalo = 1.5f;
    public float velocidadeSubida = 2f;

    private int alturaAtual = 0;
    private Vector3 posicaoInicial;
    private Transform jogadorEmCima = null;
    private Coroutine rotinaElevador;

    void Start()
    {
        posicaoInicial = transform.position;
    }

    IEnumerator ControlarElevador()
    {
        while (alturaAtual < alturaMaxima)
        {
            Vector3 inicio = transform.position;
            Vector3 destino = inicio + Vector3.up;

            float tempo = 0f;
            while (tempo < 1f)
            {
                tempo += Time.deltaTime * velocidadeSubida;
                transform.position = Vector3.Lerp(inicio, destino, tempo);
                yield return null;
            }

            alturaAtual++;
            yield return new WaitForSeconds(intervalo);
        }
    }

    IEnumerator EsperarEIniciarElevador()
    {
        yield return new WaitForSeconds(intervalo);

        if (jogadorEmCima != null)
        {
            rotinaElevador = StartCoroutine(ControlarElevador());
        }
    }

    // SUBSTITUIR TRIGGER POR COLISÃO
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            jogadorEmCima = collision.transform;
            jogadorEmCima.SetParent(transform);
            StartCoroutine(EsperarEIniciarElevador());
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player") && jogadorEmCima != null)
        {
            StartCoroutine(SoltarPlayerNoProximoFrame());

            if (rotinaElevador != null)
            {
                StopCoroutine(rotinaElevador);
                rotinaElevador = null;
            }
        }
    }

    IEnumerator SoltarPlayerNoProximoFrame()
    {
        yield return null;

        if (jogadorEmCima != null)
        {
            jogadorEmCima.SetParent(null);
            jogadorEmCima = null;
        }
    }
}
