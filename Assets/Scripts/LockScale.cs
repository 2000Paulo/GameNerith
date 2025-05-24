using UnityEngine;

[ExecuteAlways]
public class LockScale : MonoBehaviour
{
    public Vector3 lockedScale = Vector3.one;

    void Update()
    {
        if (transform.localScale != lockedScale)
        {
            transform.localScale = lockedScale;
        }
    }

    void OnValidate()
    {
        transform.localScale = lockedScale;
    }
}
