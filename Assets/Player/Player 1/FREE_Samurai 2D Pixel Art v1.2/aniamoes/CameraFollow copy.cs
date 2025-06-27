// using UnityEngine;

// public class CameraFollow : MonoBehaviour
// {
//     public Transform target; // o jogador
//     public float followSpeed = 5f;

//     private void LateUpdate()
//     {
//         if (target == null) return;

//         Vector3 targetPosition = new Vector3(target.position.x, target.position.y, transform.position.z);
//         Vector3 smoothPosition = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);

//         // Snap para evitar tremores com pixel art
//         float pixelSize = 1f / 32f; // 32 = Pixels Per Unit
//         smoothPosition.x = Mathf.Round(smoothPosition.x / pixelSize) * pixelSize;
//         smoothPosition.y = Mathf.Round(smoothPosition.y / pixelSize) * pixelSize;

//         transform.position = smoothPosition;
//     }
// }
