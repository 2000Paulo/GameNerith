using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuPrincipal : MonoBehaviour
{
    [Header("Menus")]
    public GameObject menuPrincipal;
    public GameObject menuOpcoes;

    [Header("Áudio")]
    public AudioSource musicSource;
    public Slider volumeSlider;

    void Start()
    {
        // Começa com o menu principal ativo e opções desativado
        menuPrincipal.SetActive(true);
        menuOpcoes.SetActive(false);

        // Configura o slider de volume
        if (musicSource != null && volumeSlider != null)
        {
            volumeSlider.value = musicSource.volume;
            volumeSlider.onValueChanged.AddListener(SetVolume);
        }
    }

    public void Jogar()
    {
        Time.timeScale = 1f; // Aqui resolve o bug!
        SceneManager.LoadScene("SampleScene");
    }

    public void Sair()
    {
        Application.Quit();
        Debug.Log("Saiu do jogo."); // só aparece no editor
    }

    public void AbrirOpcoes()
    {
        menuPrincipal.SetActive(false);
        menuOpcoes.SetActive(true);
    }

    public void VoltarParaMenuPrincipal()
    {
        menuPrincipal.SetActive(true);
        menuOpcoes.SetActive(false);
    }

    public void SetVolume(float volume)
    {
        if (musicSource != null)
        {
            musicSource.volume = volume;
        }
    }
}
