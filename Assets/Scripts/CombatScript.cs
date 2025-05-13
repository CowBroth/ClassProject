using System.Collections;
using UnityEngine;

public class CombatScript : MonoBehaviour
{
    public GameObject attackHitbox;
    public MovementScript movementScript;
    void Start()
    {
        movementScript = GetComponentInParent<MovementScript>();
    }
    public void AttackStart()
    {
        attackHitbox.SetActive(true);
    }
    public void AttackEnd()
    {
        attackHitbox.SetActive(false);
    }
    public void ParryMethod()
    {
        StartCoroutine(ParryTimer());
    }
    public IEnumerator ParryTimer()
    {
        float t = 0.10f;
        movementScript.parrying = true;
        yield return new WaitForSeconds(t);
        movementScript.parrying = false;
    }
}