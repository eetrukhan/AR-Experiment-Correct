using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.XR.MagicLeap;

public class TrackPositionAdjuster : MonoBehaviour
{
    //public MLPersistentBehavior PersistentBehavior;

    public Transform Track;

    public Transform HandleBeginning;
    public Transform HandleEnd;

    private Vector3 _upwards;
    private float _distFromTrackToHandle;

    private const float MIN_TRACK_LENGTH = 0.01f; // 1 cm

    void Awake()
    {
        if (!HandleBeginning || !HandleEnd)
        {
            Debug.LogError("Error: The handle objects haven't been set up. Disabling the script");
            enabled = false;
            return;
        }

        /*if (PersistentBehavior == null)
        {
            Debug.LogError("Error: The PersistentBehavior field can't be left unassigned. Disabling the script");
            enabled = false;
            return;
        }

        Draggable2D draggable2D = HandleBeginning.GetComponent<Draggable2D>();
        draggable2D.On2DDragEnded += PersistentBehavior.UpdateBinding;
        draggable2D = HandleEnd.GetComponent<Draggable2D>();
        draggable2D.On2DDragEnded += PersistentBehavior.UpdateBinding;*/

        // If the track isn't scecified, consider that this scripf is attached to the track GO
        if (Track == null)
            Track = transform;

        // We keep the initial orientation of the Z axis
        _upwards = Track.TransformDirection(Vector3.up);

        // We assume that the track GO is in between handles
        float distBetweenTwoHandles = (HandleEnd.localPosition - HandleBeginning.localPosition).magnitude;
        float trackLength = Track.localScale.z;
        _distFromTrackToHandle = (distBetweenTwoHandles - trackLength) / 2;
    }

    void Update()
    {
        // If the position of at least one of the two handles has changed since the last time
        if (HandleBeginning.hasChanged || HandleEnd.hasChanged)
        {
            // Update the pose of the track
            Track.position = (HandleBeginning.position + HandleEnd.position) / 2;

            Vector3 forward = HandleEnd.position - HandleBeginning.position;
            Track.rotation = Quaternion.LookRotation(forward, _upwards);

            Vector3 scale = Track.localScale;
            float distBetweenTwoHandles = (HandleEnd.localPosition - HandleBeginning.localPosition).magnitude;
            scale.z = distBetweenTwoHandles - 2 * _distFromTrackToHandle;
            // Colliders do not support negative values
            if (scale.z < MIN_TRACK_LENGTH)
                scale.z = MIN_TRACK_LENGTH;
            Track.localScale = scale;

            HandleBeginning.hasChanged = false;
            HandleEnd.hasChanged = false;
        }
    }
}
