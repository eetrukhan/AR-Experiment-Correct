using Logic;
using UnityEngine;

public class TrayHolderReferencedContent : MonoBehaviour
{
    [Tooltip("The camera is needed to emulate a torso reference frame")]
    public GameObject Camera;

    [Tooltip("If checked, makes object move smoothly")]
    public bool SimulateInertia = false;

    [Tooltip("The speed at which this object changes its position, if the inertia effect is enabled")]
    public float LerpSpeed = 2f;

    [Tooltip("Angle when tray should be hiden")]
    public float TrayHideAngle = -25f;

    void OnEnable()
    {
        if (Camera == null)
        {
            Debug.LogError("Error: Camera is not set. Disabling the script.");
            enabled = false;
            return;
        }
    }

    void Update()
    {
//      Debug.Log("Ang: " + Camera.transform.rotation.eulerAngles.x + " " + (Camera.transform.rotation.eulerAngles.x < 180) + " " + (Camera.transform.rotation.eulerAngles.x >= Mathf.Abs(TrayHideAngle)) + " " + TrayHideAngle);
        Vector3 posTo = Camera.transform.position;
        if (Camera.transform.rotation.eulerAngles.x < 180 && Camera.transform.rotation.eulerAngles.x >= Mathf.Abs(TrayHideAngle))
        {
            EventManager.Broadcast(EVENT.HideTray);
            Debug.Log("Hiding");
            Debug.Log(Camera.transform.rotation.eulerAngles.x);
            return;
        }

        //
        float DistanceFromCamera = 3f;
        Vector3 AposTo = Camera.transform.position + (Camera.transform.forward * DistanceFromCamera);

        bool ParallelToTheGround = true;
        Vector3 upwards = ParallelToTheGround ? Vector3.up : Camera.transform.up;
        Quaternion rotTo = Quaternion.LookRotation(transform.position - Camera.transform.position, upwards);
        //
        /*  if (SimulateInertia)
          {
              float posSpeed = Time.deltaTime * LerpSpeed;
              transform.position = Vector3.SlerpUnclamped(transform.position, posTo, posSpeed);
          }
          else
          {
              transform.position = posTo;
          }
          */
      //transform.position = posTo;
      transform.position = AposTo;

    }
}
