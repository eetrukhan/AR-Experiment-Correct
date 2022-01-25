using System;
using System.Collections.Generic;
using UnityEngine;

public class Draggable2D : MonoBehaviour, I3DDragBeganHandler, I3DDragHandler, I3DDragEndedHandler
{
    // The quad has to be at the GroundPlane layer
    public GameObject Quad;

    public event Action On2DDragEnded;

    private Vector3 _lastHitPoint;
    private int _layerMask;

    void Awake()
    {
        if (Quad == null)
        {
            Debug.LogWarning("Warning: The quad hasn't been set up. Disabling the script");
            enabled = false;
            return;
        }

        _lastHitPoint = Vector3.negativeInfinity;
        _layerMask = LayerMask.GetMask("GroundPlane");
    }

    public void On3DDragBegan(Vector3 laserPointerOrigin, Vector3 laserPointerDirection)
    {
        if (!enabled)
            return;

        if (Physics.Raycast(laserPointerOrigin, laserPointerDirection, out RaycastHit hittestResult, Mathf.Infinity, _layerMask))
        {
            _lastHitPoint = hittestResult.point;
        }
    }

    public void On3DDrag(Vector3 laserPointerOrigin, Vector3 laserPointerDirection)
    {
        if (!enabled)
            return;

        if (Physics.Raycast(laserPointerOrigin, laserPointerDirection, out RaycastHit hittestResult, Mathf.Infinity, _layerMask))
        {
            if (_lastHitPoint != Vector3.negativeInfinity)
            {
                Vector3 diff = hittestResult.point - _lastHitPoint;
                transform.position += diff;
            }

            _lastHitPoint = hittestResult.point;
        }
    }

    public void On3DDragEnded(Vector3 laserPointerOrigin, Vector3 laserPointerDirection)
    {
        if (!enabled)
            return;

        if (Physics.Raycast(laserPointerOrigin, laserPointerDirection, out RaycastHit hittestResult, Mathf.Infinity, _layerMask))
        {
            if (_lastHitPoint != Vector3.negativeInfinity)
            {
                Vector3 diff = hittestResult.point - _lastHitPoint;
                transform.position += diff;
            }
        }

        _lastHitPoint = Vector3.negativeInfinity;
        On2DDragEnded();
    }
}
