using UnityEngine;
using UnityEngine.Playables;

[RequireComponent(typeof(PlayableDirector))]
public class TimelineAutoRegister : MonoBehaviour
{
    private PlayableDirector director;

    private void Awake()
    {
        director = GetComponent<PlayableDirector>();

        director.played += OnPlayed;
        director.stopped += OnStopped;
    }

    private void Start()
    {
        if (director.state == PlayState.Playing)
        {
            Debug.Log("Auto registering already playing timeline: " + director.name);
            TimelineFlowManager.Instance.RegisterDirector(director);
        }
    }

    private void OnPlayed(PlayableDirector pd)
    {
        Debug.Log("Timeline Started: " + pd.name);
        TimelineFlowManager.Instance.RegisterDirector(pd);
    }

    private void OnStopped(PlayableDirector pd)
    {
        if (pd.time >= pd.duration)
        {
            TimelineFlowManager.Instance.OnTimelineFinished(pd);
        }
    }
}