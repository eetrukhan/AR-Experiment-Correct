using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalAroundPlacer : MonoBehaviour
{
    [SerializeField]
    private RsDevice source;
    [SerializeField]
    private Transform originCube;
    // Start is called before the first frame update
    void Start()
    {
       // source = (RsDevice)FindObjectOfType(typeof(RsDevice));
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.L))
        {
            if (source.GetARAnchor("origin", out Vector3 pos, out Quaternion rot))
            {
                originCube.position = pos;
                originCube.rotation = rot;

                Debug.Log("The cube was successfully placed at the origin");
            }
        }
    }
}
