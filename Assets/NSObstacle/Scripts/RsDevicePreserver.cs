using UnityEngine;

/***
 * Makes a RsDevice prefab, which this script is attached to, live through all scenes
 */
public class RsDevicePreserver : MonoBehaviour
{
    protected void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("RealSenseDevice");
        if (objs.Length > 1)
        {
            // The 'Awake' method is called on a new instance of the prefab, not on the original one, so we should kill this.gameObject
            Destroy(gameObject);
            return;
        }

        GetComponent<RsDevice>().enabled = true;
        DontDestroyOnLoad(gameObject);
    }
}
