using UnityEngine;

public class LightsVane : MonoBehaviour
{
#pragma warning disable 649
    [SerializeField]
    private Transform _copyRotationFrom;
#pragma warning restore 649
    // Start is called before the first frame update
    void Start()
    {
        if (_copyRotationFrom == null)
        {
            Debug.LogError("LightsVane: The CopyRotationFrom field can't be left unassigned. Disabling the script");
            enabled = false;
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 rotation = new Vector3(0f, _copyRotationFrom.rotation.eulerAngles.y, 0f);
        transform.rotation = Quaternion.Euler(rotation);
    }
}
