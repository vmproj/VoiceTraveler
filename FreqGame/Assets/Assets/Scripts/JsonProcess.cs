using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

public class JsonProcess : MonoBehaviour
{

    //Jsonのファイルパス
    string filePath;
    string json;
    string jd;

    //VoiceDataクラス
    public VoiceData voiceData;

    void Awake()
    {
        voiceData = new VoiceData();

        filePath = Application.streamingAssetsPath + "/FreqData.json";
        DataRead();

    }

    /// <summary>
    /// データの読み取り
    /// </summary>
    public void DataRead()
    {
        //ファイルの読み取り
        json = File.ReadAllText(filePath, Encoding.UTF8);
        voiceData = JsonUtility.FromJson<VoiceData>(json);
    }
    public void DataSave()
    {
        //保存するデータをstring型に変換
        json = JsonUtility.ToJson(voiceData);
        File.WriteAllText(filePath, json, Encoding.UTF8);
    }

    //周波数登録してセーブ
    public void FreqChange(int min,int max)
    {
        voiceData.maxFreq = max.ToString();
        voiceData.minFreq = min.ToString();
        DataSave();
    }
}
