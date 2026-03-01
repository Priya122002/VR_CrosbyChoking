using UnityEngine.Playables;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class InformationPanelBehaviour : PlayableBehaviour
{
    // more likely used as data container
    public int currentStep;
    public int totalSteps;
    public string infoText; // to change the inner text
    public string content;
    public string hindiInfoText; // to change the inner text
    public string hindiContent;
    
    public bool nextButtonEnabled; // to enable or disable the next button
    public bool prevButtonEnabled;
    
    public string nextButtonText;
    public string prevButtonText;
    public float buttonDelay = 0f;
    public InformationPanelManager infoPanel;

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        var clipTouchTime = playable.GetTime();
        
        if (info.effectivePlayState == PlayState.Playing && clipTouchTime <= 0.1f)
        {
            if (infoPanel == null)
            {
                return;
            }

             
            if (!infoPanel.gameObject.activeSelf) infoPanel.gameObject.SetActive(true);
            string steps = "Step " + currentStep + " of " + totalSteps;
            if (totalSteps == 0) steps = "";
            
            infoPanel.UpdateInformationPanel(
                steps: steps,
                header: infoText, content: content,
                hindiHeader: hindiInfoText, hindiContent: hindiContent,
                //showButtons: nextButtonEnabled,
                nextText: nextButtonText, prevText: prevButtonText,
                prevButtonEnabled: prevButtonEnabled,
                nextButtonEnabled: nextButtonEnabled,
                buttonVisibilityDelay: buttonDelay
            );
            infoPanel.ShowButtonWithDelay(buttonDelay);

        }
    }
}
