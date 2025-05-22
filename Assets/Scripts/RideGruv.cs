using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RideGruv : MonoBehaviour
{
    public bool rideable;
    public bool isRiding;
    public Transform gruv;
    MovementScript movescript;
    private void Start()
    {
        movescript = GetComponent<MovementScript>();
    }
    private void Update()
    {
        gruv.GetComponentInChildren<Animator>().SetBool("Riding", isRiding);
        if (isRiding)
        {
            Riding();
        }
        if (Vector3.Distance(transform.position, gruv.position) < 1.5f)
        {
            rideable = true;
        }
        else
        {
            rideable = false;
        }
        if (Input.GetKeyDown(KeyCode.E) && rideable)
        {
            isRiding = true;
        }
        if (!isRiding)
        {
            gruv.LookAt(new Vector3(transform.position.x, gruv.position.y, transform.position.z));
        }
    }
    void Riding()
    {
        gruv.transform.position = transform.position;
        gruv.rotation = transform.rotation;

        if (Input.GetKeyDown(KeyCode.E))
        {
            isRiding = false;
            transform.position = new Vector3(gruv.position.x + 2.5f, transform.position.y, gruv.position.z);
        }
    }
}
