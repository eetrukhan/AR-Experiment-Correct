using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoneWalkingStateBase : State
{
    protected bool _onStartingPosition;
    protected StartFrom _startFrom;

    private const float DELAY_SEC = 3;

    public DoneWalkingStateBase(ISceneController sceneController) : base(sceneController)
    {
        _sceneController.SetTimerTo(DELAY_SEC);
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
        if (_onStartingPosition)
        {
            _sceneController.SetState(new ReadyStateBase(_sceneController, _startFrom));
            return;
        }
        else
        {
            _sceneController.SetState(new IdleStateBase(_sceneController));
        }
    }
}
