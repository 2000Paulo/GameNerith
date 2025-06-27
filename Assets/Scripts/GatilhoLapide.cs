using UnityEngine;

public class GatilhoLapide : MonoBehaviour
{
    public CaixaTexto caixaTexto;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            caixaTexto.MostrarTexto("DESCANSE EM PAZ");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            caixaTexto.MostrarTexto("");
        }
    }
}
