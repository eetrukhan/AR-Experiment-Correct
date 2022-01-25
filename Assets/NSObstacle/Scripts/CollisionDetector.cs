using System;
using UnityEngine;

public class CollisionDetector : MonoBehaviour
{
    public event Action OnCollisionDetected;

    void OnCollisionExit(Collision collision)
    {
        OnCollisionDetected();
    }
}
