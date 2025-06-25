using UnityEngine;

public class AttackHitbox : MonoBehaviour
{
    public int damage = 10;

    private void OnTriggerEnter2D(Collider2D other)
    {
        var player = other.GetComponent<PlayerDamageReceiver>();
        if (player != null)
        {
            player.ApplyDamage(damage, "ataque");
        }
    }
}
