using UnityEngine;
using UnityEngine.EventSystems;

public interface I3DDragEndedHandler : IEventSystemHandler
{
    void On3DDragEnded(Vector3 laserPointerOrigin, Vector3 laserPointerDirection);
}
