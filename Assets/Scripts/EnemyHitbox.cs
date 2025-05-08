using UnityEngine;

public class EnemyHitbox : MonoBehaviour
{
    public MovementScript playerScript;
    bool blocking;
   
    void Start()
    {
        blocking = playerScript.blocking;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
