using UnityEngine.Playables;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using System.Linq;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.Events;

public class TimeLineManagerController : MonoBehaviour
{

    //// Update is called once per frame
    //void Update()
    //{
    //    if(Input.GetKeyDown(KeyCode.Space)){
    //        MoveNext();
    //    }
    //}
    //public GameObject buttonObject;
    //public bool PreloadImageTrack = false;
    [System.NonSerialized] public PlayableDirector dir;
    public TimelineClip timeline;
    public List<double> timeStops;
    [SerializeField] float previousResetOffsetTime = 0.05f;
    [SerializeField] private int currentIndex = 0;
    [SerializeField] private int currentSequence = 0;
    [SerializeField] public string ZoneName;
    [SerializeField] public string ZoneNameHindi;
    [TextArea(2, 5)]
    [SerializeField] public string DetailedInformation;
    [SerializeField] public string DetailedInformationHindi;
    private float _timeScale = 0.4f;
    public bool alwaysKeepActive = false;
    public bool isPlayingTimeline = false;

    public GameObject SequenceButton;
    public Transform SequenceButtonParent;

    public List<double> clipStartTimes;
    // add cap or limit to the number of timestamps that can be saved. or simply use timeline functionality to fetch the list of timestamps 
    //for each timeline - incase of need for going to previous timelines we can provide a form of menu option

    public UnityEvent OnTimelineDisable;
    public UnityEvent OnTimelineBegin;
    public UnityEvent OnTimelineEnable;
    

    private void Start()
    {
        dir = transform.GetComponent<PlayableDirector>();

        //if (!dir)
        //{
        //    transform.gameObject.SetActive(false);
        //    return;
        //}
        //timeStops.Add(0);
        clipStartTimes = new List<double>();
        TimelineAsset asset = dir.playableAsset as TimelineAsset;

        //ControlTrack track_ = (ControlTrack) asset.GetOutputTrack(0);
        //if (!track_)
        //{
        //    transform.gameObject.SetActive(false);
        //    return;
        //}
        // Note - we're deleting the track if it exists already, since we want to generate everything on the spot for this example
        // foreach (TrackAsset track in asset.GetOutputTracks())
        // {
        //     foreach (TimelineClip clip in track.GetClips())
        //     {
        //         clipStartTimes.Add(clip.start);
        //         var seqButton = Instantiate(SequenceButton, SequenceButtonParent);
        //         seqButton.GetComponent<Button>().onClick.AddListener(
        //             delegate { ToSequence(clip.start); }
        //         );
        //         seqButton.transform.GetChild(0).GetComponent<TMP_Text>().text = clip.displayName;
        //     }
        // }
        try
        {


            foreach (ControlTrack track in asset.GetOutputTracks())
            {
                //Debug.LogWarning("+++++" + track.name);
                var orderedControlTrack = track.GetClips().OrderBy(clip => clip.start);

                foreach (TimelineClip clip in orderedControlTrack/*track.GetClips()*/)
                {
                    ControlPlayableAsset clipAsset = clip.asset as ControlPlayableAsset;
                    double clip_start = clip.start;
                    GameObject timelineObject = clipAsset.sourceGameObject.Resolve(dir);
                    //var g = dir.GetGenericBinding(clipAsset.sourceGameObject);
                    if (timelineObject)
                    {


                        PlayableDirector subDir = timelineObject.GetComponent<PlayableDirector>();


                        TimelineAsset asset_ = subDir.playableAsset as TimelineAsset;
                        //Debug.LogWarning(clip.displayName + "-----------" + clip_start);
                        TrackAsset infoPanelTrack_ = asset_.GetOutputTrack(0); // put the information panel on top
                                                                               //foreach (MarkerTrack track_ in asset_.GetOutputTracks()) // known bug, should place the playpause signal at the top since the default index (0) is only read.
                                                                               //{
                        if (infoPanelTrack_ is MarkerTrack)
                        {
                            infoPanelTrack_ = asset_.GetOutputTrack(1);
                        }
                        var orderedInfoTrack_ = infoPanelTrack_.GetClips().OrderBy(sig_ => sig_.start);

                        //Debug.LogWarning(orderedInfoTrack_.Count() + "-----------" + asset_.name);

                        //for (int i = 0; i < signalTrack_.GetMarkerCount(); i++)
                        foreach (TimelineClip infoClip_ in orderedInfoTrack_)
                        {
                            timeStops.Add(infoClip_.start + clip_start /*clip.start*/);
                            //Debug.LogWarning(asset_.name + "--------------" + infoClip_.start + "---" + clip_start + "----------------" + timeStops[timeStops.Count - 1]);
                            //if (i==track_.GetMarkerCount() - 1)
                            //{
                            //    clip_start += timeStops[timeStops.Count-1];
                            //}
                        }
                    }
                    // foreach (TimelineClip clip_ in track_.GetClips())
                    // {
                    //     timeStops.Add(clip_.start + clip.start);
                    //     //Debug.LogWarning("------------------------------" + clip_.displayName);
                    // }
                    //    break;
                    //}

                }
            }

        }
        catch (System.InvalidCastException e_)
        {
            Debug.LogWarning("[TimeLineManagerController] invalid cast exception: " + e_.Message, gameObject);
        }

        dir.stopped += OnTimelineFinished;


        if (alwaysKeepActive)
        {
            dir.gameObject.SetActive(true);
        }
        else
        {
            dir.gameObject.SetActive(false);
        }
    }
    private void OnEnable()
    {
        OnTimelineEnable.Invoke();
    }
    private void OnDisable()
    {
        OnTimelineDisable.Invoke();
        if (dir != null) dir.stopped -= OnTimelineFinished;
    }

