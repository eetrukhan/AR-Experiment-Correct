using UnityEngine;

public class PlaceMeWhereItWas : MonoBehaviour
{
    [SerializeField]
    private Transform it;

    protected void OnEnable()
    {
        transform.position = it.position;
        transform.rotation = it.rotation;
    }
}
