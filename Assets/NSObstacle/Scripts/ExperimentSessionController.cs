using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(TrialDataStorage))]
public class ExperimentSessionController : MonoBehaviour, ISceneController
{
    public enum TaskType
    {
        Spheres,
        Video
    }

    public uint MaxNumberOfTrials;
    [SerializeField]
    private float _intensity;

    public GameObject Track;
    public GameObject UsersHead;
    public TaskType HMDAdministeredTaskType = TaskType.Spheres;
    public HMDAdministratedTaskController HMDAdministratedTask;
    public VideoTaskController VideoTask;
    public float[] SphereVelocities;
    public GameObject TimerDisplay;
    public GameObject Metronome;
    public GameObject DigitalKeypad;
    public GameObject Reticle;

    public GameObject ErrorMessage;
    public Text ErrorMessageCaption;
    public Text ErrorMessageText;

    private GameObject _handleAtTheBeginning;
    private GameObject _handleAtTheEnd;
    private GameObject _borderAtTheBeginning;
    private GameObject _borderAtTheEnd;

    private System.Random _random = new System.Random();

    private bool _velocityMeasurement = true;
    private uint _subjectNo;
    
    private string _conditionsOrder;
    private int _conditionNo;
    private uint _trialNo;
    private State _state;
    private TrialDataStorage _allTrialsData;

    void Start()
    {
        if (MaxNumberOfTrials == 0)
        {
            Debug.LogError("Error: MaxNumberOfTrials can't be equal to 0. Disabling the script");
            enabled = false;
            return;
        }

        if (Track == null)
        {
            Debug.LogError("Error: The Track field can't be left unassigned. Disabling the script");
            enabled = false;
            return;
        }

        if (TimerDisplay == null)
        {
            Debug.LogError("Error: The TimerDisplay field can't be left unassigned. Disabling the script");
            enabled = false;
            return;
        }

        if (UsersHead == null)
        {
            Debug.LogError("Error: The UsersHead field can't be left unassigned. Disabling the script");
            enabled = false;
            return;
        }

        if (DigitalKeypad == null)
        {
            Debug.LogError("Error: The DigitalKeypad field can't be left unassigned. Disabling the script");
            enabled = false;
            return;
        }

        if (Reticle == null)
        {
            Debug.LogError("Error: The Reticle field can't be left unassigned. Disabling the script");
            enabled = false;
            return;
        }

        if (HMDAdministratedTask == null)
        {
            Debug.LogError("Error: The HMDAdministratedTask field can't be left unassigned. Disabling the script");
            enabled = false;
            return;
        }

        if (SphereVelocities.Length == 0)
        {
            Debug.LogError("Error: The 'SphereVelocities' array can't be empty. Disabling the script");
            enabled = false;
            return;
        }

        if (VideoTask == null)
        {
            Debug.LogError("Error: The VideoTask field can't be left unassigned. Disabling the script");
            enabled = false;
            return;
        }

        if (ErrorMessage == null)
        {
            Debug.LogError("Error: The ErrorMessage field can't be left unassigned. Disabling the script");
            enabled = false;
            return;
        }

        if (ErrorMessageText == null)
        {
            Debug.LogError("Error: The ErrorMessageText field can't be left unassigned. Disabling the script");
            enabled = false;
            return;
        }

        if (ErrorMessageCaption == null)
        {
            Debug.LogError("Error: The ErrorMessageCaption field can't be left unassigned. Disabling the script");
            enabled = false;
            return;
        }

        if (!PlayerPrefs.HasKey("Intensity") || !PlayerPrefs.HasKey("ConditionsOrder"))
        {
            Debug.LogError("Error: One of the scene parameters (Intensity or ConditionsOrder) is missing. Disabling the script");
            enabled = false;
            return;
        }

        LoadParams();

        //VideoTask.BalancedLatinSquareSort((int)_subjectNo);
        VideoTask.Shuffle();

        _allTrialsData = GetComponent<TrialDataStorage>();
        if (_allTrialsData.IsThereUnsavedData())
            _allTrialsData.Save();

        Transform handle = Track.transform.Find("HandleAtTheBeginning");
        if (handle != null)
        {
            _handleAtTheBeginning = handle.gameObject;
            _handleAtTheBeginning.GetComponent<OnStartingPositionHandler>().OnCollisionDetected += OnStartingPosition;
            _handleAtTheBeginning.GetComponent<OnStartingPositionHandler>().OnLeftStartingPosition += LeftStartingPosition;
        }
        handle = Track.transform.Find("HandleAtTheEnd");
        if (handle != null)
        {
            _handleAtTheEnd = handle.gameObject;
            _handleAtTheEnd.GetComponent<OnStartingPositionHandler>().OnCollisionDetected += OnStartingPosition;
            _handleAtTheEnd.GetComponent<OnStartingPositionHandler>().OnLeftStartingPosition += LeftStartingPosition;
        }
        if (!_handleAtTheBeginning || !_handleAtTheEnd)
        {
            Debug.LogError("Error: The script couldn't find handle objects inside the Track. Disabling the script");
            enabled = false;
            return;
        }

        Transform border = Track.transform.Find("TrackItself/SouthBorder");
        if (border != null)
            _borderAtTheBeginning = border.gameObject;
        border = Track.transform.Find("TrackItself/NorthBorder");
        if (border != null)
            _borderAtTheEnd = border.gameObject;
        if (!_borderAtTheBeginning || !_borderAtTheEnd)
        {
            Debug.LogError("Error: The script couldn't find border objects inside the Track. Disabling the script");
            enabled = false;
            return;
        }

        UsersHead.GetComponent<TrackOverrunHandler>().OnEnteredTrack += EnteredTrack;
        UsersHead.GetComponent<TrackOverrunHandler>().OnLeftTrack += LeftTrack;
        UsersHead.GetComponent<TrackOverrunHandler>().OnReturnedOnTrack += BackOnTrack;
        UsersHead.GetComponent<TrackOverrunHandler>().OnCrossedFinishLine += CrossedFinishLine;

        UsersHead.GetComponent<CollisionHandler>().OnRanIntoGroundObstacle += RanIntoGroundObstacle;
        UsersHead.GetComponent<CollisionHandler>().OnRanIntoHighObstacle += RanIntoHighObstable;

        UsersHead.GetComponent<SlowpokeDetector>().OnSlowpokeDetected += StoppedAlongTheWay;
        UsersHead.GetComponent<MoonwalkDetector>().OnMoonwalkDetected += WentBackwards;

        DigitalKeypad.GetComponent<DigitalKeypad>().ValueSubmitted += EnteredCollisionNumber;

        SetState(new IdleStateExp(this));
    }

