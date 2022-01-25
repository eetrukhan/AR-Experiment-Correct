using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPlacer : MonoBehaviour
{
    public Transform Camera;

    public float DistanceFromCamera = 1.5f;

    private bool _menuHasBeenPlaced = false; // There is no way to place the menu using a separate thread. This helps us to overcome that

    void Start()
    {
        // Disable the script if there is no camera
        if (Camera == null)
        {
            Debug.LogError("Error: The Camera field hasn't been set. Disabling the script.");
            enabled = false;
            return;
        }

        // That's needed to update the position of the main menu every time the user returnes to it
        SceneManager.activeSceneChanged += HandleActiveSceneChanged;
    }

    void Update()
    {
        if ((!_menuHasBeenPlaced && Camera.position != Vector3.zero) || Input.GetButtonDown("Fire3"))
            PlaceMenu();
    }

    void OnDestroy()
    {
        SceneManager.activeSceneChanged -= HandleActiveSceneChanged;
    }

    private void HandleActiveSceneChanged(Scene currentScene, Scene nextScene)
    {
        if (nextScene.name == "MainMenu")
            PlaceMenu();
    }

    private void PlaceMenu()
    {
        // Place the menu right in from of the user
        Vector3 cameraRotationEuler = Camera.rotation.eulerAngles;
        cameraRotationEuler.x = 0f;
        Vector3 menuPosition = Camera.position + ((Quaternion.Euler(cameraRotationEuler) * Vector3.forward) * DistanceFromCamera);

        transform.position = menuPosition;
        _menuHasBeenPlaced = true;
    }
}
