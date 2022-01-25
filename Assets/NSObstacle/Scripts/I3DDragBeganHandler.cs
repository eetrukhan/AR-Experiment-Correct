using UnityEngine;
using UnityEngine.EventSystems;

public interface I3DDragBeganHandler : IEventSystemHandler
{
    void On3DDragBegan(Vector3 laserPointerOrigin, Vector3 laserPointerDirection);
}
