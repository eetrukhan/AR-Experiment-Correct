using System;
using UnityEngine;
using UnityEngine.UI;

public class ConditionsPanelController : MonoBehaviour
{
    public InputField SubjectCodeInputField;
    public bool IncrementSubjectCodeOnEnable = true;
    public Button MinusButton;

    void OnEnable()
    {
        // Check whether everything in its place 
        if (SubjectCodeInputField == null)
        {
            Debug.LogError("Error: The SubjectCodeInputField field cannot be left unassigned. Disabling the script");
            enabled = false;
            return;
        }

        if (MinusButton == null)
        {
            Debug.LogError("Error: The MinusButton field cannot be left unassigned. Disabling the script");
            enabled = false;
            return;
        }

        int currectSubjectNo = PlayerPrefs.GetInt("SubjectNo", 0);
        SubjectCodeInputField.text = (IncrementSubjectCodeOnEnable ? ++currectSubjectNo : currectSubjectNo).ToString();

        MinusButton.interactable = (currectSubjectNo > 1);
    }

    public void IncreaseSubjectNo()
    {
        if (!enabled) return;

        int subjectNo = Int32.Parse(SubjectCodeInputField.text);
        SubjectCodeInputField.text = (++subjectNo).ToString();

        MinusButton.interactable = true;
    }

    public void DecreaseSubjectNo()
    {
        if (!enabled) return;

        int subjectNo = Int32.Parse(SubjectCodeInputField.text);
        SubjectCodeInputField.text = (--subjectNo).ToString();

        if (subjectNo == 1)
            MinusButton.interactable = false;
    }
}
