using UnityEngine;

public class CameraDontDestroy : MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
