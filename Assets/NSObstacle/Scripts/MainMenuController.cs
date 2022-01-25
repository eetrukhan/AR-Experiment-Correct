using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    public GameObject MainMenuPanel;

    public Button[] ButtonsDependentOnTrackPosition;
    public Button RestoreLastSessionButton;

    public GameObject InfoMessage;

    public InputField IMSubjectCodeInputField;
    public InputField ESSubjectCodeInputField;
    public InputField VMSubjectCodeInputField;
    public InputField EStandSubjectCodeInputField;
    public ToggleGroup IntensityToggleGroup;
    public ToggleGroup OrderToggleGroup;

    void Start()
    {
        // Check whether everything in its place 
        if (MainMenuPanel == null)
        {
            Debug.LogError("Error: The MainMenuPanel field can't be left unassigned. Disabling the script");
            enabled = false;
            return;
        }

        if (ButtonsDependentOnTrackPosition.Length == 0)
        {
            Debug.LogError("Error: The 'Intencities' array can't be empty. Disabling the script");
            enabled = false;
            return;
        }

        if (RestoreLastSessionButton == null)
        {
            Debug.LogError("Error: The RestoreLastSessionButton field can't be left unassigned. Disabling the script");
            enabled = false;
            return;
        }

        if (InfoMessage == null)
        {
            Debug.LogError("Error: The InfoMessage field can't be left unassigned. Disabling the script");
            enabled = false;
            return;
        }

        if (IMSubjectCodeInputField == null)
        {
            Debug.LogError("Error: The IMSubjectCodeInputField field can't be left unassigned. Disabling the script");
            enabled = false;
            return;
        }

        if (ESSubjectCodeInputField == null)
        {
            Debug.LogError("Error: The ESSubjectCodeInputField field can't be left unassigned. Disabling the script");
            enabled = false;
            return;
        }

        if (VMSubjectCodeInputField == null)
        {
            Debug.LogError("Error: The VMSubjectCodeInputField field can't be left unassigned. Disabling the script");
            enabled = false;
            return;
        }

        if (IntensityToggleGroup == null)
        {
            Debug.LogError("Error: The IntensityToggleGroup field can't be left unassigned. Disabling the script");
            enabled = false;
            return;
        }

        if (OrderToggleGroup == null)
        {
            Debug.LogError("Error: The OrderToggleGroup field can't be left unassigned. Disabling the script");
            enabled = false;
            return;
        }

        if (TrackTransformPreserver.IsThereAnyData()) // If the track has been placed
        {
            foreach (Button b in ButtonsDependentOnTrackPosition)
                b.interactable = true;
            if (PlayerPrefs.HasKey("ConditionNo"))
                RestoreLastSessionButton.interactable = true;

            InfoMessage.SetActive(false);
        }
    }

    public void StartCountingExercise()
    {
        SceneManager.LoadSceneAsync("CountingExercise");
    }

    public void StartWalkingExercise()
    {
        SceneManager.LoadSceneAsync("WalkingExercise");
    }

    public void StartNewSession()
    {
        try
        {
            int subjectNo = Int32.Parse(ESSubjectCodeInputField.text);
            IEnumerator<Toggle> enumerator = IntensityToggleGroup.ActiveToggles().GetEnumerator();
            enumerator.MoveNext();
            float intensityOfObstacleAppearance = float.Parse(enumerator.Current.name, CultureInfo.InvariantCulture.NumberFormat);
            enumerator = OrderToggleGroup.ActiveToggles().GetEnumerator();
            enumerator.MoveNext();
            string orderOfDesignPresentation = enumerator.Current.name;

            PlayerPrefs.SetInt("SubjectNo", subjectNo);
            PlayerPrefs.SetFloat("Intensity", intensityOfObstacleAppearance == -1f ? float.PositiveInfinity : intensityOfObstacleAppearance);
            PlayerPrefs.SetString("ConditionsOrder", orderOfDesignPresentation);

            PlayerPrefs.SetInt("ConditionNo", 0);
            PlayerPrefs.SetInt("TrialNo", 1);

            PlayerPrefs.SetFloat("SphereVelocity", 0.4f);
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }

        SceneManager.LoadSceneAsync("ExperimentSession");
    }

    public void RestoreLastSession()
    {
        SceneManager.LoadSceneAsync("ExperimentSession");
    }

    public void PlaceTrack()
    {
        SceneManager.LoadSceneAsync("TrackPlacement");
    }

    public void MeasureIntensity()
    {
        int subjectNo = Int32.Parse(IMSubjectCodeInputField.text);
        PlayerPrefs.SetInt("SubjectNo", subjectNo);

        PlayerPrefs.DeleteKey("Intensity");
        PlayerPrefs.DeleteKey("ConditionsOrder");
        PlayerPrefs.DeleteKey("ConditionNo");
        PlayerPrefs.DeleteKey("TrialNo");
        PlayerPrefs.DeleteKey("SphereVelocity");

        SceneManager.LoadSceneAsync("IntensityMeasurement");
    }

    public void MeasureVelocity()
    {
        int subjectNo = Int32.Parse(VMSubjectCodeInputField.text);
        PlayerPrefs.SetInt("SubjectNo", subjectNo);

        PlayerPrefs.SetFloat("Intensity", 1.75f);
        PlayerPrefs.SetString("ConditionsOrder", "140"); // 0 = NoInfo, 1 = HUD, 4 = 35

        PlayerPrefs.SetInt("ConditionNo", 0);
        PlayerPrefs.SetInt("TrialNo", 1);

        PlayerPrefs.DeleteKey("SphereVelocity");

        SceneManager.LoadSceneAsync("ExperimentSession");
    }

    public void StartStandingExperiment()
    {
        int subjectNo = Int32.Parse(EStandSubjectCodeInputField.text);
        PlayerPrefs.SetInt("SubjectNo", subjectNo);

        SceneManager.LoadSceneAsync("ExperimentStanding");
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
