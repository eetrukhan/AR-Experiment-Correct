using System;
using System.Collections;
using Logic;
using TMPro;
using UnityEngine;

public class NotificationHider : MonoBehaviour
{
    public float hideTimeOfTheNotificationAfterArrival;
    public GameObject id;
    private bool shouldBeDestroyed = false;
    

    void Awake()
    {
     /*   Notification n = FindObjectOfType<Storage>().getFromStorage(id.GetComponent<TextMeshPro>().text, "Telegram");
        if (shouldBeDestroyed)
        {
            long timestamp = DateTime.Now.Ticks;
            long timeDestroyed = n.Timestamp + (new DateTime(0, 0, 0, 0, 0, 10).Ticks);
            Debug.Log(timestamp + " / "+ timeDestroyed);
            if (timeDestroyed <= timestamp)
                StartCoroutine(Destroyer(0));
            else
                StartCoroutine(Destroyer(timeDestroyed - timestamp));
        }
        */
    }
    void Start()
    {
        if (transform.parent != null && transform.parent.name != "TrayHolder")
        {
            Notification n = FindObjectOfType<Storage>().getFromStorage(id.GetComponent<TextMeshPro>().text, "Telegram");
            long timestamp = DateTime.Now.Ticks;
            long timeDestroyed = n.Timestamp + 10*TimeSpan.TicksPerSecond;
            Debug.Log(timestamp + " / "+ timeDestroyed);
            if (timeDestroyed <= timestamp)
                StartCoroutine(Destroyer(0));
            else
                StartCoroutine(Destroyer(timeDestroyed - timestamp));
           // if(!shouldBeDestroyed)
             //   StartCoroutine(Destroyer(hideTimeOfTheNotificationAfterArrival));
        }
    }

    IEnumerator Destroyer(long time)
    {
        shouldBeDestroyed = true;
        yield return new WaitForSeconds((float)(new TimeSpan((time)).TotalSeconds));
        Debug.Log("Destroy");
        string sourceName = transform.Find("Source").GetComponent<TextMeshPro>().text;
        string tag = "MarkAsRead";
        Notification n = FindObjectOfType<Storage>().getFromStorage(id.GetComponent<TextMeshPro>().text, sourceName);
        if (n != null)
        {
            if (n.isSilent)
            {
                sourceName = GlobalCommon.silentGroupKey;
            }
            FindObjectOfType<Storage>().removeFromStorage(id.GetComponent<TextMeshPro>().text, sourceName, tag);
            rebuildSwitcher();
        }
    }

    private void rebuildSwitcher()
    {
        switch (GlobalCommon.currentTypeName)
        {
            case "InFrontOfMobile":
                {
                    FindObjectOfType<InFrontOfMobile>().rebuildScene();
                    break;
                }
            case "InFrontOfStickers":
                {
                    FindObjectOfType<InFrontOfStickers>().rebuildScene();
                    break;
                }
            case "AroundMobile":
                {
                    FindObjectOfType<AroundMobile>().rebuildScene();
                    break;
                }
            case "AroundStickers":
                {
                    FindObjectOfType<AroundStickers>().rebuildScene();
                    break;
                }
            case "HiddenWaves":
                {
                    FindObjectOfType<HiddenWaves>().rebuildScene();
                    break;
                }
        }
    }
}
