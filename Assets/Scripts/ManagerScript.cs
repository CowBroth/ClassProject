using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        //Cursor.lockState = CursorLockMode.Locked;
    }
    public void DieMethod()
    {
        StartCoroutine(DieTimer());
    }
    public IEnumerator DieTimer()
    {
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene(0);
    }
    
}
