
public class ErrorStateExp : ErrorState
{
    private ErrorType _errorType;

    public ErrorStateExp(ISceneController sceneController, ErrorType errorType, bool onStartingPosition = false) : base(sceneController, errorType, onStartingPosition)
    {
        _errorType = errorType;

        _sceneController.SaveParams();
    }

    public override void TimeIsUp()
    {
        // Check whether we've finished with the current design
        if (_sceneController.GetTrialNo() == 1 && _errorType != ErrorType.MissedTheStart && _errorType != ErrorType.FalseStart)
        {
            _sceneController.GetTrack().SetActive(false);

            // Show message
            _sceneController.GetErrorMessageCaption().text = "Ура!";
            _sceneController.GetErrorMessageText().text = "На этом пока все. Передайте AR-шлем экспериментатору и передохните. Вы это заслужили!";

            _sceneController.SetState(new IdleStateExp(_sceneController));
            return;
        }

        _sceneController.GetErrorMessage().SetActive(false);

        if (_onStartingPosition)
            _sceneController.SetState(new ReadyStateExp(_sceneController, _startFrom));
        else
            _sceneController.SetState(new IdleStateExp(_sceneController));
    }
}
