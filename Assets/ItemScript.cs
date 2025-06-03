using UnityEngine;

public class ItemScript : MonoBehaviour
{
    public GameObject swordL1, swordL2, shieldL1, shieldL2;
    public GameObject sword, shield;
    void SwordUpgrade()
    {
        swordL1.SetActive(false);
        swordL2.SetActive(true);
    }
    void ShieldUpgrade()
    {
        shieldL1.SetActive(false);
        shieldL2.SetActive(true);
    }
}
