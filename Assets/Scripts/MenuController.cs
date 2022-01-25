using System;
using Logic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public GameObject MainMenuPanel;
    public GameObject InfoMessage;
    public Text headerText;
    public InputField subjectNumber;
    public InputField timeInSeconds;
    public InputField notificationsNumber;
    public InputField numberOfHaveToActNotifications;
    public TextMeshProUGUI notificationSource;
    public TextMeshProUGUI notificationAuthor;
    public InputField trialsNumber;

    public void StartNewSession()
    {
        try
        {
            Int32.TryParse(subjectNumber.text, out ExperimentData.subjectNumber);
            Int32.TryParse(timeInSeconds.text, out ExperimentData.timeInSeconds);
            Int32.TryParse(notificationsNumber.text, out ExperimentData.notificationsNumber);
            Int32.TryParse(numberOfHaveToActNotifications.text, out ExperimentData.numberOfHaveToActNotifications);
            Int32.TryParse(trialsNumber.text, out ExperimentData.trialsNumber);
            ExperimentData.notificationSource = notificationSource.text;
            ExperimentData.notificationAuthor = notificationAuthor.text;
            string text = headerText.text.Split(':')[1].Trim().Replace("\"", "");
            switch (text)
            {
                case "Перед пользователем - мобильные":
                    {
                        StartInFrontOfMobile();
                        return;
                    }
                case "Перед пользователем - стикеры":
                    {
                        StartInFrontOfStickers();
                        return;
                    }       
                case "Вокруг пользователя - мобильные":
                    {
                        StartAroundMobile();
                        return;
                    }
                case "Вокруг пользователя - стикеры":
                    {
                        StartAroundStickers();
                        return;
                    }
                case "Невидимые - волны":
                    {
                        StartHiddenWaves();
                        return;
                    }
                default: return;
            }
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }

    public void StartInFrontOfMobile()
    {
        GlobalCommon.currentTypeName = "InFrontOfMobile";
        SceneManager.LoadSceneAsync("InFrontOfMobile");
    }

    public void StartInFrontOfStickers()
    {
        GlobalCommon.currentTypeName = "InFrontOfStickers";
        SceneManager.LoadSceneAsync("InFrontOfStickers");
    }

    public void StartAroundMobile()
    {
        GlobalCommon.currentTypeName = "AroundMobile";
        SceneManager.LoadSceneAsync("AroundMobile");
    }

    public void StartAroundStickers()
    {
        GlobalCommon.currentTypeName = "AroundStickers";
        SceneManager.LoadSceneAsync("AroundStickers");
    }

    public void StartHiddenWaves()
    {
        GlobalCommon.currentTypeName = "HiddenWaves";
        SceneManager.LoadSceneAsync("HiddenWaves");
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
