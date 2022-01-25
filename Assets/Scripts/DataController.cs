using UnityEngine;

namespace Logic
{
    public class DataController : MonoBehaviour
    {
        protected void Awake()
        {
            GameObject[] objs = GameObject.FindGameObjectsWithTag("DataHolder");
            if (objs.Length > 1)
            {
                // The 'Awake' method is called on a new instance of the object, not on the original one, so we should kill this.gameObject
                Destroy(gameObject);
                return;
            }
            FindObjectOfType<Storage>().removeAllFromStorage();
            DontDestroyOnLoad(gameObject);
        }
    }
}
