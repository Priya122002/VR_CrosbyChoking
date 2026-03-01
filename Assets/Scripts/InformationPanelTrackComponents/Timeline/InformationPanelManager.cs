using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InformationPanelManager : MonoBehaviour
{
   // public VoiceController voiceController = null;
    //public Toggle languageToggle;
    public TextMeshProUGUI stepsText;
    public TextMeshProUGUI headerText;
    public TextMeshProUGUI contentText;
    public Button nextButton;
 
    public TextMeshProUGUI nextButtonText;

    /*public ContentSizeFitter _nextButtonCSF;
    public ContentSizeFitter _prevButtonCSF;*/
    public GameObject buttonsHolder;

    private bool isHindi = false;
    private string engHeaderText, engContentText, hinHeaderText, hinContentText;

    [SerializeField] private Button endModuleButtton;

    private void Start()
    {
        if (endModuleButtton != null) endModuleButtton.onClick.AddListener(StopInfoText);

        gameObject.SetActive(false);
    }
    private void OnDestroy()
    {
        if (endModuleButtton != null) endModuleButtton.onClick.RemoveListener(StopInfoText);
    }

    private void OnEnable()
    {
    }


    public void UpdateInformationPanel(
        string steps, string header, string content, string hindiHeader, string hindiContent,
        string nextText = "Next", string prevText = "Prev", bool prevButtonEnabled = true, bool nextButtonEnabled = true, float buttonVisibilityDelay = 0)
    {
        stepsText.text = steps;
        engHeaderText = header;
        engContentText = content;
        hinHeaderText = hindiHeader;
        hinContentText = hindiContent;
        ChangeText();
        nextButtonText.text = nextText;

        nextButton.gameObject.SetActive(nextButtonEnabled);
        PlayInfoText();
    }

    public void SetCurrentModuleName(string englishName = "", string hindiName = "")
    {
        buttonsHolder.SetActive(false);
        stepsText.text = "";
        engHeaderText = englishName;
        engContentText = "";
        hinHeaderText = hindiName;
        hinContentText = "";
        ChangeText();
    }
    public void OnNextClicked()
    {
        Debug.Log("Next Button Clicked");

        if (TimelineFlowManager.Instance == null)
        {
            Debug.LogError("TimelineFlowManager is NULL");
            return;
        }

        if (TimelineFlowManager.Instance.IsFinished())
        {
            Debug.Log("Timeline already finished");
            nextButton.interactable = false;
            return;
        }

        Debug.Log("Resuming Timeline");
        TimelineFlowManager.Instance.Resume();
    }
    public void ShowButtonWithDelay(float t)
    {
        if (t >= 0f && t <= 1f)
        {
            showButton();
        }
        else
        {
            buttonsHolder.SetActive(false);
            Invoke(nameof(showButton), t * 1.5f);
        }
    }
    void showButton()
    {
        buttonsHolder.SetActive(true);
    }

    public void PlayInfoText(bool force = false)
    {
       /* if (voiceController)
        {
            string plainText = GetString(headerText.text + ". " + contentText.text);
            if (plainText.Length > 0) voiceController.PlayAudio(plainText, force);
        }*/
    }

    public static string GetString(string str)
    {
        Regex rich = new Regex(@"<[^>]*>");

        if (rich.IsMatch(str))
        {
            str = rich.Replace(str, string.Empty);
        }

        return str;
    }


    public void UpdateLanguage(bool value)
    {
        if (isHindi == value) return;
        isHindi = value;
       // voiceController?.UpdateVoiceEngine(isHindi);
        ChangeText();
        PlayInfoText(false);//to play the TTS after changing the Language
    }


    private void ChangeText()
    {
        //voiceController.UpdateVoiceEngine(isHindi);
        if (isHindi)
        {
            headerText.text = hinHeaderText;
            contentText.text = hinContentText;
        }
        else
        {
            headerText.text = engHeaderText;
            contentText.text = engContentText;
        }
    }

    public void StopInfoText()
    {
       /* if (voiceController)
        {
            voiceController.PauseAudio();
        }*/
    }

}
