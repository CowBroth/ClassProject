using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    Vector3[] targetPos;
    public Transform charHead;

    public Vector3 mPos;
    public Vector3 vel = Vector3.zero;
    Quaternion a;
    Quaternion b;
    private float t = 0.01f;
    public float testX = 1;
    public float testY = 1;

    float screenWidth;
    float screenHeight;
    float mouseX;
    float mouseY;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        screenWidth = Screen.width;
        screenHeight = Screen.height;
    }

    void Update()
    {
        mPos = Input.mousePosition;

        mouseX = mPos.x;
        mouseY = mPos.y;
        if (mouseX < 0)
        {
            mouseX = 0;
        }
        if (mouseX > screenWidth)
        {
            mouseX = screenWidth;
        }
        if (mouseY < 0)
        {
            mouseY = 0;
        }
        if (mouseY > screenHeight)
        {
            mouseY = screenHeight;
        }
        mouseX = (mouseX / screenWidth) - 0.5f;
        mouseY = (mouseY / screenHeight) - 0.5f;

        b = Quaternion.Euler(mouseY * 30, -mouseX * 30, 0);

        charHead.rotation = Quaternion.Slerp(a, b, t);
        a = charHead.rotation;
    }
    public void PlayButton()
    {
        SceneManager.LoadScene(1);
    }
    public void ExitButton()
    {
        Application.Quit();
    }
}
