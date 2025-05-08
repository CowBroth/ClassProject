using UnityEngine;

public class ManagerScript : MonoBehaviour
{
    public static ManagerScript instance;
    public GameObject player;
    public int playerState;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {

    }
}
