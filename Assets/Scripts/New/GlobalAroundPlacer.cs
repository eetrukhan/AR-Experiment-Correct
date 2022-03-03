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
      /*  if (Input.GetKeyDown(KeyCode.L))
        {
            if (source.GetARAnchor("origin", out Vector3 pos, out Quaternion rot))
            {
                originCube.position = pos;
                originCube.rotation = rot;

                Debug.Log("The cube was successfully placed at the origin");
            }
        }
*/
        float speed = 0.1f;
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            transform.position += new Vector3(0,speed,0)*Time.deltaTime;
            Debug.Log("Up");
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            transform.position += new Vector3(0,-speed,0)*Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            transform.position += new Vector3(0,0,speed)*Time.deltaTime; 
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            transform.position += new Vector3(0,0,-speed)*Time.deltaTime; 
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            transform.position += new Vector3(-speed,0,0)*Time.deltaTime; 
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            transform.position += new Vector3(speed,0,0)*Time.deltaTime; 
        }
    }
}
