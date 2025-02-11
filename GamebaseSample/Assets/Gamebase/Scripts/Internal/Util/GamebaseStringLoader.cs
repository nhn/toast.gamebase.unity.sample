#if (UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL)
using System.Collections;
using System.IO;
using Toast.Gamebase.Internal;
using Toast.Gamebase.Internal.Single.Communicator;
using UnityEngine.Networking;

public class GamebaseStringLoader
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
        return (filePath.Contains("://") == true) || (filePath.Contains(":///") == true);
    }

    private IEnumerator LoadWebFile(string filePath, System.Action<string> callback)
    {
        using (UnityWebRequest www = UnityWebRequest.Get(filePath))
        {
            www.timeout = CommunicatorConfiguration.timeout;

            yield return UnityCompatibility.WebRequest.Send(www);

            if (www.isDone != true)
            {
                yield break;
            }
            
            string jsonString = string.Empty;
            if (www.responseCode == 200)
            {
                if (UnityCompatibility.WebRequest.IsError(www) == false)
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
