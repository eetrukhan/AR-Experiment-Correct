using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepAtEyeLevel : MonoBehaviour
{
    public Transform Camera;

    [Tooltip("How much lower the top border of the canvas should be regarding the eye level (in meters)")]
    public float LowerDown = 0f;

    [Tooltip("If checked, makes the object move smoothly")]
    public bool SimulateInertia = false;

    [Tooltip("The speed at which this object changes its position, if the inertia effect is enabled")]
    public float PositionLerpSpeed = 5f;

    private float _verticalOffset;

    void Start()
    {
        // Disable the script if there is no camera
        if (Camera == null)
        {
            Debug.LogError("Error: The Camera hasn't been set. Disabling the script.");
            enabled = false;
            return;
        }

        // Calculate an offset that will allow to lower the canvas down so that its top border is at the eye level
        RectTransform t = (RectTransform) transform;
        float canvasHeightInMeters = t.sizeDelta.y / 1000f;
        _verticalOffset = - canvasHeightInMeters / 2f;
    }

    // Update is called once per frame
    void Update()
    {
        if (!enabled)
            return;

        Vector3 posTo = transform.position;
        posTo.y = Camera.position.y + _verticalOffset - LowerDown;

        if (SimulateInertia)
        {
            float posSpeed = Time.deltaTime * PositionLerpSpeed;
            transform.position = Vector3.SlerpUnclamped(transform.position, posTo, posSpeed);
        }
        else
        {
            transform.position = posTo;
        }
    }
}
