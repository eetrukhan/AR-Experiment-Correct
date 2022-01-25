using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HMDTaskPlacement : MonoBehaviour
{
#pragma warning disable 649
    [SerializeField]
    private Transform _camera;
    [SerializeField]
    private float _distanceFromCamera = 1.5f;
    [SerializeField]
    private float _lowerDown = 1f;
#pragma warning restore 649

    private bool _menuHasBeenPlaced = false; // There is no way to place the menu using a separate thread. This helps us to overcome that

    void Start()
    {
        // Disable the script if there is no camera
        if (_camera == null)
        {
            Debug.LogError("Error: The Camera field hasn't been set. Disabling the script.");
            enabled = false;
            return;
        }
    }

    void Update()
    {
        if (!enabled) return;

        if ((!_menuHasBeenPlaced && _camera.position != Vector3.zero) || Input.GetButtonDown("Fire2")) // C on the VR BOX joystick, Left Alt on a keyboard
            PlaceMenu();
    }

    private void PlaceMenu()
    {
        // Place the menu right in from of the user
        Vector3 cameraRotationEuler = _camera.rotation.eulerAngles;
        cameraRotationEuler.x = 0f;
        Vector3 menuPosition = _camera.position + ((Quaternion.Euler(cameraRotationEuler) * Vector3.forward) * _distanceFromCamera);
        menuPosition.y -= _lowerDown;

        transform.position = menuPosition;
        _menuHasBeenPlaced = true;
    }
}
