using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine;


public class ConsequencesSystem : MonoBehaviour
{
    public static ConsequencesSystem instance;

    private ConsequencesData temp;
    private float previousCheck;

    public int treeCount;

    //system values
    public double enviromentScore;
    double air = 1;
    public double Co2Level;
    public double O2Level;
    public double soilFertility;
    public double flowersAndGrass;
    public double otherGasses;

    //pollution
    public double totalPollution;
    public double airPollution;
    public double waterPollution;

    public double landPollution;

    //regan rates for trees and enviroment
    private double o2ConsumptionRate = 0.02;
    private double co2ReganRate = 0.002;


    private void Awake()
    {
        instance = this;
    }

//this method sets in system values and their initial values. the reason we are passing 
    // values instead of having them be blank is to have the ability to restore the system to any point of desired
    // including pollution level values. Please set the values to 0 at the start of the game 
    public void Start()
    {
        temp = DataBaseManager.instance.getConsequencesData();
        load();
        countTrees();
        previousCheck = Time.time;
    }

    private void Update()
    {
        if (Time.time - previousCheck > 3)

        {
            countTrees();
            air = otherGasses + Co2Level + O2Level;
            tuneSystem();
            air = otherGasses + Co2Level + O2Level;
            countTrees();
            updateAirPollution();
            updateTotalPollutionLevel();
            tuneSystem();
            save();
           
            previousCheck = Time.time;
        }
    }

    public void save()
    {
        DataBaseManager.instance.updateConsequencesData(air, Co2Level, O2Level, soilFertility, flowersAndGrass,
            otherGasses, totalPollution, airPollution, waterPollution, landPollution);
    }

    public void load()
    {
        this.landPollution = temp.landPollution;
        this.waterPollution = temp.waterPollution;
        this.airPollution = temp.landPollution;
        this.totalPollution = temp.landPollution;
        this.O2Level = temp.O2Level;
        this.Co2Level = temp.Co2Level;
        this.soilFertility = temp.soilFertility;
        otherGasses = temp.air;
        flowersAndGrass = temp.flowersAndGrass;
    }


    // used for items that influence the system with a specified time period. Please use this method when u have resources that add constantly.


    public void tuneSystem() // to check if the system values exceed what is allowed
    {
        if (air > 1)
            air = 1;
        if (Co2Level / air < 0.04)
            Co2Level = 0.04;
        if (O2Level / air > 0.2095)
            O2Level = 0.2095;
    }

    public void updateAirPollution() // updating values of air pollution based on levels of co2 and o2
    {
        Co2Level = Co2Level - (treeCount * 0.001) + co2ReganRate;
        O2Level = O2Level + (treeCount * 0.02) - 0.04;


        double excessiveCo2 = 0;
        double lackOfo2 = 0;
        if (O2Level < 0.2)
        {
            int counter = 0;
            double divide = O2Level;
            while (divide < 0.2)
            {
                divide = divide + 0.020;
                counter++;
            }

            lackOfo2 = counter * 5;
        }

        if (Co2Level > 0.045)
        {
            int counter = 0;
            double divide = Co2Level;
            while (divide > 0.045)
            {
                divide = divide - 0.005;
                counter++;
            }

            excessiveCo2 = counter * 10;
        }

        airPollution = excessiveCo2 + lackOfo2;
        if (airPollution > 100)
            airPollution = 100;
        updateTotalPollutionLevel();
    }

    private void countTrees()
    {
        List<GameObject> goList = new List<GameObject>();
        string nameToAdd = "coconutTree [INSTANCE]";

        foreach (GameObject go in GameObject.FindObjectsOfType(typeof(GameObject)))
        {
            if (go.name == nameToAdd)
                goList.Add(go);
        }

        treeCount = goList.Count;
    }


    public void updateTotalPollutionLevel() //updating total pollution levels based on the ratios of pollution
    {
        totalPollution = (0.5 * airPollution) + (0.3 * soilFertility) + (0.2 * landPollution);
    }
}