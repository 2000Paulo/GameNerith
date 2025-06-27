using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Estado do Jogador")]
    public int vidaAtual = 100;
    public int vidaMaxima = 100;
    public int pontuacaoAtual = 0;

    public string proximoSpawnPoint;

    [Header("UI")]
    public GameObject gameOverUI;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Persiste entre cenas
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject); // Evita múltiplos GameManagers
            return;
        }
    }

    void Start()
    {
        Time.timeScale = 1f;
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

        // Se voltar para o menu, destrói o GameManager
        if (scene.name == "CenaPrincipalD")
        {
            Destroy(gameObject);
        }
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void AdicionarPontos(int pontos)
    {
        pontuacaoAtual += pontos;
    }

    public void ReduzirVida(int dano)
    {
        vidaAtual = Mathf.Clamp(vidaAtual - dano, 0, vidaMaxima);
    }

    public void CurarVida(int cura)
    {
        vidaAtual = Mathf.Clamp(vidaAtual + cura, 0, vidaMaxima);
    }

    public void GameOver()
    {
        Time.timeScale = 0.0001f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (gameOverUI != null)
        {
            gameOverUI.SetActive(true);
        }
        else
        {
            Debug.LogError("⚠️ ERRO: Game Over UI não foi encontrado pelo GameManager!");
        }
    }

    public void VoltarMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("CenaPrincipalD");
        // O OnSceneLoaded cuidará de destruir este GameManager
    }
}
