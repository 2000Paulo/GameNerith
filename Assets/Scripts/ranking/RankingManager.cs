using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class EntradaRanking
{
    public string nome;
    public int pontos;
}

[System.Serializable]
public class Ranking
{
    public List<EntradaRanking> entradas = new List<EntradaRanking>();
}

public class RankingManager : MonoBehaviour
{
    private string caminhoArquivo;
    public int tamanhoMaximo = 8;

    void Awake()
    {
        caminhoArquivo = Application.persistentDataPath + "/ranking.json";
    }

    public Ranking CarregarRanking()
    {
        if (File.Exists(caminhoArquivo))
        {
            string json = File.ReadAllText(caminhoArquivo);
            return JsonUtility.FromJson<Ranking>(json);
        }
        else
        {
            return new Ranking(); // Ranking vazio
        }
    }

    public void SalvarPontuacao(string nome, int pontos)
    {
        Ranking ranking = CarregarRanking();

        ranking.entradas.Add(new EntradaRanking { nome = nome, pontos = pontos });

        ranking.entradas = ranking.entradas
            .OrderByDescending(e => e.pontos)
            .Take(tamanhoMaximo)
            .ToList();

        string json = JsonUtility.ToJson(ranking, true);
        File.WriteAllText(caminhoArquivo, json);
    }

    public bool EntrouNoRanking(int pontos)
    {
        Ranking ranking = CarregarRanking();

        if (ranking.entradas.Count < tamanhoMaximo)
            return true;

        int menorPontuacao = ranking.entradas.Min(e => e.pontos);
        return pontos > menorPontuacao;
    }

    public List<EntradaRanking> ObterRankingOrdenado()
    {
        return CarregarRanking()
            .entradas
            .OrderByDescending(e => e.pontos)
            .ToList();
    }
}
