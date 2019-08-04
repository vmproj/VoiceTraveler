using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ここに反応するキーワードを入れる。
/// StreamingAssets/FreqData.json にデータを記述する
/// </summary>

[Serializable]
public class VoiceData {

    [SerializeField]public string[] openWord;
    [SerializeField]public string maxFreq, minFreq;
}
