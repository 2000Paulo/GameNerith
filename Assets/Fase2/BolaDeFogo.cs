using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class BolaDeFogo : MonoBehaviour
{
    public float velocidade = 3f;
    private bool deveCair = false;

    private Rigidbody2D rb;

    private bool colisaoHabilitada = false; // <- Controle de delay de colisão

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0; // desativa a gravidade automática
    }

    private void OnEnable()
    {
        deveCair = false;
        colisaoHabilitada = false;
        rb.linearVelocity = Vector2.zero;
    }

    private void Update()
    {
        if (deveCair)
        {
            rb.linearVelocity = Vector2.down * velocidade;
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }
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
        yield return null; // espera 1 frame
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
            return; // ainda não habilitou colisão, ignora

        Debug.Log($"{name} colidiu com: {collision.gameObject.name}");

        // Colisão com PLAYER
        if (collision.CompareTag("Player"))
        {
            Debug.Log($"{name} atingiu o PLAYER e foi desativada.");
            gameObject.SetActive(false);
            return;
        }

        // Colisão com chão trigger (ex: seu elevador ou chão)
        if (collision.isTrigger && collision.gameObject.layer == LayerMask.NameToLayer("Default"))
        {
            Debug.Log($"{name} tocou o CHÃO (Trigger) e foi desativada.");
            gameObject.SetActive(false);
            return;
        }

        // Colisão com chão NÃO-TRIGGER (colisor normal)
        if (!collision.isTrigger && collision.gameObject.layer == LayerMask.NameToLayer("Default"))
        {
            Debug.Log($"{name} tocou o CHÃO (Collider normal) e foi desativada.");
            gameObject.SetActive(false);
            return;
        }
    }
}
