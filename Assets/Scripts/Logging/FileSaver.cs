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

        //private static string FILE_NAME = "/logData.json";
        private static string FILE_NAME = "/logData.csv";

        public static void saveToFile(string logRow)
        {
            logRow += "\n";
            try
            {
                
                StreamWriter writer = new StreamWriter(Application.persistentDataPath + FILE_NAME, true, System.Text.Encoding.UTF8);
                //logRow = logRow.Replace(";", ",");
                writer.WriteLine(logRow);
                //writer.Write(JsonUtility.ToJson(new SerializableWrapper(logRow)));
                writer.Flush();
                writer.Close();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
    }
}