using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;
public class TimeLineControl : MonoBehaviour
{
    private PlayableDirector dir;

    [SerializeField] private List<float> timeStamps;
    [SerializeField] private int currentIndex = -1;
    [SerializeField] private float fastSpeed = 10f;
    [SerializeField] private bool playContinuous = true;
    [SerializeField] private bool timelineTeleport = false;

    void Start()
    {
        dir = GetComponent<PlayableDirector>();
    }

    public void MoveTimelineTo(int index)
    {
        dir.Pause();
        dir.time = timeStamps[index];
        dir.Resume();
    }

    public void NextPosition()
    {
        dir.Pause();
        currentIndex++;
        if(currentIndex >= timeStamps.Count)
            return;
        dir.time = timeStamps[currentIndex];
        dir.Resume();
    }

    public void NextPositionMove(int index)
    {
        if (timelineTeleport)
            MoveTimelineTo(index);
        if(currentIndex >= timeStamps.Count)
            return;
        float timeRequired;
        AudioListener.pause = true;
        if(timeStamps[index] - dir.time < 0)
        {
            dir.playableGraph.GetRootPlayable(0).SetSpeed(-fastSpeed);
            timeRequired = (float)(dir.time - timeStamps[index])/fastSpeed;
        }
        else
        {
            dir.playableGraph.GetRootPlayable(0).SetSpeed(fastSpeed);
            timeRequired = (float)(timeStamps[index] - dir.time)/fastSpeed;
        }
        currentIndex = index;
        Invoke("NormalSpeed", timeRequired);
    }

    void NormalSpeed()
    {
        dir.playableGraph.GetRootPlayable(0).SetSpeed(1);
        AudioListener.pause = false;
    }

    public void Reset()
    {
        currentIndex = -1;
    }

    public void BackToStart()
    {
        dir.Pause();
        dir.time = 0;
        dir.Play();
    }

    public void PauseIfNotContinuous()
    {
        if(!playContinuous)
        {
            dir.playableGraph.GetRootPlayable(0).Pause();
        }
    }

    public void ResumeIfNotContinuous()
    {
        if (!playContinuous)
        {
            dir.playableGraph.GetRootPlayable(0).Play();
        }
    }

}

