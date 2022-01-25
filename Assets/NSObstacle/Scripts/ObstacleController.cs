using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    public GameObject[] Children;

    public Material TouchedMaterial;

    void Start()
    {
        if (TouchedMaterial == null)
        {
            Debug.LogError("Error: The TouchedMaterial can't be left unassigned. Disabling the script");
            enabled = false;
            return;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!enabled)
            return;

        foreach (GameObject go in Children)
            go.GetComponent<Renderer>().material = TouchedMaterial;
    }

    void OnTriggerExit(Collider other)
    {
        if (!enabled)
            return;
    }
}
