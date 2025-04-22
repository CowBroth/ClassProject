using UnityEngine;

public class Hitbox : MonoBehaviour
{
    public float timerLength = 0.25f;
    float timer;
    public GameObject point;
    void Start()
    {
        timer = 0f;
        point = GameObject.Find("AttackPoint");
    }
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > timerLength)
        {
            Destroy(gameObject);
        }
        transform.position = point.transform.position;
        transform.rotation = point.transform.rotation;
    }
}
