using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StagePoint : MonoBehaviour
{
    [SerializeField]
    string stageName;
    [SerializeField]
    bool canGo;
    [SerializeField]
    int difficulty;
    [SerializeField]
    int highScore;
    [SerializeField]
    string userName;

    public bool CanGo { get { return canGo; } }
    public string StageName { get { return stageName; } }
    public int Difficulty { get { return difficulty; } }
    public int HighScore { get { return highScore; } set { highScore = value; } }
    public string UserName { get { return userName; }set { userName = value; } }

    void Start()
    {

    }

    void Update()
    {

    }
}
