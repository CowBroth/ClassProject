using UnityEngine;

public class CombatScript : MonoBehaviour
{
    public GameObject attackHitbox;
    public Transform attackPosition;

    public void AttackHitbox()
    {
        Instantiate(attackHitbox, attackPosition.position, gameObject.transform.rotation);
    }
}
