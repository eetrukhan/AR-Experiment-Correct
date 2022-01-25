using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CountingExerciseController : MonoBehaviour
{
#pragma warning disable 649
    [SerializeField]
    private ExperimentSessionController.TaskType HMDAdministeredTaskType = ExperimentSessionController.TaskType.Spheres;
    [SerializeField]
    private HMDAdministratedTaskController _HMDAdministratedTask;
    [SerializeField]
    private VideoTaskController _VideoTask;
    [SerializeField]
    private uint _exerciseLength = 15;
    [SerializeField]
    private GameObject _digitalKeypad;
    [SerializeField]
    private GameObject _reticle;
    [SerializeField]
    private TextMeshPro _resultText;
#pragma warning restore 649

    private enum Mode
    {
        Idle,
        Counting,
        Answering
    }
    private Mode _mode = Mode.Idle;
    private uint _numberOfCollisions;
    private string _defaultText;

    private void Start()
    {
        // Sanity check
        if (_HMDAdministratedTask == null)
        {
            Debug.LogError("CountingExerciseController: The HMDAdministratedTask field hasn't been set. Disabling the script.");
            enabled = false;
            return;
        }

        if (_VideoTask == null)
        {
            Debug.LogError("CountingExerciseController: The VideoTask field hasn't been set. Disabling the script.");
            enabled = false;
            return;
        }

        if (_exerciseLength == 0)
        {
            Debug.LogError("CountingExerciseController: ExerciseLength has to be greater than 0. Disabling the script.");
            enabled = false;
            return;
        }

        if (_digitalKeypad == null)
        {
            Debug.LogError("CountingExerciseController: The DigitalKeypad field hasn't been set. Disabling the script.");
            enabled = false;
            return;
        }

        if (_reticle == null)
        {
            Debug.LogError("CountingExerciseController: The Reticle field hasn't been set. Disabling the script.");
            enabled = false;
            return;
        }

        if (_resultText == null)
        {
            Debug.LogError("CountingExerciseController: The ResultText field hasn't been set. Disabling the script.");
            enabled = false;
            return;
        }
        _defaultText = _resultText.text;

        // Initial preparation
        _VideoTask.Shuffle();
        _digitalKeypad.GetComponent<DigitalKeypad>().ValueSubmitted += EnteredCollisionNumber;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Jump")) // Trigger on the VR BOX joystick, Space on a keyboard
            StartExercise();

        if (Input.GetButtonDown("Fire3")) // D on the VR BOX joystick, Left Shift on a keyboard
            ReturnToMainMenu();
    }

    private void StartExercise() // Idle -> Counting
    {
        // Setting the scene, metaphorically speaking :-)
        _digitalKeypad.SetActive(false);
        _reticle.SetActive(false);
        _resultText.gameObject.SetActive(false);
        CurrentHMDTask().SetActive(true);
        _HMDAdministratedTask.GenerateFiled(); // If the current task is of the video type, this method will just do nothing
        CancelInvoke();

        // Starting a counting exercise
        if (HMDAdministeredTaskType == ExperimentSessionController.TaskType.Spheres)
            _HMDAdministratedTask.Play();
        else
            _VideoTask.Play();
        _mode = Mode.Counting;

        Invoke("TimeIsUp", _exerciseLength);
    }

    private void TimeIsUp() // Counting -> Answering
    {
        if (HMDAdministeredTaskType == ExperimentSessionController.TaskType.Spheres)
            _numberOfCollisions = _HMDAdministratedTask.GetNumberOfColissions();
        CurrentHMDTask().SetActive(false);

        _digitalKeypad.SetActive(true);
        _reticle.SetActive(true);

        _mode = Mode.Answering;
    }

    public void EnteredCollisionNumber(uint collisionsNumberReported) // Answering -> Idle
    {
        // Displaying the answer
        if (HMDAdministeredTaskType == ExperimentSessionController.TaskType.Spheres)
            _resultText.text = collisionsNumberReported == _numberOfCollisions ?
                "Верно!" : $"Почти! Столкновений было {_numberOfCollisions}";
        else
            _resultText.text = _defaultText;
        _resultText.gameObject.SetActive(true);

        // Taking care of everythinh else
        _digitalKeypad.SetActive(false);
        _reticle.SetActive(false);

        _mode = Mode.Idle;
    }

    private GameObject CurrentHMDTask()
    {
        return HMDAdministeredTaskType == ExperimentSessionController.TaskType.Spheres ? _HMDAdministratedTask.gameObject : _VideoTask.gameObject;
    }

    private void ReturnToMainMenu()
    {
        Scene mainMenuScene = SceneManager.GetSceneByName("MainMenu");
        if (mainMenuScene.isLoaded)
            SceneManager.SetActiveScene(mainMenuScene);
        else
            SceneManager.LoadScene("MainMenu");

        SceneManager.UnloadSceneAsync("CountingExercise");
    }
}
