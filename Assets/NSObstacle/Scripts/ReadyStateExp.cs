using System;
using System.Collections.Generic;

public class ReadyStateExp : ReadyState
{
    public ReadyStateExp(ISceneController sceneController, StartFrom startFrom) : base(sceneController, startFrom)
    {
        List<TorsoReferencedContent> torsoReferencedContent = new List<TorsoReferencedContent>(3);
        torsoReferencedContent.Add(_sceneController.GetHMDAdministratedTask().GetComponent<TorsoReferencedContent>());
        torsoReferencedContent.Add(_sceneController.GetVideoTask().GetComponent<TorsoReferencedContent>());
        torsoReferencedContent.Add(_sceneController.GetTimerDisplay().GetComponent<TorsoReferencedContent>());
        List<HeadReferencedContent> headReferencedContent = new List<HeadReferencedContent>(3);
        headReferencedContent.Add(_sceneController.GetHMDAdministratedTask().GetComponent<HeadReferencedContent>());
        headReferencedContent.Add(_sceneController.GetVideoTask().GetComponent<HeadReferencedContent>());
        headReferencedContent.Add(_sceneController.GetTimerDisplay().GetComponent<HeadReferencedContent>());

        switch (_sceneController.GetDesignOption())
        {
            case DesignOptions.NoInfo:
            case DesignOptions.HUD:
                torsoReferencedContent.ForEach(component => component.enabled = false);
                headReferencedContent.ForEach(component => component.enabled = true);
                break;
            case DesignOptions.TorsoRef35:
                headReferencedContent.ForEach(component => component.enabled = false);
                torsoReferencedContent.ForEach(component =>
                {
                    component.Pitch = 35;
                    component.enabled = true;
                });
                break;
            default:
                throw new NotImplementedException("Unimplemented design option");
        }

        if (_sceneController.GetDesignOption() == DesignOptions.NoInfo) return;

        switch (_sceneController.GetHMDAdministeredTaskType())
        {
            case ExperimentSessionController.TaskType.Spheres:
                // We do that later now
                //_sceneController.GetHMDAdministratedTask().gameObject.SetActive(true);
                //_sceneController.GetHMDAdministratedTask().GenerateFiled();
                break;
            case ExperimentSessionController.TaskType.Video:
                _sceneController.GetVideoTask().gameObject.SetActive(true);
                _sceneController.GetVideoTask().PrepareFragment((int)_sceneController.GetTrialNo() - 1);
                break;
            default:
                throw new NotImplementedException("Unimplemented task type");
        }
    }

    public override void EnteredTrack()
    {
        CountDownController countDownController = _sceneController.GetTimerDisplay().GetComponent<CountDownController>();
        bool falseStart = !(countDownController.HasItExpired || countDownController.HasItAlmostExpired);

        _sceneController.GetTimerDisplay().SetActive(false);
        _sceneController.StopTimer();
        if (!falseStart)
        {
            _sceneController.SetState(new WalkingStateExp(_sceneController, _startFrom));
            return;
        }

        // In case of a false start
        _sceneController.GetUsersHead().GetComponent<TrackOverrunHandler>().StopTracking();
        _sceneController.GetHMDAdministratedTask().gameObject.SetActive(false);
        _sceneController.GetVideoTask().gameObject.SetActive(false);

        _sceneController.SetState(new ErrorStateExp(_sceneController, ErrorStateBase.ErrorType.FalseStart));
    }

    public override void TimeIsUp()
    {
        _sceneController.GetUsersHead().GetComponent<TrackOverrunHandler>().StopTracking();

        _sceneController.GetTimerDisplay().SetActive(false);
        _sceneController.GetHMDAdministratedTask().gameObject.SetActive(false);
        _sceneController.GetVideoTask().gameObject.SetActive(false);
        _sceneController.GetMetronome().SetActive(false);
        _sceneController.SetState(new ErrorStateExp(_sceneController, ErrorStateBase.ErrorType.MissedTheStart, _onStartingPosition));
    }
}
