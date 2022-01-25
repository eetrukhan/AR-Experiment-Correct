
public class ReadyStateBase : State
{
    protected StartFrom _startFrom;
    protected bool _onStartingPosition;

    private const float DELAY_SEC = 4;

    public ReadyStateBase(ISceneController sceneController, StartFrom startFrom) : base(sceneController)
    {
        _sceneController.SetTimerTo(DELAY_SEC);

        // Showing a finish line and hiding a starting position on the opposite side of the track 
        /*_sceneController.GetTrackBorder(startFrom == StartFrom.TheBeginning ? StartFrom.TheEnd : StartFrom.TheBeginning).SetActive(true);
        _sceneController.GetTrackStartingPosition(startFrom == StartFrom.TheBeginning ? StartFrom.TheEnd : StartFrom.TheBeginning).SetActive(false);*/

        // Traking care of existing obstacles and starting to track the subject position on the track
        _sceneController.GetTrack().GetComponent<ObstacleFactory>().Destroy();
        _sceneController.GetUsersHead().GetComponent<TrackOverrunHandler>().StartTracking(startFrom);

        _startFrom = startFrom;
        _onStartingPosition = true;
        _sceneController.GetTimerDisplay().SetActive(true);
    }

    public override void OnStartingPosition(StartFrom startFrom)
    {
        _onStartingPosition = true;
    }

    public override void LeftStartingPosition()
    {
        _onStartingPosition = false;
    }

    public override void EnteredTrack()
    {
        CountDownController countDownController = _sceneController.GetTimerDisplay().GetComponent<CountDownController>();
        bool falseStart = !(countDownController.HasItExpired || countDownController.HasItAlmostExpired);

        _sceneController.StopTimer();
        if (falseStart)
        {
            _sceneController.GetTimerDisplay().SetActive(false);
            _sceneController.GetUsersHead().GetComponent<TrackOverrunHandler>().StopTracking();
            _sceneController.SetState(new ErrorStateBase(_sceneController, ErrorStateBase.ErrorType.FalseStart));
        }
        else
        {
            if (countDownController.HasItExpired)
                _sceneController.GetTimerDisplay().SetActive(false);
            _sceneController.SetState(new WalkingStateBase(_sceneController, _startFrom));
        }
    }

    public override void TimeIsUp()
    {
        _sceneController.GetUsersHead().GetComponent<TrackOverrunHandler>().StopTracking();

        _sceneController.GetTimerDisplay().SetActive(false);
        _sceneController.GetMetronome().SetActive(false);
        _sceneController.SetState(new ErrorStateBase(_sceneController, ErrorStateBase.ErrorType.MissedTheStart, _onStartingPosition));
    }
}
