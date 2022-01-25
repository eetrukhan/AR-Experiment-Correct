using UnityEngine;

public class PathLengthController : MonoBehaviour
{
    [SerializeField]
    protected bool _suppressYMovements = true;

    protected Vector3 _lastKnownPosition;
    protected float _pathLength = -1;

    private static readonly float UPDATE_INTERVAL = 0.2f;

    protected virtual void OnEnable()
    {
        _pathLength = 0;
        _lastKnownPosition = transform.position;

        InvokeRepeating("UpdatePathLength", UPDATE_INTERVAL, UPDATE_INTERVAL);
    }

    protected virtual void OnDisable()
    {
        CancelInvoke();
        UpdatePathLength();
    }

    public float GetPathLength()
    {
        if (isActiveAndEnabled)
            UpdatePathLength();

        return _pathLength;
    }

    private void UpdatePathLength()
    {
        Vector3 movement = transform.position - _lastKnownPosition;
        if (_suppressYMovements)
            movement.y = 0f;
        _pathLength += movement.magnitude;

        _lastKnownPosition = transform.position;
    }
}
