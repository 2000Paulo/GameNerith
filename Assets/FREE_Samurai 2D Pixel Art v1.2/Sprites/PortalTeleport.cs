using UnityEngine;
using System.Collections.Generic;

public class PortalTeleport : MonoBehaviour
{
    public Transform destinationPortal;
    public string targetTag = "Player";
    public float teleportCooldown = 1f;
    public float offsetDistance = 2f;

    private Dictionary<GameObject, float> cooldowns = new Dictionary<GameObject, float>();

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(targetTag)) return;

        GameObject obj = other.gameObject;

        // Ignora se estiver em cooldown
        if (cooldowns.ContainsKey(obj) && Time.time < cooldowns[obj]) return;

        // Teleporta com offset baseado na direção que o destino está apontando
        Vector3 direction = destinationPortal.right.normalized;
        other.transform.position = destinationPortal.position + direction * offsetDistance;

        // Aplica cooldown
        cooldowns[obj] = Time.time + teleportCooldown;
    }
}
