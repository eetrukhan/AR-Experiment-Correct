using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WalkingExerciseController : MonoBehaviour, ISceneController
{
    public GameObject Track;
    public GameObject UsersHead;

    public GameObject TimerDisplay;
    public GameObject Metronome;

    public GameObject ErrorMessage;
    public Text ErrorMessageText;

    private GameObject _handleAtTheBeginning;
    private GameObject _handleAtTheEnd;
    private GameObject _borderAtTheBeginning;
    private GameObject _borderAtTheEnd;

    private State _state;
    private static readonly float DUMMY_INTENSITY_VALUE = 1f;

    void Start()
    {
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

        UsersHead.GetComponent<SlowpokeDetector>().OnSlowpokeDetected += StoppedAlongTheWay;
        UsersHead.GetComponent<MoonwalkDetector>().OnMoonwalkDetected += WentBackwards;

        SetState(new IdleStateBase(this));
    }

    void Update()
    {
        if (Input.GetButton("Fire3"))
            ReturnToMainMenu();
    }

    public void SetState(State newState)
    {
        _state = newState;
    }

    public uint GetSubjectNo()
    {
        throw new NotImplementedException();
    }

    public float GetIntensity()
    {
        return DUMMY_INTENSITY_VALUE;
    }

    public float GetSphereVelocity()
    {
        throw new NotImplementedException();
    }

    public DesignOptions GetDesignOption()
    {
        throw new NotImplementedException();
    }

    public string GetProcedure()
    {
        throw new NotImplementedException();
    }

    public uint GetTrialNo()
    {
        throw new NotImplementedException();
    }

    public uint ReturnAndIncrementTrialNo()
    {
        throw new NotImplementedException();
    }

    public void SaveParams()
    {
        throw new NotImplementedException();
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
        throw new NotImplementedException();
    }

    public GameObject GetReticle()
    {
        throw new NotImplementedException();
    }

    public GameObject GetErrorMessage()
    {
        return ErrorMessage;
    }

    public Text GetErrorMessageCaption()
    {
        throw new NotImplementedException();
    }

    public Text GetErrorMessageText()
    {
        return ErrorMessageText;
    }

    public TrialDataStorage GetDataStorage()
    {
        throw new NotImplementedException();
    }

    public ExperimentSessionController.TaskType GetHMDAdministeredTaskType()
    {
        throw new NotImplementedException();
    }

    public HMDAdministratedTaskController GetHMDAdministratedTask()
    {
        throw new NotImplementedException();
    }

    public VideoTaskController GetVideoTask()
    {
        throw new NotImplementedException();
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

    public void TimeIsUp()
    {
        _state.TimeIsUp();
    }

    private void ReturnToMainMenu()
    {
        Scene mainMenuScene = SceneManager.GetSceneByName("MainMenu");
        if (mainMenuScene.isLoaded)
            SceneManager.SetActiveScene(mainMenuScene);
        else
            SceneManager.LoadScene("MainMenu");

        SceneManager.UnloadSceneAsync("WalkingExercise");
    }
}