using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public GameObject opcoesUI;
    private bool isPaused = false;

    void Awake()
    {
        // Evita múltiplos PauseManagers
        if (Object.FindObjectsByType<PauseManager>(FindObjectsSortMode.None).Length > 1)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        opcoesUI.SetActive(false);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Destroi o PauseManager ao voltar pro menu principal
        if (scene.name == "CenaPrincipalD")
        {
            Destroy(gameObject);
        }

        // Garante que o jogo volte ao normal ao carregar cena nova
        Time.timeScale = 1f;
        isPaused = false;
        if (opcoesUI != null)
            opcoesUI.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                Resume();
            else
                Pause();
        }
    }

    public void Resume()
    {
        if (opcoesUI != null)
            opcoesUI.SetActive(false);

        Time.timeScale = 1f;
        isPaused = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Pause()
    {
        if (opcoesUI != null)
            opcoesUI.SetActive(true);

        Time.timeScale = 0f;
        isPaused = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void LoadMenuPrincipal()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("CenaPrincipalD");
    }
}
