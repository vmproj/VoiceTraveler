using System.Collections;
using UnityEngine;

class JuliusSample : MonoBehaviour
{
    [SerializeField] bool isDebug = false;

    public IEnumerator Start()
    {
        yield return Application.RequestUserAuthorization(UserAuthorization.Microphone);

        var path = Application.dataPath;
        switch (Application.platform)
        {
            case RuntimePlatform.WindowsPlayer:
                path += "/StreamingAssets/";
                break;
            case RuntimePlatform.WindowsEditor:
                path += "/StreamingAssets/";
                break;
            case RuntimePlatform.IPhonePlayer:
                path += "/Raw/";
                break;
            default:
                path += "/StreamingAssets/";
                break;

        }
        path += "grammar-kit/grammar/mic.jconf";

        Julius.ResultReceived += OnResultReceived;
        Julius.Begin(path);
    }

    public string lastResult = "";

    void OnResultReceived(string result)
    {
        if (result.Contains("confidence_score"))
        {
            lastResult = "<FinalResult>\n";
        }
        else
        {
            lastResult = "<First Pass Progress>\n";
        }

        lastResult += result;
    }

    void OnGUI()
    {
        if (isDebug)
            GUI.Label(new Rect(0, 0, Screen.width, Screen.height), lastResult);
    }

    void OnDestroy()
    {
        Julius.Finish();
    }
}
