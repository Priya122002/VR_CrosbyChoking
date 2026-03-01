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
    public Button prevButton;
    public TextMeshProUGUI nextButtonText;
    public TextMeshProUGUI prevButtonText;

    /*public ContentSizeFitter _nextButtonCSF;
    public ContentSizeFitter _prevButtonCSF;*/
    public GameObject buttonsHolder;

    private bool isHindi = false;
    private string engHeaderText, engContentText, hinHeaderText, hinContentText;

    [SerializeField] private Button endModuleButtton;

    private void Start()
    {
       /* _nextButtonCSF = nextButton.GetComponent<ContentSizeFitter>();
        _prevButtonCSF = prevButton.GetComponent<ContentSizeFitter>();*/
       /* if (!voiceController)
        {
            voiceController = FindFirstObjectByType<VoiceController>();
            if (!voiceController)
            {
                Debug.LogWarning("[InformationPanelManager] Failed to get VoiceController");
            }
        }

        SettingsManager.instance.OnLanguageSelection += UpdateLanguage;*/
        if (endModuleButtton != null) endModuleButtton.onClick.AddListener(StopInfoText);

        gameObject.SetActive(false);
    }
    private void OnDestroy()
    {
       // SettingsManager.instance.OnLanguageSelection -= UpdateLanguage;
        if (endModuleButtton != null) endModuleButtton.onClick.RemoveListener(StopInfoText);
    }

    private void OnEnable()
    {
      //  UpdateLanguage(SettingsManager.instance.IsHindiSelected);
    }


    public void UpdateInformationPanel(
        string steps, string header, string content, string hindiHeader, string hindiContent,
        string nextText = "Next", string prevText = "Prev", bool prevButtonEnabled = true, bool nextButtonEnabled = true, float buttonVisibilityDelay = 0)
    {
        // buttonsHolder.SetActive(false);
        stepsText.text = steps;
        engHeaderText = header;
        engContentText = content;
        hinHeaderText = hindiHeader;
        hinContentText = hindiContent;
        ChangeText();
        // nextButton.gameObject.SetActive(showButtons);
        // prevButton.gameObject.SetActive(showButtons);
       /* if (_nextButtonCSF)
            _nextButtonCSF.horizontalFit = ContentSizeFitter.FitMode.MinSize;
        if (_prevButtonCSF)
            _prevButtonCSF.horizontalFit = ContentSizeFitter.FitMode.MinSize;*/
        nextButtonText.text = nextText;
        prevButtonText.text = prevText;
        //if (showButtons)
        //{
        //    nextButtonText.text = nextText;
        //    prevButtonText.text = prevText;
        //}
        prevButton.gameObject.SetActive(prevButtonEnabled);
        nextButton.gameObject.SetActive(nextButtonEnabled);

        // Invoke(nameof(ShowButtonWithDelay), buttonVisibilityDelay);
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
       // UIUtility.RebuildLayoutsRecursivelyWithDelay(this, gameObject, 0.1f);
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
