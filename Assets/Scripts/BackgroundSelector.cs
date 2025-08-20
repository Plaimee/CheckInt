using System.Data;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BackgroundSelector : MonoBehaviour
{
    public Button[] backgroundButton;
    public Button nextButton;
    public static string backgroundPath;
    private string[] backgroundName = { "uni_label.png", "palace.png", "road.png", "bridge.png" };
    private string filePath = Path.Combine(Application.dataPath, "BackgroundImage", "Sanamchandra");

    void Start()
    {
        nextButton.interactable = false;
        for (int i = 0; i < backgroundButton.Length; i++)
        {
            int buttonIndex = i;

            backgroundButton[i].onClick.AddListener(() => OnBackgroundSelected(buttonIndex));
        }
    }

    public void OnBackgroundSelected(int index)
    {
        BackgroundSelector.backgroundPath = Path.Combine(filePath, backgroundName[index]);
        Debug.Log("Button " + backgroundButton[index].name + " clicked. \n Path: " + backgroundPath);
        nextButton.interactable = true;
    }
}
