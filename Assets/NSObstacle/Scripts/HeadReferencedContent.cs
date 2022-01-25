using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadReferencedContent : MonoBehaviour
{
    [Tooltip("The camera to which this object is locked to")]
    public GameObject Camera;

    [Tooltip("The distance from the camera that this object should be placed")]
    public float DistanceFromCamera = 0.75f;

    [Tooltip("If checked, makes the object move smoothly")]
    public bool SimulateInertia = false;

    [Tooltip("If unchecked, prevent the object from moving along the direction of walking")]
    public bool AllowMotionAlongZAxis = true;

    [Tooltip("The speed at which this object changes its position, if the inertia effect is enabled")]
    public float PositionLerpSpeed = 5f;

    [Tooltip("The speed at which this object changes its rotation, if the inertia effect is enabled")]
    public float RotationLerpSpeed = 5f;

    [Tooltip("If disabled, the object will rotate with the camera in the XY-plane")]
    public bool ParallelToTheGround = true;

    private Vector3 _lastCameraPosition;

    /// <summary>
    /// Ensures that everying is ready.
    /// </summary>
    void OnEnable()
    {
        // Disable the script if there is no camera
        if (Camera == null)
        {
            Debug.LogError("Error: HeadReferencedContent.Camera is not set. Disabling the script.");
            enabled = false;
            return;
        }

        // Preventing the object from flying around when it's just been activated
        transform.position = Camera.transform.position + (Camera.transform.forward * DistanceFromCamera);
        if (SimulateInertia && !AllowMotionAlongZAxis)
            _lastCameraPosition = Camera.transform.position;

        Vector3 upwards = ParallelToTheGround ? Vector3.up : Camera.transform.up;
        transform.rotation = Quaternion.LookRotation(transform.position - Camera.transform.position, upwards);
    }

    /// <summary>
    /// Update position and rotation of this object to face the camera
    /// </summary>
    void Update()
    {
        if (SimulateInertia && !AllowMotionAlongZAxis)
        {
            Vector3 deltaCameraPosition = Camera.transform.position - _lastCameraPosition;
            _lastCameraPosition = Camera.transform.position;

            Vector3 parallelTransport = transform.position + deltaCameraPosition;

            Vector3 currentObjectOrientation = parallelTransport - Camera.transform.position;
            Quaternion currentRotation = Quaternion.LookRotation(currentObjectOrientation);
            Quaternion requiredRotation = Quaternion.LookRotation(Camera.transform.forward); // == Camera.transform.rotation

            float posSpeed = Time.deltaTime * PositionLerpSpeed;
            Vector3 smoothedObjectOrientation = Quaternion.Slerp(currentRotation, requiredRotation, posSpeed) * (Vector3.forward * DistanceFromCamera);

            transform.position = Camera.transform.position + smoothedObjectOrientation;

            Vector3 upwards = ParallelToTheGround ? Vector3.up : Camera.transform.up;
            Quaternion rotTo = Quaternion.LookRotation(smoothedObjectOrientation, upwards);

            float rotSpeed = Time.deltaTime * RotationLerpSpeed;
            transform.rotation = Quaternion.Slerp(transform.rotation, rotTo, rotSpeed);
        }
        else
        {
            Vector3 posTo = Camera.transform.position + (Camera.transform.forward * DistanceFromCamera);

            Vector3 upwards = ParallelToTheGround ? Vector3.up : Camera.transform.up;
            Quaternion rotTo = Quaternion.LookRotation(transform.position - Camera.transform.position, upwards);

            if (SimulateInertia)
            {
                float posSpeed = Time.deltaTime * PositionLerpSpeed;
                transform.position = Vector3.SlerpUnclamped(transform.position, posTo, posSpeed);

                float rotSpeed = Time.deltaTime * RotationLerpSpeed;
                transform.rotation = Quaternion.Slerp(transform.rotation, rotTo, rotSpeed);
            }
            else
            {
                transform.position = posTo;

                transform.rotation = rotTo;
            }
        }
    }

    public void SetDistanceFromCamera(float value)
    {
        DistanceFromCamera = value;
    }

    public void SetParallelToTheGround(bool value)
    {
        ParallelToTheGround = value;
    }
}
