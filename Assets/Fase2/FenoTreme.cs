using UnityEngine;

public class FenoTreme : MonoBehaviour
{
    public float intensidade = 0.05f;
    public float duracao = 0.3f;

    private Vector3 posicaoOriginal;
    private bool tremendo = false;

    void Start()
    {
        posicaoOriginal = transform.localPosition;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(Tremer());
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && Input.GetAxis("Horizontal") != 0)
        {
            StartCoroutine(Tremer());
        }
    }

    System.Collections.IEnumerator Tremer()
    {
        if (tremendo) yield break;

        tremendo = true;
        float tempo = 0f;

        while (tempo < duracao)
        {
            Vector3 offset = Random.insideUnitCircle * intensidade;
            transform.localPosition = posicaoOriginal + offset;
            tempo += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = posicaoOriginal;
        tremendo = false;
    }
}
