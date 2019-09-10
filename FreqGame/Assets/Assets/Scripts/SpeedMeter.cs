using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class SpeedMeter : MonoBehaviour
{
    [SerializeField] private Image[] image;
    private SpeedLevelText speedLevelText;
    private int preSpeedLevel = 0;
    private void Start()
    {
        speedLevelText = GameObject.FindObjectOfType<SpeedLevelText>();
        this.ObserveEveryValueChanged(x => speedLevelText.speed_Level).
            Subscribe(_ => SetSpeedUI());
    }
    private void SetSpeedUI()
    {
        if(speedLevelText.speed_Level == 5)
        {
            image[4].color += Color.black * 0.60f;
        }
        else if(speedLevelText.speed_Level == 4)
        {
            if(preSpeedLevel == 3) 
                image[3].color += Color.black * 0.60f;
            else if(preSpeedLevel == 5)
                image[4].color -= Color.black * 0.60f;
        }
        else if (speedLevelText.speed_Level == 3)
        {
            if (preSpeedLevel == 2)
                image[2].color += Color.black * 0.60f;
            else if (preSpeedLevel == 4)
                image[3].color -= Color.black * 0.60f;
        }
        else if (speedLevelText.speed_Level == 2)
        {
            if (preSpeedLevel == 1)
                image[1].color += Color.black * 0.60f;
            else if (preSpeedLevel == 3)
                image[2].color -= Color.black * 0.60f;
        }
        else if (speedLevelText.speed_Level == 1)
        {
            if (preSpeedLevel == 0)
                image[0].color += Color.black * 0.60f;
            else if (preSpeedLevel == 2)
                image[1].color -= Color.black * 0.60f;
        }
        else if(speedLevelText.speed_Level == 0)
        {
            for(int i = 0; i < 5; i++)
            {
                if(image[i].color.a > 0)
                {
                    image[i].color -= Color.black * 0.60f;
                }
            }
        }
        preSpeedLevel = speedLevelText.speed_Level;
    }
}
