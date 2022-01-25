
public class IdleStateExp : State
{
    public IdleStateExp(ISceneController sceneController) : base(sceneController) { }

    public override void OnStartingPosition(StartFrom startFrom)
    {
        _sceneController.SetState(new ReadyStateExp(_sceneController, startFrom));
    }
}
