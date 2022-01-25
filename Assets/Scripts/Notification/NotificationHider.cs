using System.Collections;
using Logic;
using TMPro;
using UnityEngine;

public class NotificationHider : MonoBehaviour
{
    public float hideTimeOfTheNotificationAfterArrival;
    public GameObject id;

    void Start()
    {
        if (transform.parent != null && transform.parent.name != "TrayHolder")
        {
            StartCoroutine(Destroyer());
        }
    }

    IEnumerator Destroyer()
    {
        yield return new WaitForSeconds(hideTimeOfTheNotificationAfterArrival);
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
