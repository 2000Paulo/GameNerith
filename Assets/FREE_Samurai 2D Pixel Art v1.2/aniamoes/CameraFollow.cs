using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform Target;       // O jogador
    public float followSpeed = 5f; // Velocidade de seguimento
    public Vector2 offset = new Vector2(0, 1.5f); // Offset para subir a câmera no eixo Y
    public float pixelsPerUnit = 32f; // PPU do seu projeto

    private void LateUpdate()
    {
        if (Target == null) return;

        // Posição alvo com offset (por exemplo, subir a câmera)
        Vector3 targetPosition = new Vector3(
            Target.position.x + offset.x,
            Target.position.y + offset.y,
            transform.position.z // Mantém o Z da câmera
        );

        // Suavidade baseada no tempo entre frames
        float t = followSpeed * Time.deltaTime;

        // Interpola a posição atual até a posição alvo
        Vector3 smoothPosition = Vector3.Lerp(transform.position, targetPosition, t);

        // Pixel perfect rounding (x e y somente)
        float unit = 1f / pixelsPerUnit;

        smoothPosition.x = Mathf.Round(smoothPosition.x / unit) * unit;
        smoothPosition.y = Mathf.Round(smoothPosition.y / unit) * unit;

        // Aplica a posição suavizada e ajustada
        transform.position = smoothPosition;
    }
}
