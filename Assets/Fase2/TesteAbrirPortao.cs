using UnityEngine;

public class TesteAbrirPortao : MonoBehaviour
{
    public PortaoMovel portao;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            portao.AlternarPortao();
        }
    }
}
