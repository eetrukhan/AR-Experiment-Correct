using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class TouchPointerController : MonoBehaviour
{
    public EventSystem eventSystem;

    void Start()
    {
        // Disable the script if there is no camera
        if (eventSystem == null)
        {
            Debug.LogError("Error: The eventSystem field cannpt be left unassigned. Disabling the script.");
            enabled = false;
            return;
        }
    }

    // See the Collision action matrix (https://docs.unity3d.com/Manual/CollidersOverview.html)
    void OnTriggerEnter(Collider other)
    {
        if (!enabled) return;

        PointerEventData pointerEventData = new PointerEventData(eventSystem);
        pointerEventData.pointerEnter = other.gameObject;
        ExecuteEvents.Execute(other.gameObject, pointerEventData, ExecuteEvents.pointerEnterHandler);
    }

    void OnTriggerExit(Collider other)
    {
        if (!enabled) return;

        ExecuteEvents.Execute(other.gameObject, new PointerEventData(eventSystem), ExecuteEvents.pointerExitHandler);
    }
}
