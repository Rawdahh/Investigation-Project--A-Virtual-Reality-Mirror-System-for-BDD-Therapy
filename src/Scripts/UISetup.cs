using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UISetup : MonoBehaviour
{
    public Canvas canvasButtons;

    void Start()
    {
        // WelcomeText
        Transform welcomeTextTransform = canvasButtons.transform.Find("WelcomeText");
        if (welcomeTextTransform != null)
        {
            TextMeshProUGUI welcomeText = welcomeTextTransform.GetComponent<TextMeshProUGUI>();
            if (welcomeText != null)
            {
                welcomeText.text = "Welcome to Perceptual Pathways";
                welcomeText.color = Color.white;
                welcomeText.fontSize = 80;
                welcomeText.alignment = TextAlignmentOptions.Center;
                welcomeText.outlineColor = Color.black;
                welcomeText.outlineWidth = 0.2f;
                welcomeTextTransform.localPosition = new Vector3(0, 200, 0); // Center on wall
            }
        }

        // Subtitle Text
        GameObject subtitleTextObj = new GameObject("SubtitleText", typeof(TextMeshProUGUI));
        subtitleTextObj.transform.SetParent(canvasButtons.transform, false);
        TextMeshProUGUI subtitleText = subtitleTextObj.GetComponent<TextMeshProUGUI>();
        subtitleText.text = "An immersive experience that explores how we notice and interpret moments in everyday life.";
        subtitleText.color = Color.white;
        subtitleText.fontSize = 40;
        subtitleText.alignment = TextAlignmentOptions.Center;
        subtitleText.outlineColor = Color.black;
        subtitleText.outlineWidth = 0.2f;
        subtitleText.rectTransform.anchoredPosition = new Vector2(0, 100);

        // BeginExperienceButton
        Transform startGameButtonTransform = canvasButtons.transform.Find("StartGameButton");
        if (startGameButtonTransform != null)
        {
            startGameButtonTransform.name = "BeginExperienceButton";
            TextMeshProUGUI buttonText = startGameButtonTransform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>();
            if (buttonText != null)
            {
                buttonText.text = "Begin Experience";
            }
        }

        // Add AboutExperienceButton
        GameObject aboutExperienceButtonObj = Instantiate(startGameButtonTransform.gameObject, canvasButtons.transform);
        aboutExperienceButtonObj.name = "AboutExperienceButton";
        aboutExperienceButtonObj.transform.localPosition = new Vector3(0, -50, 0);
        TextMeshProUGUI aboutButtonText = aboutExperienceButtonObj.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>();
        if (aboutButtonText != null)
        {
            aboutButtonText.text = "About Experience";
        }

        // Add ComfortPanelText
        GameObject comfortPanelTextObj = new GameObject("ComfortPanelText", typeof(TextMeshProUGUI));
        comfortPanelTextObj.transform.SetParent(canvasButtons.transform, false);
        TextMeshProUGUI comfortPanelText = comfortPanelTextObj.GetComponent<TextMeshProUGUI>();
        comfortPanelText.text = "If at any point you'd like to rest, look down and press the blue button to pause.";
        comfortPanelText.color = Color.white;
        comfortPanelText.fontSize = 30;
        comfortPanelText.alignment = TextAlignmentOptions.Center;
        comfortPanelText.rectTransform.anchoredPosition = new Vector2(0, -200);
    }
}
