using System;
using UnityEngine;

public class WalkingStateExp : WalkingState
{
    private bool _noInfoCondition;

    public WalkingStateExp(ISceneController sceneController, StartFrom startFrom) : base(sceneController, startFrom)
    {
        _noInfoCondition = _sceneController.GetDesignOption() == DesignOptions.NoInfo; //!(_sceneController.GetHMDAdministratedTask().gameObject.activeSelf || _sceneController.GetVideoTask().gameObject.activeSelf);
        if (_noInfoCondition) return;

        switch (_sceneController.GetHMDAdministeredTaskType())
        {
            case ExperimentSessionController.TaskType.Spheres:
                _sceneController.GetHMDAdministratedTask().gameObject.SetActive(true);
                _sceneController.GetHMDAdministratedTask().GenerateFiled();
                _sceneController.GetHMDAdministratedTask().SphereVelocity = _sceneController.GetSphereVelocity();
                _sceneController.GetDataStorage().GetCurrectTrialData().SphereVelocity =
                    _sceneController.GetHMDAdministratedTask().SphereVelocity;
                _sceneController.GetHMDAdministratedTask().Play();
                break;
            case ExperimentSessionController.TaskType.Video:
                _sceneController.GetVideoTask().Play((int)_sceneController.GetTrialNo() - 1);
                break;
            default:
                throw new NotImplementedException("Unimplemented task type");
        }
    }

    public override void StoppedAlongTheWay()
    {
        StopEverything();
        LogFailure(ErrorStateBase.ErrorType.StoppedAlongTheWay);

        _sceneController.SetState(new ErrorStateExp(_sceneController, ErrorStateBase.ErrorType.StoppedAlongTheWay));
    }

    public override void WentBackwards()
    {
        StopEverything();
        LogFailure(ErrorStateBase.ErrorType.WentBackwards);

        _sceneController.SetState(new ErrorStateExp(_sceneController, ErrorStateBase.ErrorType.WentBackwards));
    }

    public override void CrossedFinishLine()
    {
        StopEverything();
        LogSuccess();
        
        _sceneController.SetState(new DoneWalkingStateExp(_sceneController, _noInfoCondition));
    }

    public override void TimeIsUp()
    {
        if (_outOfTheTrack)
        {
            StopEverything();
            LogFailure(ErrorStateBase.ErrorType.LeftTrackForTooLong);

            _sceneController.SetState(new ErrorStateExp(_sceneController, ErrorStateBase.ErrorType.LeftTrackForTooLong));
        }
    }

    protected override void StopEverything()
    {
        base.StopEverything();

        _sceneController.GetHMDAdministratedTask().gameObject.SetActive(false);
        _sceneController.GetVideoTask().gameObject.SetActive(false);
    }

    protected override void LogSuccess()
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
        if (!_noInfoCondition)
            _sceneController.GetDataStorage().GetCurrectTrialData().RealCollisionsNumber =
                _sceneController.GetHMDAdministeredTaskType() == ExperimentSessionController.TaskType.Spheres ?
                _sceneController.GetHMDAdministratedTask().GetNumberOfColissions().ToString() :
                _sceneController.GetVideoTask().LastVideoName;
    }
}
