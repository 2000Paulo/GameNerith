using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class PauseManager : MonoBehaviour
{
    public GameObject opcoesUI;
    private bool isPaused = false;

    private static PauseManager instance;

    void Awake()
    {
        // Garante que só exista um PauseManager
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;

        if (opcoesUI != null)
            opcoesUI.SetActive(false);
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Destroi se voltar pro menu principal (opcional)
        if (scene.name == "CenaPrincipalD")
        {
            Destroy(gameObject);
        }

        // Tenta reassociar UI se necessário
        if (opcoesUI == null)
        {
            opcoesUI = GameObject.Find("OpcoesUI"); // ou o nome do seu painel
        }

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

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked; // trava cursor no jogo
    }

    public void Pause()
    {
        if (opcoesUI != null)
            opcoesUI.SetActive(true);

        Time.timeScale = 0f;
        isPaused = true;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        // Garante que o EventSystem esteja presente e ativo
        if (EventSystem.current == null)
        {
            GameObject es = new GameObject("EventSystem");
            es.AddComponent<EventSystem>();
            es.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();
        }
    }

    public void LoadMenuPrincipal()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("CenaPrincipalD");
    }
}
