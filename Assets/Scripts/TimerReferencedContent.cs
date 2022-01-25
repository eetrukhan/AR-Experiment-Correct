using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerReferencedContent : MonoBehaviour
{
    public enum Rotate
    {
        AroundYAxisOnly,
        In3D
    }

    [Tooltip("The camera this object should face")]
    public Transform Camera;

    [Tooltip("Type of billboarding behaviour")]
    public Rotate Rotation = Rotate.In3D;

    [Tooltip("If checked, makes the object rotate smoothly")]
    public bool SimulateInertia = false;

    [Tooltip("The speed at which this object changes its rotation, if the inertia effect is enabled")]
    public float RotationLerpSpeed = 5f;

    void Start()
    {
        // Disable the script if there is no camera
        if (Camera == null)
        {
            Debug.LogError("Error: The Camera field hasn't been set. Disabling the script.");
            enabled = false;
            return;
        }
    }

    void Update()
    {
        Vector3 cameraPosition = Camera.position;
        if (Rotation == Rotate.AroundYAxisOnly)
            cameraPosition.y = transform.position.y;
        Quaternion rotTo = Quaternion.LookRotation(transform.position - cameraPosition);

        if (SimulateInertia)
        {
            float rotSpeed = Time.deltaTime * RotationLerpSpeed;
            transform.rotation = Quaternion.Slerp(transform.rotation, rotTo, rotSpeed);
        }
        else
            transform.rotation = rotTo;
    }
}
