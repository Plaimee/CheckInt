using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CaptureController : MonoBehaviour
{
    [Header("Component References")]
    public Button captureButton;
    public Button saveButton;
    public Button retakeButton;
    public Text timerText;
    public GameObject frame;

    [Header("Timer Settings")]
    float countdownTime = 3f;

    void Start()
    {
        if (WebCamController.instance == null)
        {
            Debug.LogError("WebCamContollerIs Not Assigned");
            return;
        }

        if (timerText != null)
        {
            timerText.gameObject.SetActive(false);
        }

        captureButton.onClick.AddListener(StartCaptureProcess);
        retakeButton.onClick.AddListener(RetakeSnapShot);
        WebCamController.instance.PlayWebCam();
        SetInitialState();
    }

    private void SetInitialState()
    {
        captureButton.gameObject.SetActive(true);
        saveButton.gameObject.SetActive(false);
        retakeButton.gameObject.SetActive(false);
        frame.SetActive(true);

    }
    private void SetPostCaptureState()
    {
        saveButton.gameObject.SetActive(true);
        retakeButton.gameObject.SetActive(true);
        frame.SetActive(false);

    }

    void StartCaptureProcess()
    {
        if (timerText != null)
        {
            timerText.gameObject.SetActive(true);
        }
        captureButton.gameObject.SetActive(false);
        StartCoroutine(CountdownCoroutine());
    }

    public void RetakeSnapShot()
    {
        WebCamController.instance.PlayWebCam();
        SetInitialState();
    }

    IEnumerator CountdownCoroutine()
    {
        float remainingTime = countdownTime;
        while (remainingTime > 0)
        {
            if (timerText != null)
            {
                timerText.text = Mathf.CeilToInt(remainingTime).ToString();
            }
            yield return new WaitForSeconds(1f);
            remainingTime--;
        }

        if (timerText != null)
        {
            timerText.gameObject.SetActive(false);
        }

        WebCamController.instance.TakeSnapShot();
        WebCamController.instance.StopWebCam();
        SetPostCaptureState();
    }
}
