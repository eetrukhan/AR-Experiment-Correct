using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightOnSelect : MonoBehaviour, ILaserPointerEnterHandler, ILaserPointerExitHandler
{
    public Material highlightMaterial;

    private Material _defaultMaterial;

    void Awake()
    {
        if (highlightMaterial == null)
        {
            Debug.LogError("Error: The highlight material can't be left unassigned. Disabling the script");
            enabled = false;
            return;
        }

        // Save the reference to material that was set up at the beginning
        _defaultMaterial = GetComponent<Renderer>().material;
    }

    public void OnLaserPointerEnter(Vector3 laserPointerOrigin, Vector3 laserPointerDirection)
    {
        GetComponent<Renderer>().material = highlightMaterial;
    }

    public void OnLaserPointerExit(Vector3 laserPointerOrigin, Vector3 laserPointerDirection)
    {
        GetComponent<Renderer>().material = _defaultMaterial;
    }
}
