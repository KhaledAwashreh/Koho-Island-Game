using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class objectives : MonoBehaviour
{
    //For small Objective ui
    public Text objText; //Holds obj title
    public Text subText; //Holds subtext
    
    //For expanded Objective ui
    public Text ExpObjText;
    public Text ExpSubText;
    public Text ExpXp;
    public Text Rewards;
    
    private int region;
    private int dataHolder; // = save current data so that i can compare

    public int flag;
    // Start is called before the first frame update
    private void Awake()
    {
        flag = GetUsername.flag;
        //Check if it is a new game, and if it is make everything default
        if (flag == 1)
        { 
            DataBaseManager.instance.addUser();
            DataBaseManager.instance.DefaultObjectives();
            flag = -1;
        }
    }

    void Start()
    {
        region = 1;
        dataHolder = -1;
        objText.text = "Error";
        subText.text = "Error";
        
        
    }

    // Update is called once per frame
    void Update()
    {
        // If player is in region 1
        if (region == 1)
        {
            region1();
        }
    }
    
    public void region1() 
    {
        //Main data for region 1
        ObjectiveData obj = DataBaseManager.instance.GetObjective(1);
        string title = "Start a Fire" ;
        string[] sub1 = {"Gather 10 wood", "Chop 3 trees", "build a campfire"};
        bool[] c1 = DataBaseManager.instance.GetObjective(1).completed;   
        int[] exp1 = {4, 3, 4};
        string[] rewards1 = {"gold", "food", "gold"};
        int[] amount1 = {2, 3, 4};
        
        //Putting data into UI for sub 0
        objText.text = title;
        ExpObjText.text = title + " (1/3)";
        ExpSubText.text = sub1[0];
        ExpXp.text = "Exp Earned: " + exp1[0].ToString();

        string r = ""; //Rewards for region 1
        for (int i = 0; i < rewards1.Length; i++)
        {
            r = r + " " + rewards1[i] + " " + amount1[i] + "\n";
        }
        Rewards.text = r;
        
        //////////////////////////////////////////////////////////////////////////////////////////////SUBOBJECTIVE 1
        if (c1[0] == false)
        {

            // dataholder: takes initial amount that the player had as soon as the region unlocked. Used to compare between future amount of resource
            if (dataHolder == -1)
            {
                dataHolder = DataBaseManager.instance.GetResourceData(DataBaseManager.WOOD_RESOURCE_NAME);
            }
            
            int n = 10; //Represents the amount of wood needed to get (goal)
            //check if player reached the goal
            int diff = (DataBaseManager.instance.GetResourceData(DataBaseManager.WOOD_RESOURCE_NAME) - dataHolder );
            if (diff < n)
            {
                subText.text = sub1[0] + ": " + diff + "/" + n;
            }
            else if (diff >= n)
            {
                c1[0] = true;
                subText.text = sub1[0] + ": \n           Complete";
                DataBaseManager.instance.UpdateObjectiveData(1, c1);
                dataHolder = -1;
                //changes c1[0] to true
            }
        }
        
        
        ///////////////////////////////////////////////////////////////////////////////////////////////SUBOBJECTIVE2
        else if (c1[1] == false && c1[0] == true) //c1[0] must be true in order to unlock the next objective
        {
            // Sub2 Data  
            subText.text = sub1[1];
            ExpObjText.text = title + " (2/3)";
            ExpSubText.text = sub1[1];
            ExpXp.text = "Exp Earned: " + exp1[1].ToString();
            
            if (dataHolder == -1)
            {
                dataHolder = ItemOptionsWindowScript.chopCount;
            }

            int n = 3;
            int diff = ItemOptionsWindowScript.chopCount - dataHolder;
            if (diff < 3)
            {
                subText.text = sub1[1] + ": " + diff + "/" + n;
            }
            else if (diff >= n)
            {
                c1[1] = true;
                subText.text = sub1[1] + ": \n           Complete";
                DataBaseManager.instance.UpdateObjectiveData(1, c1);
                dataHolder = -1;
                //changes c1[1] to true
            }
            
        }
    }
    
    
}
