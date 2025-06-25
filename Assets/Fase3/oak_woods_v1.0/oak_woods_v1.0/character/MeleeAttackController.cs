using UnityEngine;
using System.Collections;

public class MeleeAttackController : MonoBehaviour
{
    public GameObject attackHitbox;
    public float attackDuration = 0.3f;

    private bool isAttacking = false;

    // Torna o método público para ser chamado de outro script
    public void TriggerAttack()
    {
        if (!isAttacking)
            StartCoroutine(ActivateAttack());
    }

    private IEnumerator ActivateAttack()
    {
        isAttacking = true;
        attackHitbox.SetActive(true);
        yield return new WaitForSeconds(attackDuration);
        attackHitbox.SetActive(false);
        isAttacking = false;
    }
}
