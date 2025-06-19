using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public string proximoSpawnPoint;

    [Header("UI")]
    public GameObject gameOverUI;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        // Garante que o jogo volte ao tempo normal
        Time.timeScale = 1f;

        // Garante que o cursor esteja visível
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GameObject ui = GameObject.Find("Canvas_GameOver");
        if (ui != null)
        {
            gameOverUI = ui;
            gameOverUI.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Canvas_GameOver não encontrado na cena: " + scene.name);
        }
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void GameOver()
    {
        // Pausa o jogo suavemente sem travar completamente
        Time.timeScale = 0.0001f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (gameOverUI != null)
            gameOverUI.SetActive(true);
        else
            Debug.LogWarning("Game Over UI não está conectada no GameManager!");
    }

    public void JogarNovamente()
    {
        Time.timeScale = 1f;

        if (gameOverUI != null)
            gameOverUI.SetActive(false);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void VoltarMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("CenaPrincipalD");
    }
}
