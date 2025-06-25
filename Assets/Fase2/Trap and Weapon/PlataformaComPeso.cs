using System.Collections;
using UnityEngine;

public class PlataformaComPeso : MonoBehaviour
{
    public float deslocamentoY = 0.2f;          // Quanto a plataforma desce
    public float duracaoDescer = 0.2f;          // Tempo para descer
    public float duracaoSubir = 0.3f;           // Tempo para voltar

    private Vector3 posicaoOriginal;
    private bool emUso = false;

    void Start()
    {
        posicaoOriginal = transform.position;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!emUso && collision.collider.CompareTag("Player"))
        {
            StartCoroutine(AbaixarPlataforma());
        }
    }

    IEnumerator AbaixarPlataforma()
    {
        emUso = true;

        Vector3 alvo = posicaoOriginal + Vector3.down * deslocamentoY;
        float tempo = 0f;

        while (tempo < duracaoDescer)
        {
            tempo += Time.deltaTime;
            transform.position = Vector3.Lerp(posicaoOriginal, alvo, tempo / duracaoDescer);
            yield return null;
        }

        yield return new WaitForSeconds(0.1f); // Pequena pausa antes de subir

        tempo = 0f;
        while (tempo < duracaoSubir)
        {
            tempo += Time.deltaTime;
            transform.position = Vector3.Lerp(alvo, posicaoOriginal, tempo / duracaoSubir);
            yield return null;
        }

        transform.position = posicaoOriginal;
        emUso = false;
    }
}
