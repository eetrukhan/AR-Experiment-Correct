using Logic;
using UnityEngine;

public class NotificationsHodlerReferencedContent : MonoBehaviour
{
    [Tooltip("The camera is needed to emulate a torso reference frame")]
    public GameObject Camera;

    [Tooltip("The distance from the camera that this object should be placed")]
    private float DistanceFromCamera = 1f; //10f

    [Tooltip("Angle to the horizon")]
    public float AngleToTheHorizon = 8f;   // ок на -11 было + чуть сдвинут сам холдер

    [Tooltip("Angle when tray should be shown")]
    public float TrayShowAngle = 35f;

    private Vector3 minusPos = new Vector3(0,0f,0);
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
        transform.position = posTo+minusPos;//
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
            transform.position = posTo + minusPos ; // 
        }
        else
        {
            Vector3 posTo = transform.position;
            Quaternion oldRotTo = transform.rotation;
            // Debug.Log("Camera.transform.rotation.eulerAngles.x: "+Camera.transform.rotation.eulerAngles.x+"; AngleToTheHorizon" + AngleToTheHorizon);
            //if (Camera.transform.rotation.eulerAngles.x > 180 && Mathf.Abs(Camera.transform.rotation.eulerAngles.x - 360) > AngleToTheHorizon)
            if ((Camera.transform.rotation.eulerAngles.x > 180 && Mathf.Abs(Camera.transform.rotation.eulerAngles.x - 360) > AngleToTheHorizon)
                ||(Camera.transform.rotation.eulerAngles.x <= 180 && Camera.transform.rotation.eulerAngles.x < Mathf.Abs(AngleToTheHorizon)))
            {
                posTo.y = DistanceFromCamera * Mathf.Tan(Mathf.Deg2Rad * AngleToTheHorizon);
                transform.position = posTo + minusPos; //
                transform.rotation = oldRotTo;
            }
            else
            {
                Vector3 posRealTo = Camera.transform.position + Camera.transform.forward * DistanceFromCamera;                
                posTo.y = posRealTo.y;
                transform.position = posTo + minusPos;//
                oldRotTo.x = rotTo.x;
                transform.rotation = oldRotTo;
            }
        }
        
    }
}
