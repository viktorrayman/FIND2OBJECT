using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


public class TimeManager : MonoBehaviour
{
    public Text timerText;
    
    public float additiveTime;
    public System.Action OnTimesEnd;


    DateTime time;



    private void Start()
    {
        

    }

    public void StartTime(float startTime, float additiveTime)
    {
        time = new DateTime();
        time = time.AddSeconds(startTime);

        this.additiveTime = additiveTime;
    }
    public void AddAdditiveTime()
    {
        time = time.AddSeconds(additiveTime);
    }
    private void    Update()
    {
        timerText.text = time.ToString("ss:ff");

        DateTime subtracted = new DateTime();
        subtracted = subtracted.AddSeconds(Time.deltaTime);

        TimeSpan newTime = time.Subtract(subtracted);
        if(newTime.Ticks > 0)
        {
            time = new DateTime() + newTime;
        }
        else
        {
            OnTimesEnd?.Invoke();
        }
    }
}
