using UnityEngine;

/// <summary>
/// Keeps an object at a specific place associated with the anchor name
/// </summary>
public class RsARAnchor : MonoBehaviour
{
    enum UpdateMode
    {
        UntilItsResolved,
        EveryNthSecond
    }

#pragma warning disable 649
    [SerializeField]
    private RsDevice source;
    [Space]
    [SerializeField]
    private string anchorName;
#pragma warning restore 649
    [SerializeField]
    private UpdateMode updateMode = UpdateMode.UntilItsResolved;
    [Tooltip("In seconds")]
    [SerializeField]
    private float updatePeriod = 2f;

    private float lastUpdateTime = float.NegativeInfinity;

    protected void OnEnable()
    {
        if (source == null)
        {
            GameObject rsDeviceObj = GameObject.Find("RsDevice"); // Yeah, I know it isn't a good practice (better to use FindWithTag) but it is a safeguard mechanism so let it be. (c) John Lennon ;-)
            if (rsDeviceObj == null || (source = rsDeviceObj.GetComponent<RsDevice>()) == null)
            {
                Debug.LogError("RsARAnchor: Couldn't find the 'RsDevice' component in the scene. Disabling the script.");
                enabled = false;
                return;
            }
        }

        if (string.IsNullOrEmpty(anchorName))
        {
            Debug.LogError("RsARAnchor: The 'Anchor Name' field cannot be left unassigned. Disabling the script");
            enabled = false;
            return;
        }

        if (updatePeriod <= 0f)
        {
            Debug.LogError("RsARAnchor: The 'Update period' should be greater that 0. Disabling the script");
            enabled = false;
            return;
        }
    }

    protected void Update()
    {
        float elipsedTime = Time.unscaledTime - lastUpdateTime;
        if (elipsedTime < updatePeriod) return;

        if (source.GetARAnchor(anchorName, out Vector3 pos, out Quaternion rot))
        {
            transform.position = pos;
            transform.rotation = rot;

            if (updateMode == UpdateMode.UntilItsResolved)
            {
                Debug.Log($"The '{anchorName}' anchor is successfully resolved");
                enabled = false;
            }
        }
        lastUpdateTime = Time.unscaledTime;
    }
}
