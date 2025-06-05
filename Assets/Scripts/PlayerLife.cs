using UnityEngine;
using UnityEngine.UI;  

public class PlayerLife : MonoBehaviour
{
    public PlayerHealth playerHealth;
    public Image lifeBar;
    public Image redBar;
    public float redBarSpeed = 1.5f;
    public float redBarDelay = 0.3f; // Tempo de atraso antes da barra vermelha começar a descer

    private float targetLifeAmount;
    private float redBarDelayTimer = 0f;

    void Update()
    {
        UpdateLifeBar();
    }

    void UpdateLifeBar()
    {
        targetLifeAmount = (float)playerHealth.GetHealth() / playerHealth.maxHealth;
        lifeBar.fillAmount = targetLifeAmount;

        // Se a barra verde diminuiu, inicia o timer de delay
        if (redBar.fillAmount > targetLifeAmount)
        {
            if (redBarDelayTimer < redBarDelay)
            {
                redBarDelayTimer += Time.deltaTime;
            }
            else
            {
                redBar.fillAmount -= redBarSpeed * Time.deltaTime;
                if (redBar.fillAmount < targetLifeAmount)
                    redBar.fillAmount = targetLifeAmount;
            }
        }
        else
        {
            redBar.fillAmount = targetLifeAmount;
            redBarDelayTimer = 0f; // Reseta o timer se curar ou igualar
        }
    }
}