using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PortalFinal : MonoBehaviour
{
    [Header("Referências")]
    public GameObject Canvas_RankingParabens;
    public TMP_InputField inputNome;
    public TMP_Text textoPontuacao;

    private bool jogadorFinalizou = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!jogadorFinalizou && other.CompareTag("Player"))
        {
            jogadorFinalizou = true;
            ChecarRankingFinal();
        }
    }

    void ChecarRankingFinal()
    {
        int pontosFinais = GameManager.instance.pontuacaoAtual;

        RankingManager rm = FindObjectOfType<RankingManager>();

        if (rm.EntrouNoRanking(pontosFinais))
        {
            Canvas_RankingParabens.SetActive(true);
            textoPontuacao.text = " " + pontosFinais;
        }
        else
        {
            // Se não entrou, destrói o GameManager e volta para o menu
            Destroy(GameManager.instance.gameObject);
            SceneManager.LoadScene("CenaPrincipalD");
        }
    }

    public void SalvarRankingEVoltar()
    {
        string nomeJogador = inputNome.text;
        int pontos = GameManager.instance.pontuacaoAtual;

        if (string.IsNullOrWhiteSpace(nomeJogador))
            nomeJogador = "Sem Nome";

        RankingManager rm = FindObjectOfType<RankingManager>();
        rm.SalvarPontuacao(nomeJogador, pontos);

        // Destrói o GameManager para evitar reuso de dados na próxima partida
        Destroy(GameManager.instance.gameObject);

        // Volta para o menu principal
        SceneManager.LoadScene("CenaPrincipalD");
    }
}
