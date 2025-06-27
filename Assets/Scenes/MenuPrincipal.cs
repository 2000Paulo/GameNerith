using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MenuPrincipal : MonoBehaviour
{
    [Header("Menus")]
    public GameObject menuPrincipal;
    public GameObject menuOpcoes;
    public GameObject canvasRanking;

    [Header("Ranking UI")]
    public TMP_Text[] camposNomes;
    public TMP_Text[] camposPontos;

    [Header("Áudio")]
    public AudioSource musicSource;
    public Slider volumeSlider;

    void Start()
    {
        menuPrincipal.SetActive(true);
        menuOpcoes.SetActive(false);
        canvasRanking.SetActive(false);

        if (musicSource != null && volumeSlider != null)
        {
            volumeSlider.value = musicSource.volume;
            volumeSlider.onValueChanged.AddListener(SetVolume);
        }

        // Preenche automaticamente campos de ranking via caminho relativo dentro do Canvas
        camposNomes = new TMP_Text[7];
        camposPontos = new TMP_Text[7];

        for (int i = 0; i < 7; i++)
        {
            string nomeId = "PainelRanking/Nome" + (i + 1);
            string pontoId = "PainelRanking/Ponto" + (i + 1);

            var nomeObj = canvasRanking.transform.Find(nomeId);
            var pontoObj = canvasRanking.transform.Find(pontoId);

            if (nomeObj != null)
                camposNomes[i] = nomeObj.GetComponent<TMP_Text>();
            if (pontoObj != null)
                camposPontos[i] = pontoObj.GetComponent<TMP_Text>();

            Debug.Log($"[AutoSetup] {nomeId} encontrado: {nomeObj != null}, {pontoId} encontrado: {pontoObj != null}");
        }
    }

    public void Jogar()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("SampleScene");
    }

    public void Sair()
    {
        Application.Quit();
        Debug.Log("Saiu do jogo.");
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
        canvasRanking.SetActive(false);
    }

    public void AbrirRanking()
    {
        menuPrincipal.SetActive(false);
        canvasRanking.SetActive(true);
        MostrarRanking();
    }

    private void MostrarRanking()
    {
        RankingManager rm = FindObjectOfType<RankingManager>();
        if (rm == null)
        {
            Debug.LogError("❌ RankingManager não encontrado na cena!");
            return;
        }

        var ranking = rm.ObterRankingOrdenado();
        Debug.Log("✅ Entradas carregadas: " + ranking.Count);

        for (int i = 0; i < camposNomes.Length; i++)
        {
            if (camposNomes[i] == null || camposPontos[i] == null)
            {
                Debug.LogError($"❌ TMP_Text ausente no índice {i} (Nome ou Ponto).");
                continue;
            }

            if (i < ranking.Count)
            {
                Debug.Log($"✔️ Preenchendo posição {i + 1}: {ranking[i].nome} - {ranking[i].pontos}");
                camposNomes[i].text = ranking[i].nome;
                camposPontos[i].text = ranking[i].pontos.ToString();
            }
            else
            {
                camposNomes[i].text = "-";
                camposPontos[i].text = "-";
            }
        }
    }

    public void FecharRanking()
    {
        canvasRanking.SetActive(false);
        menuPrincipal.SetActive(true);
    }

    public void SetVolume(float volume)
    {
        if (musicSource != null)
        {
            musicSource.volume = volume;
        }
    }
}
