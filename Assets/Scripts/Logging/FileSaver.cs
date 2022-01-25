using System;
using System.IO;
using UnityEngine;

namespace Logic
{
    public class FileSaver : MonoBehaviour
    {
        [Serializable]
        private struct SerializableWrapper
        {
            public SerializableWrapper(String row)
            {
                Row = row;
            }

            public String Row;
        }

        private static string FILE_NAME = "/logData.json";

        public static void saveToFile(string logRow)
        {
            try
            {
                StreamWriter writer = new StreamWriter(Application.persistentDataPath + FILE_NAME, true, System.Text.Encoding.UTF8);
                writer.Write(JsonUtility.ToJson(new SerializableWrapper(logRow)));
                writer.Close();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
    }
}