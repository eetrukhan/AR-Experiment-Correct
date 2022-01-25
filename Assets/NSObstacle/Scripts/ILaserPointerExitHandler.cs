using UnityEngine;
using UnityEngine.EventSystems;

public interface ILaserPointerExitHandler : IEventSystemHandler
{
    void OnLaserPointerExit(Vector3 laserPointerOrigin, Vector3 laserPointerDirection);
}
