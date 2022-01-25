using System;
using UnityEngine;

public class WalkingStateBase : State
{
    protected bool _outOfTheTrack;
    protected float _startedWalking;

    private const float DELAY_SEC = 2;

    public WalkingStateBase(ISceneController sceneController, StartFrom startFrom) : base(sceneController)
    {
        _outOfTheTrack = false;
        _startedWalking = Time.unscaledTime;

        _sceneController.GetUsersHead().GetComponent<PathLengthController>().enabled = true;
        _sceneController.GetUsersHead().GetComponent<CollisionHandler>().enabled = true;
        _sceneController.GetUsersHead().GetComponent<SlowpokeDetector>().enabled = true;
        _sceneController.GetUsersHead().GetComponent<MoonwalkDetector>().StartDetecting(startFrom);

        float intensity = _sceneController.GetIntensity();
        _sceneController.GetTrack().GetComponent<ObstacleFactory>().IntensityOfObstacleAppearance = intensity;
        _sceneController.GetTrack().GetComponent<ObstacleFactory>().StartProducing(startFrom);
    }

    public override void StoppedAlongTheWay()
    {
        StopEverything();

        _sceneController.SetState(new ErrorStateBase(_sceneController, ErrorStateBase.ErrorType.StoppedAlongTheWay));
    }

    public override void WentBackwards()
    {
        StopEverything();

        _sceneController.SetState(new ErrorStateBase(_sceneController, ErrorStateBase.ErrorType.WentBackwards));
    }

    public override void LeftTrack()
    {
        _outOfTheTrack = true;
        _sceneController.SetTimerTo(DELAY_SEC);
    }

    public override void BackOnTrack()
    {
        _outOfTheTrack = false;
        _sceneController.StopTimer();
    }

    public override void CrossedFinishLine()
    {
        StopEverything();
        
        _sceneController.SetState(new DoneWalkingStateBase(_sceneController));
    }

    public override void TimeIsUp()
    {
        if (_outOfTheTrack)
        {
            StopEverything();

            _sceneController.SetState(new ErrorStateBase(_sceneController, ErrorStateBase.ErrorType.LeftTrackForTooLong));
        }
    }

    protected virtual void StopEverything()
    {
        try
        {
            _sceneController.StopTimer();
            _sceneController.GetMetronome().SetActive(false);
            _sceneController.GetUsersHead().GetComponent<PathLengthController>().enabled = false;
            _sceneController.GetUsersHead().GetComponent<CollisionHandler>().enabled = false;
            _sceneController.GetUsersHead().GetComponent<SlowpokeDetector>().enabled = false;
            _sceneController.GetUsersHead().GetComponent<MoonwalkDetector>().StopDetecting();
            _sceneController.GetUsersHead().GetComponent<TrackOverrunHandler>().StopTracking();
            _sceneController.GetTrack().GetComponent<ObstacleFactory>().StopProducing();
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }
}
