using UnityEngine;
using UnityEngine.SceneManagement;

public class TransicaoDeFase : MonoBehaviour
{
    public string nomeDaFaseDestino = "Fase2";
    public string nomeDoSpawnPointDestino = "SpawnPoint1";

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Diz para o GameManager qual spawn usar
            GameManager.instance.proximoSpawnPoint = nomeDoSpawnPointDestino;

            Time.timeScale = 1f;
            SceneManager.LoadScene(nomeDaFaseDestino);
        }
    }
}
