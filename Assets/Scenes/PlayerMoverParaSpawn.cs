using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMoverParaSpawn : MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        // Registra para ser chamado quando uma cena for carregada
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        // Limpa o evento se o player for destruído
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        string spawnName = GameManager.instance?.proximoSpawnPoint;

        if (!string.IsNullOrEmpty(spawnName))
        {
            GameObject spawnPoint = GameObject.Find(spawnName);

            if (spawnPoint != null)
            {
                transform.position = spawnPoint.transform.position;
                Debug.Log($"Player foi movido para o SpawnPoint '{spawnName}' na cena {scene.name}");
            }
            else
            {
                Debug.LogWarning($"SpawnPoint '{spawnName}' não encontrado na cena {scene.name}!");
            }
        }
    }
}
