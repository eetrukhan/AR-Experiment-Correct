using UnityEngine;
using UnityEngine.EventSystems;

public interface ILaserPointerEnterHandler : IEventSystemHandler
{
    void OnLaserPointerEnter(Vector3 laserPointerOrigin, Vector3 laserPointerDirection);
}