    void Update()
    {
        if (Input.GetButton("Fire3")) // D
            ReturnToMainMenu();
    }

    public void SetState(State newState)
    {
        _state = newState;
    }

    public uint GetSubjectNo()
    {
        return _subjectNo;
    }

    public float GetIntensity()
    {
        return _intensity;
    }

    public float GetSphereVelocity()
    {
        return SphereVelocities.Length == 1 ? SphereVelocities[0] : GetRandomVelocity();
    }

    public DesignOptions GetDesignOption()
    {
        int designOption = _conditionsOrder[_conditionNo] - '0';
        return (DesignOptions) designOption;
    }

    public string GetProcedure()
    {
        return _velocityMeasurement ? "Velocity Measurement" : gameObject.name;
    }

    public uint GetTrialNo()
    {
        return _trialNo;
    }

    public uint ReturnAndIncrementTrialNo()
    {
        uint currentTrialNo = _trialNo;
        if (++_trialNo > MaxNumberOfTrials)
        {
            _conditionNo++;
            _trialNo = 1;
        }

        return currentTrialNo;
    }

    public void SaveParams()
    {
        if (_conditionNo == _conditionsOrder.Length) // We've reached the end of the session
        {
            PlayerPrefs.DeleteKey("Intensity");
            PlayerPrefs.DeleteKey("ConditionsOrder");
            PlayerPrefs.DeleteKey("ConditionNo");
            PlayerPrefs.DeleteKey("TrialNo");
            PlayerPrefs.DeleteKey("SphereVelocity");
        }
        else
        {
            PlayerPrefs.SetInt("ConditionNo", _conditionNo);
            PlayerPrefs.SetInt("TrialNo", (int) _trialNo);
        }
    }

    public void SetTimerTo(float sec)
    {
        Invoke("TimeIsUp", sec);
    }

    public void StopTimer()
    {
        CancelInvoke();
    }

