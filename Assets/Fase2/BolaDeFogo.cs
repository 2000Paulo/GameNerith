using UnityEngine;

public class BolaDeFogo : MonoBehaviour
{
    public float velocidade = 3f;
    private bool deveCair = false;

    private void OnEnable()
    {
        // Garante que a bola não caia quando ativar sem intenção
        deveCair = false;
    }

    private void Update()
    {
        if (deveCair)
        {
            transform.position += Vector3.down * velocidade * Time.deltaTime;
        }
    }

    public void Ativar()
    {
        Debug.Log($"{name} foi ativada e vai começar a cair.");
        deveCair = true;
    }

    public void Resetar()
    {
        Debug.Log($"{name} resetada.");
        deveCair = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log($"{name} colidiu com: {collision.gameObject.name}");

        if (collision.CompareTag("Chao"))
        {
            Debug.Log($"{name} tocou o chão e foi desativada.");
            gameObject.SetActive(false); // Desativa ao tocar o chão
        }
    }
}
