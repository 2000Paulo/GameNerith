using System.Collections;
using UnityEngine;

public class ElevadorDeTerra : MonoBehaviour
{
    public int alturaMaxima = 10;
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

        // só inicia se o jogador ainda estiver em cima
        if (jogadorEmCima != null)
        {
            rotinaElevador = StartCoroutine(ControlarElevador());
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            jogadorEmCima = other.transform;
            jogadorEmCima.SetParent(transform);
            StartCoroutine(EsperarEIniciarElevador());
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && jogadorEmCima != null)
        {
            jogadorEmCima.SetParent(null);
            jogadorEmCima = null;

            // se o jogador sair antes de começar, cancela
            if (rotinaElevador != null)
            {
                StopCoroutine(rotinaElevador);
                rotinaElevador = null;
            }
        }
    }
}