    public GameObject GetTrack()
    {
        return Track;
    }

    public GameObject GetTrackBorder(StartFrom startFrom)
    {
        return startFrom == StartFrom.TheBeginning ? _borderAtTheBeginning : _borderAtTheEnd;
    }

    public GameObject GetTrackStartingPosition(StartFrom startFrom)
    {
        return startFrom == StartFrom.TheBeginning ? _handleAtTheBeginning : _handleAtTheEnd;
    }

    public GameObject GetUsersHead()
    {
        return UsersHead;
    }

    public GameObject GetTimerDisplay()
    {
        return TimerDisplay;
    }

    public GameObject GetMetronome()
    {
        return Metronome;
    }

    public GameObject GetDigitalKeypad()
    {
        return DigitalKeypad;
    }

    public GameObject GetReticle()
    {
        return Reticle;
    }

    public GameObject GetErrorMessage()
    {
        return ErrorMessage;
    }

    public Text GetErrorMessageCaption()
    {
        return ErrorMessageCaption;
    }

    public Text GetErrorMessageText()
    {
        return ErrorMessageText;
    }

    public TrialDataStorage GetDataStorage()
    {
        return _allTrialsData;
    }

    public TaskType GetHMDAdministeredTaskType()
    {
        return HMDAdministeredTaskType;
    }

    public HMDAdministratedTaskController GetHMDAdministratedTask()
    {
        return HMDAdministratedTask;
    }

    public VideoTaskController GetVideoTask()
    {
        return VideoTask;
    }

    public void OnStartingPosition(StartFrom startFrom)
    {
        _state.OnStartingPosition(startFrom);
    }

    public void LeftStartingPosition()
    {
        _state.LeftStartingPosition();
    }

    public void EnteredTrack()
    {
        _state.EnteredTrack();
    }

    public void StoppedAlongTheWay()
    {
        _state.StoppedAlongTheWay();
    }

    public void WentBackwards()
    {
        _state.WentBackwards();
    }

    public void LeftTrack()
    {
        _state.LeftTrack();
    }

    public void BackOnTrack()
    {
        _state.BackOnTrack();
    }

    public void CrossedFinishLine()
    {
        _state.CrossedFinishLine();
    }

    public void EnteredCollisionNumber(uint collisionsNumberReported)
    {
        _state.EnteredCollisionNumber(collisionsNumberReported);
    }

    public void TimeIsUp()
    {
        _state.TimeIsUp();
    }

    public void RanIntoGroundObstacle()
    {
        _allTrialsData.GetCurrectTrialData().NumberOfGroundObstaclesTouched++;
    }

    public void RanIntoHighObstable()
    {
        _allTrialsData.GetCurrectTrialData().NumberOfHighObstaclesTouched++;
    }

    private float GetRandomVelocity()
    {
        int velocityIndex = _random.Next(SphereVelocities.Length);
        return SphereVelocities[velocityIndex];
    }

    private void LoadParams()
    {
        _subjectNo = (uint) PlayerPrefs.GetInt("SubjectNo");
        _intensity = PlayerPrefs.GetFloat("Intensity");
        _conditionsOrder = PlayerPrefs.GetString("ConditionsOrder");

        _conditionNo = PlayerPrefs.GetInt("ConditionNo", 0);
        _trialNo = (uint) PlayerPrefs.GetInt("TrialNo", 1);

        if (PlayerPrefs.HasKey("SphereVelocity"))
        {
            _velocityMeasurement = false;

            SphereVelocities = new float[1];
            SphereVelocities[0] = PlayerPrefs.GetFloat("SphereVelocity");
        }
    }

    private void ReturnToMainMenu()
    {
        if (_velocityMeasurement)
        {
            PlayerPrefs.DeleteKey("Intensity");
            PlayerPrefs.DeleteKey("ConditionsOrder");
            PlayerPrefs.DeleteKey("ConditionNo");
            PlayerPrefs.DeleteKey("TrialNo");
        }

        Scene mainMenuScene = SceneManager.GetSceneByName("MainMenu");
        if (mainMenuScene.isLoaded)
            SceneManager.SetActiveScene(mainMenuScene);
        else
            SceneManager.LoadScene("MainMenu");

        SceneManager.UnloadSceneAsync("ExperimentSession");
    }
}