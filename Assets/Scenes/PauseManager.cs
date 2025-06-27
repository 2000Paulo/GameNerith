using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class PauseManager : MonoBehaviour
{
    public GameObject opcoesUI;
    public GameObject comandos; // 👈 painel de comandos (imagem)
    private bool isPaused = false;

    private static PauseManager instance;

    void Awake()
    {
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

        if (comandos != null)
            comandos.SetActive(false);
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "CenaPrincipalD")
        {
            Destroy(gameObject);
        }

        if (opcoesUI == null)
            opcoesUI = GameObject.Find("OpcoesUI");

        Time.timeScale = 1f;
        isPaused = false;

        if (opcoesUI != null)
            opcoesUI.SetActive(false);

        if (comandos != null)
            comandos.SetActive(false);
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

        if (comandos != null)
            comandos.SetActive(false); // 👈 também esconde comandos

        Time.timeScale = 1f;
        isPaused = false;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void Pause()
    {
        if (opcoesUI != null)
            opcoesUI.SetActive(true);

        Time.timeScale = 0f;
        isPaused = true;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

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

    // 👇 Chamado pelo botão "Listar Comandos"
    public void MostrarComandos()
    {
        if (comandos != null)
            comandos.SetActive(true);
    }
}
