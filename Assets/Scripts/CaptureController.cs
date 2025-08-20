using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CaptureController : MonoBehaviour
{
    [Header("Component References")]
    public WebCamController webCamController;
    public Button captureButton;
    public Button saveButton;
    public Button retakeButton;
    public Text timerText;
    public GameObject frame;

    [Header("Timer Settings")]
    float countdownTime = 3f;

    void Start()
    {
        if (webCamController == null)
        {
            Debug.LogError("WebCamController Is Not Assigned");
            return;
        }

        if (timerText != null)
        {
            timerText.gameObject.SetActive(false);
        }

        captureButton.onClick.AddListener(StartCaptureProcess);
        retakeButton.onClick.AddListener(RetakeSnapShot);

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
        captureButton.gameObject.SetActive(false);
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
        StartCoroutine(CountdownCoroutine());
    }

    public void RetakeSnapShot()
    {
        webCamController.PlayWebCam();
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

        webCamController.TakeSnapShot();
        webCamController.StopWebCam();
        SetPostCaptureState();
    }
}
