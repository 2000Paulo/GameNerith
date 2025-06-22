using System.Collections;
using UnityEngine;

public class TrapFall : MonoBehaviour
{
    public Sprite[] fallSprites; // 5 sprites do desmoronamento
    public AudioSource somDaPedra; // ← aqui está o audio source
    private SpriteRenderer sr;
    private bool triggered = false;

    private Vector3 originalPos;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        originalPos = transform.localPosition;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (!triggered && col.gameObject.CompareTag("Player"))
        {
            triggered = true;
            StartCoroutine(FallSequence());
        }
    }

    IEnumerator FallSequence()
    {
        // 🎵 Toca o som no início do tremor
        if (somDaPedra != null)
        {
            somDaPedra.Play();
        }

        // Etapa 1: Tremor por 2 segundos
        float shakeDuration = 2f;
        float elapsed = 0f;
        float shakeAmount = 0.05f;

        while (elapsed < shakeDuration)
        {
            Vector3 randomOffset = Random.insideUnitCircle * shakeAmount;
            transform.localPosition = originalPos + randomOffset;
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalPos;

        // Etapa 2: Troca de sprites (desmoronando)
        for (int i = 0; i < fallSprites.Length; i++)
        {
            sr.sprite = fallSprites[i];
            yield return new WaitForSeconds(0.1f);
        }

        // Etapa 3: Some / desativa
        GetComponent<Collider2D>().enabled = false;
        sr.enabled = false;
    }
}
