using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RawImage))]
public class WebCamController : MonoBehaviour
{
    public static WebCamController instance { get; private set; }
    public WebCamTexture WebCamTexture { get; private set; }
    public Texture2D LastCapturedFrame { get; private set; }
    private RawImage webCamCanvas;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);
        webCamCanvas = GetComponent<RawImage>();
        if (WebCamTexture.devices.Length == 0)
        {
            Debug.LogError("No WebCam Detected");
            return;
        }

        WebCamDevice device = WebCamTexture.devices[0];
        WebCamTexture = new WebCamTexture(device.name, 640, 480, 60);
        webCamCanvas.texture = WebCamTexture;
        webCamCanvas.SetNativeSize();

        PlayWebCam();
    }

    void Start()
    {
        Application.targetFrameRate = 60;
    }

    public void TakeSnapShot()
    {
        if (WebCamTexture == null || !WebCamTexture.isPlaying)
        {
            Debug.LogWarning("Can't Snapshot: Webcam Is Not Working");
            return;
        }

        LastCapturedFrame = new Texture2D(WebCamTexture.width, WebCamTexture.height);
        LastCapturedFrame.SetPixels(WebCamTexture.GetPixels());
        LastCapturedFrame.Apply();
    }

    public void PlayWebCam()
    {
        if (WebCamTexture != null && !WebCamTexture.isPlaying)
        {
            WebCamTexture.Play();
        }
    }

    public void StopWebCam()
    {
        if (WebCamTexture != null && WebCamTexture.isPlaying)
        {
            WebCamTexture.Stop();
        }
    }

    void OnDestroy()
    {
        StopWebCam();
    }
}
