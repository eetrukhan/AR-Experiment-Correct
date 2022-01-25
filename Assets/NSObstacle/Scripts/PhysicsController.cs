using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PhysicsController : MonoBehaviour
{
    public Transform Parent;
    public bool OverrideUnityPhysics = true;

    private Rigidbody _rigidbody;
    private Quaternion _parentRotation;
    private Vector3 lastFrameVelocity;

    private const int FIRST_POINT = 0;

    private void OnEnable()
    {
        _rigidbody = GetComponent<Rigidbody>();

        if (Parent == null)
            Parent = transform.parent;
        _parentRotation = Parent.rotation;
    }

    private void FixedUpdate()
    {
        if (!enabled) return;

        if (Parent.rotation != _parentRotation)
        {
            Quaternion diff = Parent.rotation * Quaternion.Inverse(_parentRotation);
            _rigidbody.velocity = diff * _rigidbody.velocity;

            _parentRotation = Parent.rotation;
        }
    }

    private void Update()
    {
        lastFrameVelocity = _rigidbody.velocity;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!enabled) return;

        if (OverrideUnityPhysics)
        {
            Vector3 collisionNormal = collision.GetContact(FIRST_POINT).normal;
            _rigidbody.velocity = Vector3.Reflect(lastFrameVelocity, collisionNormal);
        }
    }

    public void SetVelocity(Vector3 velocity)
    {
        lastFrameVelocity = _rigidbody.velocity = velocity;
    }
}
