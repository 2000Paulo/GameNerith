using UnityEngine;
using UnityEngine.SceneManagement;

public class TransicaoDeFase : MonoBehaviour
{
    public string nomeDaFaseDestino = "Fase2";

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(nomeDaFaseDestino);
        }
    }
}
