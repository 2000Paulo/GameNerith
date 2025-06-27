using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMoverParaSpawn : MonoBehaviour
{
    void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Start()
    {
        MoverParaSpawn();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        MoverParaSpawn();
    }

    private void MoverParaSpawn()
    {
        GameObject spawnPoint = GameObject.Find("Fase2"); // <- nome certo aqui

        if (spawnPoint != null)
        {
            transform.position = spawnPoint.transform.position;
            Debug.Log($"Player movido para o objeto 'Fase2' na cena '{SceneManager.GetActiveScene().name}'.");
        }
        else
        {
            Debug.LogWarning($"Objeto 'Fase2' não encontrado na cena '{SceneManager.GetActiveScene().name}'!");
        }
    }
}
