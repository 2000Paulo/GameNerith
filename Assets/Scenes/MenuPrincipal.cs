using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPrincipal : MonoBehaviour
{
    public void Jogar()
    {
        // Aqui você pode usar o nome da cena ou o índice (1 = SampleScene)
        SceneManager.LoadScene("SampleScene");
        // ou: SceneManager.LoadScene(1);
    }

    public void Sair()
    {
        Application.Quit();
        Debug.Log("Saiu do jogo."); // só aparece no editor
    }
}
