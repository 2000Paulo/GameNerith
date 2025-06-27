using UnityEngine;

public class Feno : MonoBehaviour
{
    private Vector3 posicaoOriginal;
    private Transform jogadorDentro = null;
    private Rigidbody2D jogadorRb;
    private bool tremendo = false;

    [Header("Som de Feno")]
    public AudioSource fenoAudio;

    private void Start()
    {
        posicaoOriginal = transform.localPosition;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerDamageReceiver receiver = other.GetComponent<PlayerDamageReceiver>();
            if (receiver != null)
            {
                receiver.StartCoroutine(TemporariamenteInvulneravel(receiver));
            }

            jogadorDentro = other.transform;
            jogadorRb = other.GetComponent<Rigidbody2D>();
            StartCoroutine(TremerSeMover());
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.transform == jogadorDentro)
        {
            jogadorDentro = null;
            jogadorRb = null;
            tremendo = false;
            transform.localPosition = posicaoOriginal;

            // 🔇 Para o som
            if (fenoAudio != null && fenoAudio.isPlaying)
                fenoAudio.Stop();
        }
    }

    private System.Collections.IEnumerator TemporariamenteInvulneravel(PlayerDamageReceiver receiver)
    {
        receiver.estaInvulneravel = true;
        Debug.Log("Invulnerabilidade ativada por 2 segundos.");
        yield return new WaitForSeconds(2f);
        receiver.estaInvulneravel = false;
        Debug.Log("Invulnerabilidade desativada.");
    }

    private System.Collections.IEnumerator TremerSeMover()
    {
        tremendo = true;
        while (tremendo)
        {
            if (jogadorDentro != null && jogadorRb != null && jogadorRb.linearVelocity.magnitude > 0.1f)
            {
                float offsetX = Mathf.Sin(Time.time * 10f) * 0.02f;
                float offsetY = Mathf.Cos(Time.time * 12f) * 0.015f;
                transform.localPosition = posicaoOriginal + new Vector3(offsetX, offsetY, 0f);

                // 🔊 Toca o som se ainda não estiver tocando
                if (fenoAudio != null && !fenoAudio.isPlaying)
                    fenoAudio.Play();
            }
            else
            {
                transform.localPosition = posicaoOriginal;

                // 🔇 Para o som
                if (fenoAudio != null && fenoAudio.isPlaying)
                    fenoAudio.Stop();
            }

            yield return null;
        }
    }
}
