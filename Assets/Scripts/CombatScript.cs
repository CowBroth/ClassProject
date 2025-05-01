using UnityEngine;

public class CombatScript : MonoBehaviour
{
    public GameObject attackHitbox;
    public void AttackStart()
    {
        attackHitbox.SetActive(true);
    }
    public void AttackEnd()
    {
        attackHitbox.SetActive(false);
    }
}
