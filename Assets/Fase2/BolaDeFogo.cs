using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class BolaDeFogo : MonoBehaviour
{
    public float velocidade = 3f;
    private bool deveCair = false;

    private Rigidbody2D rb;
    private bool colisaoHabilitada = false;

    [Header("Dano ao jogador")]
    public int dano = 1; // Valor do dano que a bola causa

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
    }

    private void OnEnable()
    {
        deveCair = false;
        colisaoHabilitada = false;
        rb.linearVelocity = Vector2.zero;
    }

    private void Update()
    {
        rb.linearVelocity = deveCair ? Vector2.down * velocidade : Vector2.zero;
    }

    public void Ativar()
    {
        Debug.Log($"{name} foi ativada e vai começar a cair.");
        deveCair = true;
        StartCoroutine(HabilitarColisaoAposDelay());
    }

    private IEnumerator HabilitarColisaoAposDelay()
    {
        colisaoHabilitada = false;
        yield return null;
        colisaoHabilitada = true;
    }

    public void Resetar()
    {
        Debug.Log($"{name} resetada.");
        deveCair = false;
        colisaoHabilitada = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!colisaoHabilitada)
            return;

        Debug.Log($"{name} colidiu com: {collision.gameObject.name}");

        if (collision.CompareTag("Player"))
        {
            Debug.Log($"{name} atingiu o PLAYER.");

            // Aplica dano
            PlayerDamageReceiver receiver = collision.GetComponent<PlayerDamageReceiver>();
            if (receiver != null)
            {
                receiver.ApplyDamage(dano, "bola de fogo");
            }

            gameObject.SetActive(false);
            return;
        }

        if (collision.isTrigger && collision.gameObject.layer == LayerMask.NameToLayer("Default"))
        {
            Debug.Log($"{name} tocou o CHÃO (Trigger) e foi desativada.");
            gameObject.SetActive(false);
            return;
        }

        if (!collision.isTrigger && collision.gameObject.layer == LayerMask.NameToLayer("Default"))
        {
            Debug.Log($"{name} tocou o CHÃO (Collider normal) e foi desativada.");
            gameObject.SetActive(false);
            return;
        }
    }
}
