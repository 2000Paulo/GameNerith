using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public string proximoSpawnPoint;

    void Awake()
    {
        // Singleton: só 1 GameManager no jogo
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
