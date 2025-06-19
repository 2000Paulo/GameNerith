using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMoverParaSpawn : MonoBehaviour
{
    void Awake()
    {
        // Agora NÃO tem DontDestroyOnLoad!
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        MoverParaSpawn();
    }

    void Start()
    {
        // Garante que será movido se essa cena for a primeira carregada
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
                Debug.Log($"Player movido para o SpawnPoint '{spawnName}' na cena.");
            }
            else
            {
                Debug.LogWarning($"SpawnPoint '{spawnName}' não encontrado na cena!");
            }
        }
    }
}
