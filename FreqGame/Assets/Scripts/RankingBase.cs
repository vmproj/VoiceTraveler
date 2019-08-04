using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using System.Text;

public class RankingBase : MonoBehaviour
{

    protected static string txt = "";
    protected static int Ranking_Num = 5;
    protected static string[] r_data = new string[Ranking_Num];
    protected static string[][] row = new string[5][];
    protected static int[] _score = new int[Ranking_Num]; //static
    protected static string[] _name = new string[Ranking_Num];
    [SerializeField]
    protected GUIStyle customGui; //= new GUIStyle();
    private string data_pass = "Score/Normal"; //Normalがデフォ

    public static void ReadFile(string str)
    {

        //TextAsset filedata = Resources.Load(str) as TextAsset;
        string temppath = Application.dataPath + "/StreamingAssets/" + str + ".txt";

        StreamReader sr = new StreamReader(temppath);

        txt = sr.ReadToEnd();//全部読み込む

        r_data = txt.Split(char.Parse("\n")); //１行ずつ分ける
        for (int i = 0; i < Ranking_Num; i++)
        {
            row[i] = r_data[i].Split(char.Parse(",")); //名前とスコアに分ける
            _name[i] = row[i][0];
            _score[i] = int.Parse(row[i][1]); //int型に変換
        }
        sr.Close();
    }


    public static void SaveRanking(int new_score, string name_e, string data_pass)
    {
        int n_tmp = 0;
        string s_tmp = "";

        string r_data = "";
        ReadFile(data_pass);

        for (int i = 0; i < _name.Length && i < _score.Length; i++)
        {
            if (_score[i] < new_score)
            {
                n_tmp = _score[i];
                _score[i] = new_score;
                new_score = n_tmp;

                s_tmp = _name[i];
                _name[i] = name_e;
                name_e = s_tmp;
            }
        }

        for (int i = 0; i < _name.Length && i < _score.Length; i++)
        {
            r_data = r_data + _name[i] + ',' + _score[i] + '\n';
        }

        StreamWriter sw = new StreamWriter(Application.dataPath + "/StreamingAssets/" + data_pass + ".txt", false); //修正まだ //falseだと上書き
       
        sw.WriteLine(r_data);//上書きする内容
        sw.Close();

    }

    // 何位に更新されたかを返す
    public static int Return_Rankin(string path, int score)
    {
        ReadFile(path);

        for (int i = 0; i < _name.Length && i < _score.Length; i++)
        {
            if (_score[i] < score)
            {
                return (i+1);
            }
        }
        return -1;

    }

    public void OnGUI()
    {
        string ranking_string = "";
        for (int i = 0; i < _name.Length && i < _score.Length; i++)
        {
            ranking_string = ranking_string + (i + 1) + " 位   " +
            String.Format("{0,-10}\t", _name[i])  + String.Format("{0,8}\t",_score[i]) + "\n";
        }
        //GUILayout.Label(ranking_string, customGui);
    }
}

