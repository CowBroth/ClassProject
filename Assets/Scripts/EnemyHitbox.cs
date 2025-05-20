using UnityEngine;

public class EnemyHitbox : MonoBehaviour
{
    public MovementScript playerScript;
    EnemyScript enemyScript;
    bool blocking;
    private void Start()
    {
        enemyScript = GetComponentInParent<EnemyScript>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (playerScript.optionEnumInt != 1)
            {
                playerScript.PlayerHit();
                print("damage");
            }
            if (playerScript.optionEnumInt == 1)
            {
                if (playerScript.parrying)
                {
                    enemyScript.OnStun(1);
                    playerScript.OnParry();
                    print("parried!");
                }
                else
                {
                    print("blocked");
                }
            }
        }
    }
}
