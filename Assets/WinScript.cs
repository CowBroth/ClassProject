using UnityEngine;

public class WinScript : MonoBehaviour
{
    MeshCollider col;
    public Transform player;
    void Start()
    {
        col = GetComponentInChildren<MeshCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, player.position) < 3)
        {
            Application.Quit();
        }
    }
}
