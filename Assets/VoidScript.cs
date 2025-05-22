using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VoidScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene(0);
        }
    }
}
