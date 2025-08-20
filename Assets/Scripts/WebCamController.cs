using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RawImage))]
public class WebCamController : MonoBehaviour
{
    public WebCamTexture WebCamTexture { get; private set; }
    public Texture2D LastCapturedFrame { get; private set; }
    private RawImage webCamCanvas;

    void Awake()
    {
        webCamCanvas = GetComponent<RawImage>();
        if (WebCamTexture.devices.Length == 0)
        {
            Debug.LogError("No WebCam Detected");
            return;
        }

        WebCamDevice device = WebCamTexture.devices[0];
        WebCamTexture = new WebCamTexture(device.name);
        webCamCanvas.texture = WebCamTexture;

        PlayWebCam();
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
