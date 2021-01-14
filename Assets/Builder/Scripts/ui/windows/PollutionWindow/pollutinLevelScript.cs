using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class pollutinLevelScript : MonoBehaviour
{
    public ProgressPanelScript PollutionLevel;

    private float previousCheck;

    // Update is called once per frame
    void Update()
    {
        if (Time.time - previousCheck > 3
            && ConsequencesSystem.instance.totalPollution < GameOverlayWindowScript.instance.totalPollution.maxValue)
        {    
            PollutionLevel.hasMaxValue = true;
            PollutionLevel.maxValue = 100;
            PollutionLevel.SetProgress((float) ConsequencesSystem.instance.totalPollution);
            PollutionLevel.value = (float) ConsequencesSystem.instance.totalPollution;
            previousCheck = Time.time;
        }
    }

    private void Start()
    {
        PollutionLevel.hasMaxValue = true;
        PollutionLevel.maxValue = 100;
        PollutionLevel.SetProgress((float) ConsequencesSystem.instance.totalPollution);
        PollutionLevel.value = (float) ConsequencesSystem.instance.totalPollution;
        previousCheck = Time.time;
    }
}