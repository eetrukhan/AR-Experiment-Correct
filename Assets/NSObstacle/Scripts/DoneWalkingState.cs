
public class DoneWalkingState : DoneWalkingStateBase
{
    public DoneWalkingState(ISceneController sceneController) : base(sceneController) { }

    public override void TimeIsUp()
    {
        if (_onStartingPosition)
        {
            _sceneController.SetState(new ReadyState(_sceneController, _startFrom));
            return;
        }
        else
        {
            _sceneController.SetState(new IdleState(_sceneController));
        }
    }
}
