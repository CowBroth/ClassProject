using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ManagerScript : MonoBehaviour
{
    public static ManagerScript instance;
    public GameObject player;
    public int playerState;
    [SerializeField] Slider slider;
    public float volume = 0.5f;
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
