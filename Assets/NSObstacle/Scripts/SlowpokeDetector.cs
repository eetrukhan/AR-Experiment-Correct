using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowpokeDetector : MonoBehaviour
{
    public Transform Camera;

    public float ThresholdSpeed = 1f; // Meters per second
    public float RepeatRate = 1f;

    public event Action OnSlowpokeDetected;

    private Vector3 _previousPosition;

    void Start()
    {
        if (Camera == null)
            Debug.LogError("Error: The Camera can't be left unassigned");
    }

    void OnEnable()
    {
        if (Camera != null)
        {
            InvokeRepeating("CheckVelocity", RepeatRate, RepeatRate);
            _previousPosition = Camera.position;
        }
    }

    void OnDisable()
    {
        CancelInvoke("CheckVelocity");
    }

    private void CheckVelocity()
    {
        if (Camera != null)
        {
            if ((Camera.position - _previousPosition).magnitude < ThresholdSpeed)
                OnSlowpokeDetected();

            _previousPosition = Camera.position;
        }
    }
}
