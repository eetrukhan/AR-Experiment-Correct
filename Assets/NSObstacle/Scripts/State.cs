using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State
{
    protected ISceneController _sceneController;

    public State(ISceneController sceneController)
    {
        _sceneController = sceneController;
    }

    public virtual void OnStartingPosition(StartFrom startFrom) { }

    public virtual void LeftStartingPosition() { }

    public virtual void EnteredTrack() { }

    public virtual void StoppedAlongTheWay() { }

    public virtual void WentBackwards() { }

    public virtual void LeftTrack() { }

    public virtual void BackOnTrack() { }

    public virtual void CrossedFinishLine() { }

    public virtual void EnteredCollisionNumber(uint collisionsNumberReported) { }

    public virtual void TimeIsUp() { }
}
