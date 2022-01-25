using System;
using UnityEngine;

public class SyncedPoseExtrapolator : MonoBehaviour
{
    [SerializeField]
    private bool updateOnEveryFrame = false;

    [Header("Extrapolate To")]
    [SerializeField, Tooltip("Delta time")]
    private float secondsIntoTheFuture = 0.04f; // One twenty-fifth of a second

    private IExtrapolation extrapolationMethod;

    protected virtual void Start()
    {
        if ((extrapolationMethod = GetComponent<IExtrapolation>()) == null)
        {
            Debug.LogError("SyncedPoseExtrapolator: Couldn't find an 'IExtrapolation' component. Disabling the script");
            enabled = false;
            return;
        }
    }

    public virtual void UpdateTransform(Vector3 pos, Quaternion rot)
    {
        if (!((MonoBehaviour)extrapolationMethod).enabled)
        {
            transform.rotation = rot;
            transform.position = pos;
            return;
        }

        try
        {
            var pose = new RsPoseStreamTransformer.RsPose
            {
                translation = pos,
                rotation = rot
            };

            var predictedPose = extrapolationMethod.Extrapolate(pose, Time.unscaledTime, secondsIntoTheFuture);
            transform.rotation = predictedPose.rotation;
            transform.position = predictedPose.translation;
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }

    public void Update() // for testing purposes
    {
        if (!updateOnEveryFrame || !((MonoBehaviour)extrapolationMethod).enabled) return;

        try
        {
            var predictedPose = extrapolationMethod.Extrapolate(Time.unscaledTime, secondsIntoTheFuture);
            transform.rotation = predictedPose.rotation;
            transform.position = predictedPose.translation;
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }
}
