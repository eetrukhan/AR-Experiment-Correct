using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

[RequireComponent(typeof(TrialDataStorage))]
public class ExperimentStandingController : MonoBehaviour
{
#pragma warning disable 649
    [SerializeField]
    private ExperimentSessionController.TaskType HMDAdministeredTaskType = ExperimentSessionController.TaskType.Spheres;
    [SerializeField]
    private HMDAdministratedTaskController _HMDAdministratedTask;
    [SerializeField]
    private VideoTaskController _VideoTask;
    [SerializeField]
    private float _exerciseLength = 15;
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
    private uint _subjectNo;
    private uint _trialNo;
    private string _result;
    private TrialDataStorage _allTrialsData;

    private void Start()
    {
        // Sanity check
        if (_HMDAdministratedTask == null)
        {
            Debug.LogError("ExperimentStandingController: The HMDAdministratedTask field hasn't been set. Disabling the script.");
            enabled = false;
            return;
        }

        if (_VideoTask == null)
        {
            Debug.LogError("ExperimentStandingController: The VideoTask field hasn't been set. Disabling the script.");
            enabled = false;
            return;
        }

        if (_exerciseLength <= 0)
        {
            Debug.LogError("ExperimentStandingController: ExerciseLength has to be greater than 0. Disabling the script.");
            enabled = false;
            return;
        }

        if (_digitalKeypad == null)
        {
            Debug.LogError("ExperimentStandingController: The DigitalKeypad field hasn't been set. Disabling the script.");
            enabled = false;
            return;
        }

        if (_reticle == null)
        {
            Debug.LogError("ExperimentStandingController: The Reticle field hasn't been set. Disabling the script.");
            enabled = false;
            return;
        }

        if (_resultText == null)
        {
            Debug.LogError("ExperimentStandingController: The ResultText field hasn't been set. Disabling the script.");
            enabled = false;
            return;
        }

        LoadParams();

        _allTrialsData = GetComponent<TrialDataStorage>();
        if (_allTrialsData.IsThereUnsavedData())
            _allTrialsData.Save();

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
        _result = HMDAdministeredTaskType == ExperimentSessionController.TaskType.Spheres ?
            _HMDAdministratedTask.GetNumberOfColissions().ToString() : _VideoTask.LastVideoName;
        CurrentHMDTask().SetActive(false);

        _digitalKeypad.SetActive(true);
        _reticle.SetActive(true);

        _mode = Mode.Answering;
    }

    public void EnteredCollisionNumber(uint collisionsNumberReported) // Answering -> Idle
    {
        SaveResults(collisionsNumberReported);

        // Taking care of everything else
        _digitalKeypad.SetActive(false);
        _reticle.SetActive(false);
        _resultText.gameObject.SetActive(true);

        _mode = Mode.Idle;
    }

    private void LoadParams()
    {
        _subjectNo = (uint)PlayerPrefs.GetInt("SubjectNo");
    }

    private void SaveResults(uint collisionsNumberReported)
    {
        _allTrialsData.NextTrial(_subjectNo, float.PositiveInfinity, DesignOptions.HUD, gameObject.name, ++_trialNo);
        _allTrialsData.GetCurrectTrialData().Success = true;
        _allTrialsData.GetCurrectTrialData().Time = _exerciseLength;
        _allTrialsData.GetCurrectTrialData().CollisionsNumberReported = collisionsNumberReported;
        _allTrialsData.GetCurrectTrialData().RealCollisionsNumber = _result;
        _allTrialsData.GetCurrectTrialData().SphereVelocity = HMDAdministeredTaskType == ExperimentSessionController.TaskType.Spheres ?
            _HMDAdministratedTask.SphereVelocity : 0f;

        _allTrialsData.Save();
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

        SceneManager.UnloadSceneAsync("ExperimentStanding");
    }
}
