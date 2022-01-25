
public class ErrorState : ErrorStateBase
{
    public ErrorState(ISceneController sceneController, ErrorType errorType, bool onStartingPosition = false) : base(sceneController, errorType, onStartingPosition) { }

    public override void TimeIsUp()
    {
        _sceneController.GetErrorMessage().SetActive(false);

        if (_onStartingPosition)
            _sceneController.SetState(new ReadyState(_sceneController, _startFrom));
        else
            _sceneController.SetState(new IdleState(_sceneController));
    }
}
