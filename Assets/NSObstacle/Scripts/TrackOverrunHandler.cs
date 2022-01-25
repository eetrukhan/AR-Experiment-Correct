using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackOverrunHandler : MonoBehaviour
{
    public Material TrackOverrunMaterial;

    public GameObject CollisionIndicator;
    public AudioSource Audio;
    public AudioClip SoundToPlay;

    public ObstacleFactory Track;

    public event Action OnEnteredTrack;
    public event Action OnLeftTrack;
    public event Action OnReturnedOnTrack;
    public event Action OnCrossedFinishLine;

    private enum TrackingState
    {
        ReadyToStart,
        Started,
        Stopped
    }

    private StartFrom _startFrom;
    private TrackingState _trackingState = TrackingState.Stopped;

    private Transform _trackPose;

    private static readonly float ALMOST_THERE = 0.05f;

    void Start()
    {
        if (TrackOverrunMaterial == null)
        {
            Debug.LogError("Error: The TrackOverrunMaterial can't be left unassigned. Disabling the script");
            enabled = false;
            return;
        }

        if (CollisionIndicator == null)
        {
            Debug.LogError("Error: The CollisionIndicator can't be left unassigned. Disabling the script");
            enabled = false;
            return;
        }

        if (Audio == null)
        {
            Debug.LogError("Error: The Audio can't be left unassigned. Disabling the script");
            enabled = false;
            return;
        }

        if (SoundToPlay == null)
        {
            Debug.LogError("Error: The SoundToPlay can't be left unassigned. Disabling the script");
            enabled = false;
            return;
        }

        if (Track == null)
        {
            Debug.LogError("Error: The Track can't be left unassigned. Disabling the script");
            enabled = false;
            return;
        }

        _trackPose = Track.transform.Find("TrackItself");
        if (_trackPose == null)
        {
            Debug.LogError("Error: TrackOverrunsHandler couldn't identify the pose of the track. Disabling the script");
            enabled = false;
            return;
        }
    }

    void Update()
    {
        if (!enabled || _trackingState != TrackingState.Started)
            return;

        try
        {
            if (IsFinishLineCrossed(Track.GetUserLocalPositionOnTrack()))
            {
                StopTracking();

                OnCrossedFinishLine();
            }
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!enabled)
            return;

        if (other.gameObject.tag == "Track")
        {
            if (_trackingState == TrackingState.ReadyToStart)
            {
                _trackingState = TrackingState.Started;

                OnEnteredTrack();
            }
            else if (_trackingState == TrackingState.Started)
            {
                CollisionIndicator.SetActive(false);
                Audio.Stop();

                OnReturnedOnTrack();
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (!enabled)
            return;

        if (other.gameObject.tag == "Track")
        {
            if (_trackingState == TrackingState.Started && !IsFinishLineCrossed(Track.GetUserLocalPositionOnTrack(), ALMOST_THERE))
            {
                CollisionIndicator.GetComponent<Renderer>().material = TrackOverrunMaterial;
                CollisionIndicator.SetActive(true);

                Audio.clip = SoundToPlay;
                Audio.Play();

                OnLeftTrack();
            }
        }
    }

    private bool IsFinishLineCrossed(Vector3 positionOnTrack, float margin = 0f)
    {
        return (_startFrom == StartFrom.TheBeginning && positionOnTrack.z >= _trackPose.localScale.z / 2f - margin) ||
                (_startFrom == StartFrom.TheEnd && positionOnTrack.z <= -_trackPose.localScale.z / 2f + margin);
    }

    public void StartTracking(StartFrom startFrom)
    {
        if (!enabled)
            return;

        if (_trackingState != TrackingState.Stopped)
            throw new Exception("TrackOverrunsHandler is already working. You can't call this method untill the tracking is stopped");

        _startFrom = startFrom;
        _trackingState = TrackingState.ReadyToStart;
    }

    public void StopTracking()
    {
        if (!enabled || _trackingState == TrackingState.Stopped)
            return;

        CollisionIndicator.SetActive(false);
        Audio.Stop();

        _trackingState = TrackingState.Stopped;
    }
}
