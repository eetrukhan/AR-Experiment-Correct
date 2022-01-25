using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TrackTransformPreserver))]
public class TrackPoseController : MonoBehaviour
{
#pragma warning disable 649
    [SerializeField]
    private Transform _camera;
    [SerializeField]
    private Transform _track;
    [SerializeField]
    private Transform _handleAtTheEnd;
    private float _halfTrackLength;
    [SerializeField]
    private float _verticalOffset = 1.7f;
    [SerializeField]
    private float _heightAdjustmentSpeed = 0.1f;
    [SerializeField]
    private float _rotationAdjustmentSpeed = 0.1f;
#pragma warning restore 649

    public enum PlacementMode
    {
        TrackProjection,
        PoseAdjustment
    }
    private PlacementMode _mode = PlacementMode.TrackProjection;
    public PlacementMode Mode { get => _mode; set => _mode = value; }

    protected void Start()
    {
        if (_camera == null)
        {
            Debug.LogError("TrackPoseController: The 'Camera' field can't be left unassigned. Disabling the script");
            enabled = false;
            return;
        }

        if (_track == null)
        {
            Debug.LogError("TrackPoseController: The 'Track' field can't be left unassigned. Disabling the script");
            enabled = false;
            return;
        }

        if (_handleAtTheEnd == null && (_handleAtTheEnd = _track.Find("HandleAtTheEnd")) == null)
        {
            Debug.LogError("TrackPoseController: Couldn't find the 'HandleAtTheEnd' object inside the track. Disabling the script");
            enabled = false;
            return;
        }

        _halfTrackLength = _handleAtTheEnd.localPosition.z;
    }

    protected void Update()
    {
        if (!enabled)
            return;

        switch (_mode)
        {
            case PlacementMode.TrackProjection:
                UpdateTrackPose();
                break;
            case PlacementMode.PoseAdjustment:
                float translation = Input.GetAxis("Vertical") * _heightAdjustmentSpeed;
                float rotation = Input.GetAxis("Horizontal") * _rotationAdjustmentSpeed;

                // Make it move 10 meters per second instead of 10 meters per frame...
                translation *= Time.deltaTime;
                // The same with the rotation
                rotation *= Time.deltaTime;

                // Move translation along the object's y-axis
                transform.Translate(0, translation, 0);
                // Rotate the track around x-axis
                transform.Rotate(rotation, 0, 0);
                break;
        }
    }

    private void UpdateTrackPose()
    {
        // Firstly, we figure out the direction the user's facing regardless tilt and roll
        Vector3 cameraEulerAngles = _camera.rotation.eulerAngles;
        Vector3 direction = Quaternion.Euler(0, cameraEulerAngles.y, 0) * Vector3.forward;

        // Then we calculate offset that consists of two components
        Vector3 offset = direction * _halfTrackLength;
        offset.y -= _verticalOffset;

        // Finally, we apply the transform
        transform.position = _camera.position + offset;
        transform.rotation = Quaternion.Euler(0, cameraEulerAngles.y, 0);
    }
}
