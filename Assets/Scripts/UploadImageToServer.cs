using System.Collections;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using ZXing;
using ZXing.QrCode;

[System.Serializable]
public class ServerResponse
{
    public string message;
    public string final_image_url;
}

public class UploadImageToServer : MonoBehaviour
{
    [Header("Server Configuration")]
    [SerializeField] private string flaskURL = "http://127.0.0.1:5000";
    [SerializeField] private string mergeAPIEndpoint = "merge_images";
    [SerializeField] private string resultSceneName = "ResultScene";
    [SerializeField] private string saveUrlEndpoint = "https://funcslash.com/projects/2025/argus/db_api.php";


    public static Texture2D finalResultTexture { get; private set; }
    public static Texture2D qrCodeTexture { get; private set; }
    private bool uploadTriggered = false;

    void Update()
    {
        bool bothPathsReady = !string.IsNullOrEmpty(ImageSaver.fullPath) && !string.IsNullOrEmpty(BackgroundSelector.backgroundPath);

        if (bothPathsReady && !uploadTriggered)
        {
            uploadTriggered = true;

            Debug.Log("Both paths are ready. Triggering upload...");
            StartCoroutine(UploadAndProcess(ImageSaver.fullPath, BackgroundSelector.backgroundPath));
        }
        else if (!bothPathsReady && uploadTriggered)
        {
            uploadTriggered = false;
            Debug.Log("System reset. Ready for new upload.");
        }
    }

    IEnumerator UploadAndProcess(string foregroundPath, string backgroundPath)
    {
        byte[] foregroundData = File.ReadAllBytes(foregroundPath);
        byte[] backgroundData = File.ReadAllBytes(backgroundPath);

        WWWForm form = new WWWForm();

        form.AddBinaryData("foreground_file", foregroundData, Path.GetFileName(foregroundPath), "image/png");
        form.AddBinaryData("background_file", backgroundData, Path.GetFileName(backgroundPath), "image/png");

        string mergeURL = $"{flaskURL}/{mergeAPIEndpoint}";
        ServerResponse response = null;

        using (UnityWebRequest www = UnityWebRequest.Post(mergeURL, form))
        {
            Debug.Log("Uploading 2 images...");
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error during upload/process: " + www.error + "\nResponse: " + www.downloadHandler.text);
                ResetPaths();
                yield break;
            }
            else
            {
                Debug.Log("Processing complete! \n Response: " + www.downloadHandler.text);
                response = JsonUtility.FromJson<ServerResponse>(www.downloadHandler.text);
            }
        }

        if (response != null && !string.IsNullOrEmpty(response.final_image_url))
        {
            yield return StartCoroutine(InsertToDatabase(response.final_image_url));
            yield return StartCoroutine(DownloadTexture(response.final_image_url));

            GenerateQRCode(response.final_image_url);
            Debug.Log("All data processed. Loading result scene...");
            SceneManager.LoadScene(resultSceneName);
        }
        ResetPaths();
    }

    IEnumerator InsertToDatabase(string url)
    {
        string jsonPayload = $"{{ \"url\": \"{url}\" }}";
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonPayload);

        using (UnityWebRequest www = new UnityWebRequest(saveUrlEndpoint, "POST"))
        {
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-type", "application/json");

            Debug.Log("Sending URL to database: " + url);
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error sending URL to database: " + www.error + "\nResponse: " + www.downloadHandler.text);
            }
            else
            {
                Debug.Log("URL saved to database successfully!. \nResponse: " + www.downloadHandler.text);
            }
        }
    }

    IEnumerator DownloadTexture(string url)
    {
        Debug.Log("Downloading image from: " + url);
        using (UnityWebRequest www = UnityWebRequestTexture.GetTexture(url))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log("Error dowloading image from " + url + ": " + www.error);
                finalResultTexture = null;
            }
            else
            {
                finalResultTexture = DownloadHandlerTexture.GetContent(www);
            }
        }
    }

    private void GenerateQRCode(string textToEncode)
    {
        var writer = new BarcodeWriter
        {
            Format = BarcodeFormat.QR_CODE,
            Options = new QrCodeEncodingOptions { Height = 150, Width = 150, Margin = 1 }
        };
        var pixels = writer.Write(textToEncode);

        var qrCodeTex = new Texture2D(150, 150, TextureFormat.RGBA32, false);
        qrCodeTex.SetPixels32(pixels);
        qrCodeTex.Apply();

        qrCodeTexture = qrCodeTex;
        Debug.Log("QR Code generated successfully!");
    }

    void ResetPaths()
    {
        Debug.Log("Reseeting image paths.");
        ImageSaver.fullPath = null;
        BackgroundSelector.backgroundPath = null;
    }
}
