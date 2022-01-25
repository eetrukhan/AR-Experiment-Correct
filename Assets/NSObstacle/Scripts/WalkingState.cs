using System;
using UnityEngine;

public class WalkingState : WalkingStateBase
{
    public WalkingState(ISceneController sceneController, StartFrom startFrom) : base(sceneController, startFrom)
    {
        float trackLength = _sceneController.GetTrack().transform.Find("TrackItself").localScale.z;
        _sceneController.GetDataStorage().NextTrial(
            _sceneController.GetSubjectNo(),
            _sceneController.GetTrack().GetComponent<ObstacleFactory>().IntensityOfObstacleAppearance,
            _sceneController.GetDesignOption(),
            _sceneController.GetProcedure(),
            _sceneController.ReturnAndIncrementTrialNo());
        _sceneController.GetDataStorage().GetCurrectTrialData().TrackLength = trackLength;
    }

    public override void StoppedAlongTheWay()
    {
        StopEverything();
        LogFailure(ErrorStateBase.ErrorType.StoppedAlongTheWay);

        _sceneController.SetState(new ErrorState(_sceneController, ErrorStateBase.ErrorType.StoppedAlongTheWay));
    }

    public override void WentBackwards()
    {
        StopEverything();
        LogFailure(ErrorStateBase.ErrorType.WentBackwards);

        _sceneController.SetState(new ErrorState(_sceneController, ErrorStateBase.ErrorType.WentBackwards));
    }

    public override void LeftTrack()
    {
        base.LeftTrack();

        _sceneController.GetDataStorage().GetCurrectTrialData().TrackOverruns++;
    }

    public override void CrossedFinishLine()
    {
        StopEverything();
        LogSuccess();
        
        _sceneController.SetState(new DoneWalkingState(_sceneController));
    }

    public override void TimeIsUp()
    {
        if (_outOfTheTrack)
        {
            StopEverything();
            LogFailure(ErrorStateBase.ErrorType.LeftTrackForTooLong);

            _sceneController.SetState(new ErrorState(_sceneController, ErrorStateBase.ErrorType.LeftTrackForTooLong));
        }
    }

    protected virtual void LogSuccess()
    {
        float walkingTime = Time.unscaledTime - _startedWalking;

        _sceneController.GetDataStorage().GetCurrectTrialData().Success = true;
        _sceneController.GetDataStorage().GetCurrectTrialData().ActualPathLength =
            _sceneController.GetUsersHead().GetComponent<PathLengthController>().GetPathLength();
        _sceneController.GetDataStorage().GetCurrectTrialData().Time = walkingTime;
        _sceneController.GetDataStorage().GetCurrectTrialData().TotalNumberOfGroundObstacles =
            _sceneController.GetTrack().GetComponent<ObstacleFactory>().GetTotalNumberOfGroundObstacles();
        _sceneController.GetDataStorage().GetCurrectTrialData().TotalNumberOfHighObstacles =
            _sceneController.GetTrack().GetComponent<ObstacleFactory>().GetTotalNumberOfHighObstacles();
        _sceneController.GetDataStorage().Save();
    }

    protected virtual void LogFailure(ErrorStateBase.ErrorType errorType)
    {
        _sceneController.GetDataStorage().GetCurrectTrialData().ActualPathLength =
            _sceneController.GetUsersHead().GetComponent<PathLengthController>().GetPathLength();
        _sceneController.GetDataStorage().GetCurrectTrialData().TotalNumberOfGroundObstacles =
            _sceneController.GetTrack().GetComponent<ObstacleFactory>().GetTotalNumberOfGroundObstacles();
        _sceneController.GetDataStorage().GetCurrectTrialData().TotalNumberOfHighObstacles =
            _sceneController.GetTrack().GetComponent<ObstacleFactory>().GetTotalNumberOfHighObstacles();
        _sceneController.GetDataStorage().GetCurrectTrialData().Note = errorType.ToString();
        _sceneController.GetDataStorage().Save();
    }
}
