using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RawImage))]
public class WebCamDisplay : MonoBehaviour
{
    private RawImage displayImage;

    void Awake()
    {
        displayImage = GetComponent<RawImage>();

        if (WebCamController.instance != null)
        {
            displayImage.texture = WebCamController.instance.WebCamTexture;
        }
        else
        {
            Debug.LogError("WebCamController instance not found. Make sure it's set up in the precious scene");
        }
    }
}