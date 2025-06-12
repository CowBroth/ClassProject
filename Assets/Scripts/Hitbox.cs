using UnityEngine;

public class Hitbox : MonoBehaviour
{
    public int damage = 0;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            if (damage == 0)
            {
                other.GetComponent<EnemyScript>().OnStun(0, 0);
            }
            if (damage == 1)
            {
                other.GetComponent<EnemyScript>().OnStun(0, 1);
            }
        }
    }
}
