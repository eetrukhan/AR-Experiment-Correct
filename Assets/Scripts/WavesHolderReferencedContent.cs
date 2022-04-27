using Logic;
using UnityEngine;

public class WavesHolderReferencedContent : MonoBehaviour
{   
    [Tooltip("The camera is needed to emulate a torso reference frame")]
    public GameObject Camera;

    [Tooltip("Angle when tray should be shown")]
    public float TrayShowAngle = 20f;

    void OnEnable()
    {
        if (Camera == null)
        {
            Camera = GameObject.FindWithTag("MainCamera");
            transform.GetComponentInChildren<Canvas>().worldCamera = Camera.GetComponent<Camera>();
        }
    }

    void Update()
    {
        //if (Camera.transform.rotation.eulerAngles.x > 180 && Mathf.Abs(Camera.transform.rotation.eulerAngles.x - 360) >= TrayShowAngle)
       // if ((Camera.transform.rotation.eulerAngles.x > 180 && Mathf.Abs(Camera.transform.rotation.eulerAngles.x - 360) > TrayShowAngle)
       //     ||(Camera.transform.rotation.eulerAngles.x <= 180 && Camera.transform.rotation.eulerAngles.x < Mathf.Abs(TrayShowAngle)))
       if (Camera.transform.rotation.eulerAngles.x > 180 && Mathf.Abs(Camera.transform.rotation.eulerAngles.x - 360) >= TrayShowAngle)
        {
            Debug.Log("Showing");
            EventManager.Broadcast(EVENT.ShowTray);
            Debug.Log(Camera.transform.rotation.eulerAngles.x);
            return;
        }

       Vector3 pos = Camera.transform.position + Camera.transform.forward; 
       transform.position = pos;
       transform.rotation = Quaternion.LookRotation(transform.position - Camera.transform.position);
    }
}