    private void OnTimelineFinished(PlayableDirector director)
    {
        if (director == dir)
        {
            isPlayingTimeline = false;
        }
        currentIndex = 0;
    }

    //Next action should be deactivated untill the completion of the clip
    public void MoveNext() // to move on to the next target in the player checklist
    {
        if (dir.state == PlayState.Playing) return;

        //if (currentIndex <= timeStops.Count - 1)
        {
            dir.Pause();
            //if(currentIndex + 1 != timeStops.Count && dir.time >= timeStops[currentIndex] && dir.time <= timeStops[currentIndex+1])
            currentIndex++;
            //else if(currentIndex + 1 == timeStops.Count && dir.time >= timeStops[currentIndex])
            //dir.time = timeStops[currentIndex] /*+ 1f*/;
            dir.Resume();
        }
    }

    public void MovePrevious()
    {
        if (currentIndex == 0)
            return;
        dir.Pause();
        currentIndex--;
        dir.time = timeStops[currentIndex] /*+ 1f*/;
        dir.Resume();
    }

    public void BeginTimeline()
    {
        try
        {
            dir.gameObject.SetActive(true);
            dir.Play();
            isPlayingTimeline = true;
            OnTimelineBegin.Invoke();
            SetSpeed(_timeScale);
        }
        catch (System.Exception e)
        {
            Debug.Log("Exception at timeline " + e.Message, gameObject);
        }
    }

    public void EndTimeline()
    {
        if (dir != null)
        {
            dir.gameObject.SetActive(false);
            dir.Stop();
        }
        currentIndex = 0;
        isPlayingTimeline = false;
    }
    public void NextSequence()
    {
        currentSequence++;
        if (currentSequence >= clipStartTimes.Count)
            currentSequence = clipStartTimes.Count - 1;
        dir.time = clipStartTimes[currentSequence];
    }

    public void ToSequence(double startTime)
    {
        dir.time = startTime;
    }

    public void ToSequence(float startTime)
    {
        dir.time = startTime;
    }

    public void PreviousSequence()
    {
        currentSequence--;
        if (currentSequence < 0)
            currentSequence = 0;
        dir.time = clipStartTimes[currentSequence];
    }

    public void SetSpeed(float timeScale)
    {
        PlayableDirector director = transform.GetComponent<PlayableDirector>();
        if (director != null && director.playableGraph.IsValid() && director.state == PlayState.Playing)
        {
            director.playableGraph.GetRootPlayable(0).SetSpeed(timeScale);
        }
    }
}
