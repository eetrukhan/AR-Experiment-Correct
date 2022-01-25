
public class IdleStateBase : State
{
    public IdleStateBase(ISceneController sceneController) : base(sceneController)
    {
        /*_sceneController.GetTrackStartingPosition(StartFrom.TheBeginning).SetActive(true);
        _sceneController.GetTrackStartingPosition(StartFrom.TheEnd).SetActive(true);
        _sceneController.GetTrackBorder(StartFrom.TheBeginning).SetActive(false);
        _sceneController.GetTrackBorder(StartFrom.TheEnd).SetActive(false);*/
    }

    public override void OnStartingPosition(StartFrom startFrom)
    {
        _sceneController.SetState(new ReadyStateBase(_sceneController, startFrom));
    }
}
