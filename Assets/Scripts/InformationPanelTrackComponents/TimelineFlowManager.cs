using UnityEngine;
using UnityEngine.Playables;

public class TimelineFlowManager : MonoBehaviour
{
    public static TimelineFlowManager Instance;

    private PlayableDirector activeDirector;
    private bool isFinished;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    // Called automatically when a timeline starts
    public void RegisterDirector(PlayableDirector director)
    {
        activeDirector = director;
        isFinished = false;

        Debug.Log("Registered Timeline: " + director.name);
    }

    // Called automatically when timeline ends
    public void OnTimelineFinished(PlayableDirector director)
    {
        if (director == activeDirector)
        {
            isFinished = true;
            Debug.Log("Timeline Finished: " + director.name);
        }
    }

    public void Pause()
    {
        activeDirector?.Pause();
    }

    public void Resume()
    {
        if (activeDirector == null)
            return;

        if (isFinished)
        {
            Debug.Log("Timeline already finished.");
            return;
        }

        activeDirector.Play();
    }

    public bool IsFinished()
    {
        return isFinished;
    }
}