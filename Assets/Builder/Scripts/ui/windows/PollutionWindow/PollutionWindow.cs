using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PollutionWindow : WindowScript
{
    private float previousCheck;

    public ProgressPanelScript AirPollution;
    public ProgressPanelScript waterPollution;
    public ProgressPanelScript landPollution;
    List<GameObject> goList = new List<GameObject>();
    public static PollutionWindow instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        previousCheck = Time.time;
        AirPollution.hasMaxValue = true;
        AirPollution.maxValue = 100;
        AirPollution.SetProgress((float) ConsequencesSystem.instance.airPollution);
        AirPollution.value = (float) ConsequencesSystem.instance.airPollution;
        
        landPollution.hasMaxValue = true;
        landPollution.maxValue = 100;
        landPollution.SetProgress((float) ConsequencesSystem.instance.landPollution);
        landPollution.value = (float) ConsequencesSystem.instance.landPollution;
      
        waterPollution.hasMaxValue = true;
        waterPollution.maxValue = 100;
        waterPollution.SetProgress((float) ConsequencesSystem.instance.waterPollution);
        waterPollution.value = (float) ConsequencesSystem.instance.waterPollution;

    }

    private void Update()
    {
        if (Time.time - previousCheck > 3)
        {
            AirPollution.value = (float) ConsequencesSystem.instance.airPollution;
            waterPollution.value = (float) ConsequencesSystem.instance.waterPollution;
            landPollution.value = (float) ConsequencesSystem.instance.landPollution;
            previousCheck = Time.time;
        }
    }
}