using System.Collections;
using UnityEngine;

public class PortaoMovel : MonoBehaviour
{
    public float altura = -4f;       // Quantos blocos vai subir
    public float duracao = 1.5f;    // Tempo para a animação completa

    private Vector3 posicaoFechada;
    private Vector3 posicaoAberta;
    private bool aberto = false;
    private bool emMovimento = false;

    void Start()
    {
        posicaoFechada = transform.position;
        posicaoAberta = posicaoFechada + Vector3.up * altura;
    }

    public void AlternarPortao()
    {
        if (emMovimento) return;

        Vector3 destino = aberto ? posicaoFechada : posicaoAberta;
        StartCoroutine(MoverPortao(destino));
        aberto = !aberto;
    }

    IEnumerator MoverPortao(Vector3 destino)
    {
        emMovimento = true;
        Vector3 origem = transform.position;
        float tempo = 0f;

        while (tempo < duracao)
        {
            tempo += Time.deltaTime;
            float t = Mathf.Clamp01(tempo / duracao);
            transform.position = Vector3.Lerp(origem, destino, t);
            yield return null;
        }

        transform.position = destino;
        emMovimento = false;
    }
}
