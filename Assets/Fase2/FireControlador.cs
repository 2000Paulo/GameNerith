using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FireControlador : MonoBehaviour
{
    private BolaDeFogo[] bolas;
    private Vector3[] posicoesIniciais;

    public float intervaloEntreBolas = 1f;
    public float intervaloEntreCiclos = 1f;

    private void Start()
    {
        bolas = GetComponentsInChildren<BolaDeFogo>(true);

        // Armazena as posições locais iniciais dos filhos
        posicoesIniciais = new Vector3[bolas.Length];
        for (int i = 0; i < bolas.Length; i++)
        {
            posicoesIniciais[i] = bolas[i].transform.localPosition;
        }
    }

    public void DispararTodas()
    {
        Debug.Log("🔥 DispararTodas() foi chamado!");
        StartCoroutine(LoopDisparoInfinito());
    }

    private IEnumerator LoopDisparoInfinito()
    {
        while (true)
        {
            for (int i = 0; i < bolas.Length; i++)
            {
                if (bolas[i] == null)
                    continue;

                // Primeiro: resetar e posicionar
                bolas[i].Resetar();
                bolas[i].transform.localPosition = posicoesIniciais[i];

                // Depois: ativar e iniciar movimento
                bolas[i].gameObject.SetActive(true);
                bolas[i].Ativar();

                yield return new WaitForSeconds(intervaloEntreBolas);
            }

            yield return new WaitForSeconds(intervaloEntreCiclos);
        }
    }
}
