using UnityEngine;

public class AttackingRange : MonoBehaviour
{
    EnemyScript enem;
    BoxCollider box;
    void Start()
    {
        enem = GetComponentInParent<EnemyScript>();
        box = GetComponent<BoxCollider>();
    }
    private void OnTriggerStay(Collider other)
    {
        enem.attackRange = true;
    }
    private void OnTriggerExit(Collider other)
    {
        enem.attackRange = false;
    }
}
