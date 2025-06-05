using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public GameObject opcoesUI;
    private bool isPaused = false;

    void Awake()
    {
        DontDestroyOnLoad(gameObject); // Não destruir ao trocar de cena
        opcoesUI.SetActive(false);     // Começa escondido
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        opcoesUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void Pause()
    {
        opcoesUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void LoadMenuPrincipal()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("CenaPrincipalD"); // Nome da sua cena principal
    }
}
