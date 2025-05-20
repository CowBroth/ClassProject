using Unity.Cinemachine;
using UnityEngine;

public class TestCam : MonoBehaviour
{
    public float axs;
    public Vector3 q;
    private bool moving;

    public CinemachineOrbitalFollow cam;
    void Start()
    {

    }
    void Update()
    {
        axs = cam.HorizontalAxis.Value;
        q = new Vector3(0, axs, 0);
        Move();
        Cam();
    }
    void Move()
    {
        if (Input.GetKey(KeyCode.W))
        {
            moving = true;
        }
        else
        {
            moving = false;
        }
    }
    void Cam()
    {
        if (moving)
        {
            transform.rotation = Quaternion.Euler(q);
        }
    }
}
