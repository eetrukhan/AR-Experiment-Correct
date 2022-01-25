using UnityEngine;
using UnityEngine.SceneManagement;

public class TrackPlacementController : MonoBehaviour
{
#pragma warning disable 649
    [SerializeField]
    private TrackPoseController _trackPoseController;
    [SerializeField]
    private GameObject _step1Panel;
    [SerializeField]
    private GameObject _step2Panel;
#pragma warning restore 649

    private RsDevice rsDevice;

    void Start()
    {
        if (_trackPoseController == null)
        {
            Debug.LogError("TrackPlacementController: The 'Track' field cannot be left unassigned. Disabling the script");
            enabled = false;
            return;
        }

        if (_step1Panel == null)
        {
            Debug.LogError("TrackPlacementController: The 'Step 1 Panel' field cannot be left unassigned. Disabling the script");
            enabled = false;
            return;
        }

        if (_step2Panel == null)
        {
            Debug.LogError("TrackPlacementController: The 'Step 2 Panel' field cannot be left unassigned. Disabling the script");
            enabled = false;
            return;
        }

        GameObject rsDeviceObj = GameObject.Find("RsDevice"); // Yeah, I know it isn't a good practice (better to use FindWithTag) but it is a safeguard mechanism so let it be. (c) John Lennon ;-)
        if (rsDeviceObj != null)
            rsDevice = rsDeviceObj.GetComponent<RsDevice>();
    }

    void Update()
    {
        switch (_trackPoseController.Mode)
        {
            case TrackPoseController.PlacementMode.TrackProjection:
                if (Input.GetButton("Jump")) // Trigger on the VR BOX joystick, Space on a keyboard
                {
                    _trackPoseController.GetComponent<TrackTransformPreserver>().Preserve();
                    if (rsDevice != null)
                        rsDevice.CreateARAnchor("track", _trackPoseController.transform.position, _trackPoseController.transform.rotation);
                    _trackPoseController.GetComponent<ObstacleFactory>().ProduceAllAtOnce(StartFrom.TheBeginning);

                    _trackPoseController.Mode = TrackPoseController.PlacementMode.PoseAdjustment;
                    _step1Panel.SetActive(false);
                    _step2Panel.SetActive(true);
                }
                break;
            case TrackPoseController.PlacementMode.PoseAdjustment:
                if (Input.GetButton("Fire2")) // C on the VR BOX joystick, Left Alt on a keyboard
                {
                    TrackTransformPreserver.ClearData();
                    _trackPoseController.GetComponent<ObstacleFactory>().Destroy();

                    _trackPoseController.Mode = TrackPoseController.PlacementMode.TrackProjection;
                    _step1Panel.SetActive(true);
                    _step2Panel.SetActive(false);
                }
                break;
        }

        if (Input.GetButton("Fire3")) // D on the VR BOX joystick, Left Shift on a keyboard
        {
            if (_trackPoseController.Mode == TrackPoseController.PlacementMode.PoseAdjustment)
            {
                _trackPoseController.GetComponent<TrackTransformPreserver>().Preserve();
                if (rsDevice != null)
                    rsDevice.CreateARAnchor("track", _trackPoseController.transform.position, _trackPoseController.transform.rotation);
            }

            ReturnToMainMenu();
        }
    }

    private void ReturnToMainMenu()
    {
        Scene mainMenuScene = SceneManager.GetSceneByName("MainMenu");
        if (mainMenuScene.isLoaded)
            SceneManager.SetActiveScene(mainMenuScene);
        else
            SceneManager.LoadScene("MainMenu");

        SceneManager.UnloadSceneAsync("TrackPlacement");
    }
}
