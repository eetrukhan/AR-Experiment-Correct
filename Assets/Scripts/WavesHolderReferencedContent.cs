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
        if (Camera.transform.rotation.eulerAngles.x > 180 && Mathf.Abs(Camera.transform.rotation.eulerAngles.x - 360) >= TrayShowAngle)
        {
            EventManager.Broadcast(EVENT.ShowTray);
            return;
        }

        transform.position = Camera.transform.position + Camera.transform.forward;
        transform.rotation = Quaternion.LookRotation(transform.position - Camera.transform.position);
    }
}