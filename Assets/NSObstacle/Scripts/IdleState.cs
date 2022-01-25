public class IdleState : State
{
    public IdleState(ISceneController sceneController) : base(sceneController) { }

    public override void OnStartingPosition(StartFrom startFrom)
    {
        _sceneController.SetState(new ReadyState(_sceneController, startFrom));
    }
}
