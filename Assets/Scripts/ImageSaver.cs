using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ImageSaver : MonoBehaviour
{
    [Header("Component References")]
    public CaptureController captureController;
    public Button saveButton;

    public static string fullPath;

    void Start()
    {
        if (WebCamController.instance == null)
        {
            Debug.LogError("WebCam Is Not Assigned");
            return;
        }

        if (saveButton != null)
        {
            saveButton.onClick.AddListener(SaveCurrentImage);
        }
    }

    public void SaveCurrentImage()
    {
        Texture2D snap = WebCamController.instance.LastCapturedFrame;

        if (snap == null)
        {
            Debug.LogError("Can not Save Image! Please Press Capture Button First");
            return;
        }

        byte[] bytes = snap.EncodeToPNG();

        string savePath = Path.Combine(Application.streamingAssetsPath, "WebCamSnaps");
        Directory.CreateDirectory(savePath);

        string fileName = "snapshot_" + System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".png";
        fullPath = Path.Combine(savePath, fileName);

        File.WriteAllBytes(fullPath, bytes);
        Debug.Log("Snapshot saved to: " + fullPath);

        SceneManager.LoadScene("ProcessScene", LoadSceneMode.Single);
    }
}
