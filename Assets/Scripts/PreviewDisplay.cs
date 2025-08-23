using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PreviewDisplay : MonoBehaviour
{
    [Tooltip("เวลาที่ไม่มีการใช้งาน (วินาที) ก่อนจะกลับไปหน้า Preview")]
    public float timeoutDuration = 300f;

    private float idleTimer;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        idleTimer = 0f;
    }

    void Update()
    {
        idleTimer += Time.deltaTime;

        if (IsAnyInput())
        {
            idleTimer = 0f;

            if (SceneManager.GetActiveScene().name == "PreviewScene")
            {
                SceneManager.LoadScene("StartScene", LoadSceneMode.Single);
            }
        }
        
        if (idleTimer >= timeoutDuration)
        {
            Debug.Log("No activity for 5 minutes, returning to PreviewScene.");
            if (SceneManager.GetActiveScene().name != "PreviewScene")
            {
                SceneManager.LoadScene("PreviewScene", LoadSceneMode.Single);
            }
            idleTimer = 0f;
        }
    }

    private bool IsAnyInput()
    {
        if (Keyboard.current != null && Keyboard.current.anyKey.wasPressedThisFrame)
        {
            return true;
        }

        if (Mouse.current != null)
        {
            if (Mouse.current.leftButton.wasPressedThisFrame ||
                Mouse.current.rightButton.wasPressedThisFrame ||
                Mouse.current.middleButton.wasPressedThisFrame)
            {
                return true;
            }

            if (Mouse.current.delta.ReadValue().sqrMagnitude > 0.1f)
            {
                return true;
            }
        }

        return false;
    }
}