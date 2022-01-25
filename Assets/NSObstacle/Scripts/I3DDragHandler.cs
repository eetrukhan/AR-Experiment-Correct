using UnityEngine;
using UnityEngine.EventSystems;

public interface I3DDragHandler : IEventSystemHandler
{
    void On3DDrag(Vector3 laserPointerOrigin, Vector3 laserPointerDirection);
}