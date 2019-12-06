#if (UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL)
using System.Collections;
using System.IO;
using Toast.Gamebase.Internal;
using Toast.Gamebase.Internal.Single.Communicator;
using UnityEngine;
using UnityEngine.Networking;

public class GamebaseStringLoader : MonoBehaviour
{
    public void LoadStringFromFile(string filePath, System.Action<string> callback)
    {        
        if (IsWebFilePath(filePath) == true)
        {
            GamebaseCoroutineManager.StartCoroutine(
                GamebaseGameObjectManager.GameObjectType.STRING_LOADER, 
                LoadWebFile(
                    filePath, 
                    callback));
        }
        else
        {
            LoadLocalFile(filePath, callback);
        }
    }

    private bool IsWebFilePath(string filePath)
    {
        return (true == filePath.Contains("://")) || (true == filePath.Contains(":///"));
    }

    private IEnumerator LoadWebFile(string filePath, System.Action<string> callback)
    {
        UnityWebRequest www = UnityWebRequest.Get(filePath);
        www.timeout = CommunicatorConfiguration.timeout;

        yield return UnityCompatibility.UnityWebRequest.Send(www);

        if (www.isDone == true)
        {
            string jsonString = string.Empty;

            if (200 == www.responseCode)
            {
                if (UnityCompatibility.UnityWebRequest.IsError(www) == false)
                {                 
                    if (string.IsNullOrEmpty(www.downloadHandler.text) == false)
                    {
                        jsonString = www.downloadHandler.text;
                    }
                }
            }

            callback(jsonString);
        }
    }

    private void LoadLocalFile(string filePath, System.Action<string> callback)
    {
        string jsonString = string.Empty;

        if (File.Exists(filePath) == true)
        {
            jsonString = File.ReadAllText(filePath);            
        }

        callback(jsonString);
    }
}
#endif
