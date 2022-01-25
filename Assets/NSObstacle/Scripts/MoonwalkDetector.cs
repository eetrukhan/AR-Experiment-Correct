using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonwalkDetector : MonoBehaviour
{
    public Transform Camera;
    public Transform Track;

    public float RepeatRate = 0.5f;

    public event Action OnMoonwalkDetected;

    private Vector3 _correctMovementDirection;
    private Vector3 _previousPosition;

    void Start()
    {
        if (Camera == null)
            Debug.LogError("Error: The Camera can't be left unassigned");

        if (Track == null)
            Debug.LogError("Error: The Track can't be left unassigned");
    }

    public bool StartDetecting(StartFrom startFrom)
    {
        if (Camera == null)
            return false;

        InvokeRepeating("CheckMovementDirection", RepeatRate, RepeatRate);
        _correctMovementDirection = startFrom == StartFrom.TheBeginning ? Track.forward : -Track.forward;
        _previousPosition = Camera.position;
        return true;
    }

    public void StopDetecting()
    {
        CancelInvoke("CheckMovementDirection");
    }

    private void CheckMovementDirection()
    {
        if (Camera != null)
        {
            Vector3 movement = Camera.position - _previousPosition;
            if (Vector3.Dot(movement, _correctMovementDirection) < 0)
                OnMoonwalkDetected();

            _previousPosition = Camera.position;
        }
    }
}
