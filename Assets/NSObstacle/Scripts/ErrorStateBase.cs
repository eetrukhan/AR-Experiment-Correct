
public class ErrorStateBase : State
{
    public enum ErrorType
    {
        MissedTheStart,
        StoppedAlongTheWay,
        WentBackwards,
        LeftTrackForTooLong,
        FalseStart
    }

    protected bool _onStartingPosition;
    protected StartFrom _startFrom;

    private const float DELAY_SEC = 5;

    public ErrorStateBase(ISceneController sceneController, ErrorType errorType, bool onStartingPosition = false) : base(sceneController)
    {
        _onStartingPosition = onStartingPosition;

        _sceneController.SetTimerTo(DELAY_SEC);

        switch (errorType)
        {
            case ErrorType.MissedTheStart:
                _sceneController.GetErrorMessageText().text = "К сожалению, вы пропустили момент начала движения. Пожалуйста, попробуйте снова.";
                break;
            case ErrorType.StoppedAlongTheWay:
                _sceneController.GetErrorMessageText().text = "Вы остановились, а этого делать нельзя :-( Пожалуйста, начните сначала.";
                break;
            case ErrorType.WentBackwards:
                _sceneController.GetErrorMessageText().text = "Судя по всему, вы пошли задом наперед( Вернитесь, пожалуйста, на стартовую позицию и попробуйте снова.";
                break;
            case ErrorType.LeftTrackForTooLong:
                _sceneController.GetErrorMessageText().text = "Вы покинули трек слишком надолго, а этого делать нельзя. Пожалуйста, начните сначала.";
                break;
            case ErrorType.FalseStart:
                _sceneController.GetErrorMessageText().text = "Фальстарт :-( Вернитесь, пожалуйста, на стартовую позицию и попробуйте снова.";
                break;
        }
        _sceneController.GetErrorMessage().SetActive(true);
    }

    public override void OnStartingPosition(StartFrom startFrom)
    {
        _startFrom = startFrom;
        _onStartingPosition = true;
    }

    public override void LeftStartingPosition()
    {
        _onStartingPosition = false;
    }

    public override void TimeIsUp()
    {
        _sceneController.GetErrorMessage().SetActive(false);

        if (_onStartingPosition)
            _sceneController.SetState(new ReadyStateBase(_sceneController, _startFrom));
        else
            _sceneController.SetState(new IdleStateBase(_sceneController));
    }
}
