using UnityEngine;

public class ItemScript : MonoBehaviour
{
    public GameObject swordL1, swordL2;
    public GameObject sword;
    public Hitbox hitbox;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            SwordUpgrade();
        }
    }
    void SwordUpgrade()
    {
        hitbox.damage = 1;
        swordL1.SetActive(false);
        swordL2.SetActive(true);
        Destroy(gameObject);
    }
}
