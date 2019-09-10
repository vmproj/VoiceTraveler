using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// 文字列を照合し、指定の処理をするクラス
/// </summary>
public class Recognition : MonoBehaviour
{

    //デバック用に変数をシリアライズ表示する

    [SerializeField] VoiceData voiceData;
    //命令リスト
    [SerializeField] List<string> dataList = new List<string> { };
    //Juliusから取得する文字列
    [SerializeField] Text juliusText;

    public bool isOpen = false; //外部参照
    private float openTime = 1.0f;
    private float countTime = 0f;
    private bool isCount = false;
    private void Start()
    {

        voiceData = FindObjectOfType<JsonProcess>().voiceData;

        //キーワード登録
        VoiceApply();

        juliusText = GameObject.Find("Canvas/VoiceWord").GetComponent<Text>();
    }

    private void Update()
    {
        if (!isCount)
        {
            if (Input.GetKeyDown(KeyCode.G))
                isOpen = true;
            else
                isOpen = dataList.Contains(juliusText.text);
            //オープン時1秒間ブール値そのまま
            if (isOpen)
            {
                juliusText.color = Color.green;
                isCount = true;
            }
            else
                juliusText.color = Color.white;

        }
        else
        {
            if (countTime < openTime)
                countTime += Time.deltaTime;
            else
            {
                countTime = 0f;
                isCount = false;
            }
        }
    }
    private void VoiceApply()
    {
        //string型に変換し、声データを登録する
        dataList.AddRange(voiceData.openWord);

    }
    //キーワードを更新
    public void VoiceApply(string word)
    {
        dataList = new List<string> { word };
    }
    public bool WordCheck(string password)
    {
        return false;
    }
}


