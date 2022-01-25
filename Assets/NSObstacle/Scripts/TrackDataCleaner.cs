using UnityEngine;

public class TrackDataCleaner : MonoBehaviour
{
    protected void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("TrackDataCleaner");
        if (objs.Length > 1)
        {
            // The 'Awake' method is called on a new instance of the object, not on the original one, so we should kill this.gameObject
            Destroy(gameObject);
            return;
        }

        TrackTransformPreserver.ClearData();
        DontDestroyOnLoad(gameObject);
    }
}
