
public class ReadyState : ReadyStateBase
{
    public ReadyState(ISceneController sceneController, StartFrom startFrom) : base(sceneController, startFrom) { }

    public override void EnteredTrack()
    {
        CountDownController countDownController = _sceneController.GetTimerDisplay().GetComponent<CountDownController>();
        bool falseStart = !(countDownController.HasItExpired || countDownController.HasItAlmostExpired);

        _sceneController.GetTimerDisplay().SetActive(false);
        _sceneController.StopTimer();
        if (falseStart)
        {
            _sceneController.GetUsersHead().GetComponent<TrackOverrunHandler>().StopTracking();
            _sceneController.SetState(new ErrorState(_sceneController, ErrorStateBase.ErrorType.FalseStart));
        }
        else
            _sceneController.SetState(new WalkingState(_sceneController, _startFrom));
    }

    public override void TimeIsUp()
    {
        _sceneController.GetUsersHead().GetComponent<TrackOverrunHandler>().StopTracking();

        _sceneController.GetTimerDisplay().SetActive(false);
        _sceneController.GetMetronome().SetActive(false);
        _sceneController.SetState(new ErrorState(_sceneController, ErrorStateBase.ErrorType.MissedTheStart, _onStartingPosition));
    }
}
