using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

//難易度を分けたファイルから、スコア・名前を取得する
public class TextReader : MonoBehaviour
{

    [SerializeField] private StagePoint[] stagePoint = new StagePoint[4];
    
    private void Start()
    {
        ReadFile("Score/EX",3);
        ReadFile("Score/Hard",2);
        ReadFile("Score/Normal",1);
        ReadFile("Score/Easy", 0);
    }

    //データパスを引数にして名前とスコアを代入する
    private void ReadFile(string str, int num) //num:何番目のファイルを読み込むか
    {

        string temppath = Application.dataPath + "/StreamingAssets/" + str + ".txt";

        StreamReader sr = new StreamReader(temppath);

        var txt = sr.ReadToEnd();//全部読み込む

        var r_data = txt.Split(char.Parse("\n")); //１行ずつ分ける
        var row = r_data[0].Split(char.Parse(","));//1行目の名前とスコアに分ける
        stagePoint[num].HighScore = int.Parse(row[1]);
        stagePoint[num].UserName = row[0];
       
        sr.Close();
    }
}
