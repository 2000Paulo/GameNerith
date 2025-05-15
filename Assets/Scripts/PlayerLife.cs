using UnityEngine;
using UnityEngine.UI;  

public class PlayerLife : MonoBehaviour
{
    public PlayerHealth playerHealth;    // Referência ao script PlayerHealth
    public Image lifeBar;                // Referência para a Life Bar (barra verde)
    public Image redBar;                 // Referência para a Red Bar (barra vermelha)

    void Update()
    {
        // Atualiza a barra de vida sempre que o jogador tomar dano ou se curar
        UpdateLifeBar();
    }

    // Função para atualizar as barras de vida
    void UpdateLifeBar()
    {
        // Atualiza a largura da Life Bar (barra verde) com base na vida atual do jogador
        lifeBar.fillAmount = (float)playerHealth.GetHealth() / playerHealth.maxHealth;

        // Atualiza a largura da Red Bar (barra vermelha) com base no dano recebido
        redBar.fillAmount = (float)(playerHealth.maxHealth - playerHealth.GetHealth()) / playerHealth.maxHealth;
    }
}
