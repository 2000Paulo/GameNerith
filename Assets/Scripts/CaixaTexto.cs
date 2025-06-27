using UnityEngine;

public class CaixaTexto : MonoBehaviour
{
    public Sprite[] letras; // Letras em ordem A-Z, espaço, ponto etc.
    public GameObject letraPrefab; // Prefab chamado Letra
    public Transform textoPai; // Objeto filho chamado Texto

    public void MostrarTexto(string mensagem)
    {
        foreach (Transform filho in textoPai)
        {
            Destroy(filho.gameObject);
        }

        float posX = 0f;
        foreach (char caractere in mensagem.ToUpper())
        {
            int indice = ObterIndice(caractere);
            if (indice == -1) continue;

            GameObject novaLetra = Instantiate(letraPrefab, textoPai);
            novaLetra.GetComponent<SpriteRenderer>().sprite = letras[indice];
            novaLetra.transform.localPosition = new Vector3(posX, 0f, 0f);
            posX += 0.32f; // espaçamento
        }
    }

    int ObterIndice(char caractere)
    {
        if (caractere >= 'A' && caractere <= 'Z')
            return caractere - 'A';
        if (caractere == ' ') return 26;
        if (caractere == '.') return 27;
        return -1;
    }
}
