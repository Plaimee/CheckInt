using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Button))]
public class LoadSceneOnClick : MonoBehaviour
{
    [Tooltip("Input Scene Name Here")]
    public string sceneNameToLoad;

    private Button button;

    void Awake() {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }
    
    private void OnClick() {
        if (!string.IsNullOrEmpty(sceneNameToLoad)) {
            SceneManager.LoadScene(sceneNameToLoad, LoadSceneMode.Single);
        } else {
            Debug.LogWarning("No Reference Scene Name of" + gameObject.name);
        }
    }
}
