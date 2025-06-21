using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMoverParaSpawn : MonoBehaviour
{
    void Awake()
    {
        // Escuta o carregamento de cena (não precisa DontDestroyOnLoad)
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        // Remove o listener para evitar erros ao sair da cena
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Start()
    {
        // Garante que será movido ao carregar a cena pela primeira vez
        MoverParaSpawn();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        MoverParaSpawn();
    }

    private void MoverParaSpawn()
    {
        string spawnName = GameManager.instance?.proximoSpawnPoint;

        if (!string.IsNullOrEmpty(spawnName))
        {
            GameObject spawnPoint = GameObject.Find(spawnName);

            if (spawnPoint != null)
            {
                transform.position = spawnPoint.transform.position;
                Debug.Log($"Player movido para o SpawnPoint '{spawnName}' na cena '{SceneManager.GetActiveScene().name}'.");
            }
            else
            {
                Debug.LogWarning($"SpawnPoint '{spawnName}' não encontrado na cena '{SceneManager.GetActiveScene().name}'!");
            }
        }
        else
        {
            Debug.Log("Nenhum spawn definido no GameManager. Player não será movido.");
        }
    }
}
