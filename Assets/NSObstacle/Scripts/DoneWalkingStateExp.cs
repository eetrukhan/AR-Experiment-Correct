using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoneWalkingStateExp : State
{
    protected bool _onStartingPosition;
    protected bool _waitTillCollisionsNumberIsEntered = true;
    protected StartFrom _startFrom;

    private const float DELAY_SEC = 3;

    public DoneWalkingStateExp(ISceneController sceneController, bool noInfoCondition) : base(sceneController)
    {
        if (noInfoCondition)
        {
            SaveEverything();

            _waitTillCollisionsNumberIsEntered = false;
            _sceneController.SetTimerTo(DELAY_SEC);
            return;
        }

        _sceneController.GetDigitalKeypad().SetActive(true);
        _sceneController.GetReticle().SetActive(true);
        _waitTillCollisionsNumberIsEntered = true;
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

    public override void EnteredCollisionNumber(uint collisionsNumberReported)
    {
        _sceneController.GetDigitalKeypad().SetActive(false);
        _sceneController.GetReticle().SetActive(false);

        _sceneController.GetDataStorage().GetCurrectTrialData().CollisionsNumberReported = collisionsNumberReported;
        SaveEverything();

        _sceneController.SetState(new IdleStateExp(_sceneController));
    }

    public override void TimeIsUp()
    {
        if (!_waitTillCollisionsNumberIsEntered)
        {
            if (_onStartingPosition)
            {
                _sceneController.SetState(new ReadyStateExp(_sceneController, _startFrom));
                return;
            }
            else
            {
                _sceneController.SetState(new IdleStateExp(_sceneController));
            }
        }
    }

    private void SaveEverything()
    {
        _sceneController.GetDataStorage().Save();
        _sceneController.SaveParams();

        // Check whether we've finished with the current design
        if (_sceneController.GetTrialNo() == 1)
        {
            _sceneController.GetTrack().SetActive(false);

            // Show message
            _sceneController.GetErrorMessageCaption().text = "Ура!";
            _sceneController.GetErrorMessageText().text = "На этом пока все. Передайте AR-шлем экспериментатору и передохните. Вы это заслужили!";
            _sceneController.GetErrorMessage().SetActive(true);
        }
    }
}
