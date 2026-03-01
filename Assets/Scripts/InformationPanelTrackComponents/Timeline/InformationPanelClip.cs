using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[System.Serializable]
public class InformationPanelClip : PlayableAsset
 { 
    // This script will allow for the creation of the induvidual clips for the information panel

    //this is where the members of the behaviour will be changed using the following variables
    [Header("Step")]
    public int currentStep;
    public int totalSteps;

    [TextArea(3, 5)]
    public string infoText;

    [TextArea(2, 5)]
    public string content;

    [TextArea(3, 5)]
    public string hindiInfoText;

    [TextArea(2, 5)]
    public string hindiContent;
    [Space]
    // public Color panelColor;
    public bool nextButtonEnabled = true;
    public bool prevButtonEnabled = true;
    // [System.NonSerialized] public VoiceController voiceController;
    // public bool moveNextAfterTTS = false;
    [Header("Custom text for Buttons")]
    public string nextButton = "Next";
    public string prevButton = "Prev";
    public float buttonDelay = 0f;
    public ClipCaps clipCaps => ClipCaps.All;

    [System.NonSerialized] public InformationPanelManager infoPanel; 
    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        // voiceController = FindFirstObjectByType<VoiceController>();
        var playable = ScriptPlayable<InformationPanelBehaviour>.Create(graph);
        InformationPanelBehaviour infoPanelBehaviour = playable.GetBehaviour();
        infoPanelBehaviour.infoPanel = infoPanel;
        infoPanelBehaviour.currentStep = currentStep;
        infoPanelBehaviour.totalSteps = totalSteps;
        infoPanelBehaviour.infoText = infoText;
        infoPanelBehaviour.content = content;
        infoPanelBehaviour.hindiInfoText = hindiInfoText;
        infoPanelBehaviour.hindiContent = hindiContent;
        // infoPanelBehaviour.parsedText = GetString(infoText + "," + content);
        // infoPanelBehaviour.moveNextAfterTTS = moveNextAfterTTS;
        // infoPanelBehaviour.panelColor = panelColor;
        infoPanelBehaviour.nextButtonEnabled = nextButtonEnabled;
        infoPanelBehaviour.prevButtonEnabled = prevButtonEnabled;
        // infoPanelBehaviour.voiceController = voiceController;
        infoPanelBehaviour.nextButtonText = nextButton;
        infoPanelBehaviour.prevButtonText = prevButton;
        infoPanelBehaviour.buttonDelay = buttonDelay;
        return playable;
    }

    
}
