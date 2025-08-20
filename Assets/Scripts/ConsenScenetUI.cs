using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ConsentSceneUI : MonoBehaviour
{
    public Toggle[] consentToggles;
    public Button nextButton;

    void Awake() {
        nextButton.interactable = false;

        foreach (Toggle toggle in consentToggles) {
            toggle.onValueChanged.AddListener((isOn) => { CheckAllToggles(); });
        }

        nextButton.onClick.AddListener(() => SceneManager.LoadScene("SnapScene", LoadSceneMode.Single));
    }

    private void CheckAllToggles() {
        foreach (Toggle toggle in consentToggles) {
            if (!toggle.isOn) {
                nextButton.interactable = false;
                return;
            }
            nextButton.interactable = true;
        }
    }
}
