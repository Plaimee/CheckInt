using UnityEngine;
using UnityEngine.UI;

public class ResultImage : MonoBehaviour
{
    [Header("UI Reference")]
    [Tooltip("Assign the RawImage from your scene here.")]
    [SerializeField] private RawImage resultDisplayImage;
    [SerializeField] private RawImage qrcodeDisplayImage;
    void Start()
    {
        if (resultDisplayImage == null || qrcodeDisplayImage == null)
        {
            Debug.LogError("Result Display Image or QR code Display Image is not assigned in the inspector");
            return;
        }

        if (UploadImageToServer.finalResultTexture != null && UploadImageToServer.qrCodeTexture != null)
        {
            Debug.Log("Applying final texture to RawImage.");
            resultDisplayImage.texture = UploadImageToServer.finalResultTexture;
            qrcodeDisplayImage.texture = UploadImageToServer.qrCodeTexture;
            resultDisplayImage.SetNativeSize();
            qrcodeDisplayImage.SetNativeSize();
        }
        else
        {
            Debug.LogError("Final result texture was not found. Did the previous scene run correctly?");
        }
    }
}
