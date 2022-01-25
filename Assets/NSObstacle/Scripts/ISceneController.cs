using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface ISceneController
{
    void SetState(State newState);
    void SetTimerTo(float sec);
    void StopTimer();
    void SaveParams();

    uint GetSubjectNo();
    float GetIntensity();
    float GetSphereVelocity();
    DesignOptions GetDesignOption();
    string GetProcedure();
    uint GetTrialNo();
    uint ReturnAndIncrementTrialNo();

    GameObject GetTrack();
    GameObject GetTrackBorder(StartFrom startFrom);
    GameObject GetTrackStartingPosition(StartFrom startFrom);
    GameObject GetUsersHead();
    GameObject GetTimerDisplay();
    GameObject GetMetronome();
    GameObject GetDigitalKeypad();
    GameObject GetReticle();
    GameObject GetErrorMessage();
    Text GetErrorMessageCaption();
    Text GetErrorMessageText();
    TrialDataStorage GetDataStorage();
    ExperimentSessionController.TaskType GetHMDAdministeredTaskType();
    HMDAdministratedTaskController GetHMDAdministratedTask();
    VideoTaskController GetVideoTask();
}
