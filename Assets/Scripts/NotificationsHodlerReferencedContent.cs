using Logic;
using UnityEngine;

public class NotificationsHodlerReferencedContent : MonoBehaviour
{
    [Tooltip("The camera is needed to emulate a torso reference frame")]
    public GameObject Camera;

    [Tooltip("The distance from the camera that this object should be placed")]
    private float DistanceFromCamera = 10f;

    [Tooltip("Angle to the horizon")]
    public float AngleToTheHorizon = 8f;

    [Tooltip("Angle when tray should be shown")]
    public float TrayShowAngle = 35f;

    void OnEnable()
    {
        if (Camera == null)
        {
            Debug.LogError("Error: Camera is not set. Disabling the script.");
            enabled = false;
            return;
        }
    }

    void Start()
    {
        Vector3 posTo = Camera.transform.position + Camera.transform.forward * DistanceFromCamera;
        Quaternion rotTo = Quaternion.LookRotation(transform.position - Camera.transform.position);
        transform.rotation = rotTo;
        transform.position = posTo;
    }

    void Update()
    {
        Quaternion rotTo = Quaternion.LookRotation(transform.position - Camera.transform.position);
        if (Camera.transform.rotation.eulerAngles.x > 180 && Mathf.Abs(Camera.transform.rotation.eulerAngles.x - 360) >= TrayShowAngle)
        {
            EventManager.Broadcast(EVENT.ShowTray);
            return;
        }

        if (transform.childCount == 0)
        {
            Vector3 posTo = Camera.transform.position + Camera.transform.forward * DistanceFromCamera;
            if (Camera.transform.rotation.eulerAngles.x > 180 && Mathf.Abs(Camera.transform.rotation.eulerAngles.x - 360) > AngleToTheHorizon)
            {
                posTo.y = DistanceFromCamera * Mathf.Tan(Mathf.Deg2Rad * AngleToTheHorizon);
            }
            transform.rotation = rotTo;
            transform.position = posTo;
        }
        else
        {
            Vector3 posTo = transform.position;
            Quaternion oldRotTo = transform.rotation;
            if (Camera.transform.rotation.eulerAngles.x > 180 && Mathf.Abs(Camera.transform.rotation.eulerAngles.x - 360) > AngleToTheHorizon)
            {
                posTo.y = DistanceFromCamera * Mathf.Tan(Mathf.Deg2Rad * AngleToTheHorizon);
                transform.position = posTo;
                transform.rotation = oldRotTo;
            }
            else
            {
                Vector3 posRealTo = Camera.transform.position + Camera.transform.forward * DistanceFromCamera;                
                posTo.y = posRealTo.y;
                transform.position = posTo;
                oldRotTo.x = rotTo.x;
                transform.rotation = oldRotTo;
            }
        }
    }
}
