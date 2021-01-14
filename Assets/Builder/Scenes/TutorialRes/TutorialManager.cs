using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public GameObject imgGroup;
    public Image[] b = new Image [9];

    public Button textBubble;
    public Text tutorialText;

    private int lvl = 0;
    private int dataholder = -1;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 9; i++)
        {
            b[i].GetComponent<Image>();
            b[i].enabled = true;
        }

        textBubble.GetComponent<Button>();
        textBubble.onClick.AddListener(tutorialLvlOnClick);
        
        int userFlag = GetUsername.flag;
        if (userFlag != 1)
        {
            imgGroup.SetActive(false);
            textBubble.gameObject.SetActive(false);
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (lvl == 0) //ONE
        {
            tutorialText.text = "You just witnessed an airplane crash and now you have to survive! (Press this button to continue)";
        }
        if (lvl == 1)//TWO
        {
            b[4].enabled = false;
            b[7].enabled = false;
            tutorialText.text = "It's getting cold. Tap on a tree and press 'chop' to cut it down";
            if (dataholder == -1)
            {
                dataholder = ItemOptionsWindowScript.chopCount;
            }

            if (ItemOptionsWindowScript.chopCount - dataholder != 0)
            {
                lvl++;
            }
            
        }

        if (lvl == 2)//THREE
        {
            b[4].enabled = true;
            b[7].enabled = true;
            b[8].enabled = false;
            
            tutorialText.text =
                "Good job! to see your resources, tap on the inventory button";

            if (UIManager.instance.clicked == true)
            {
                lvl++;
                UIManager.instance.clicked = false;
            }
            
        }

        if (lvl == 3)
        {
            b[8].enabled = true;

            tutorialText.text = "You are getting a hang of this! Good job! (Press to continue)";
        }

        if (lvl == 4)
        {
            //Supposed to open shop here

            try
            {
                InventoryWindowScript.instance.Close();
            }
            catch (Exception e)
            {
                
            }
            
            /*
            b[6].enabled = false;
            tutorialText.text = "Now tap on the build button on the bottom left";
            
            if (UIManager.instance.clicked == true)
            {
                lvl++;
                UIManager.instance.clicked = false;
            } 
            */
            lvl++;
        }

        if (lvl == 5)
        {
            //Supposed to automatically close shop here
            
            /*
            b[6].enabled = false;
            tutorialText.text =
                "The build window lets you build things that help you collect resources (Press to continue)";
            */
            lvl++;

        }

        if (lvl == 6)
        {
            b[0].enabled = false;
            tutorialText.text = "This is the level bar. It shows you the amount of experience you have (Press to continue)";
            
        }
        
        if (lvl == 7)
        {
            b[0].enabled = true;
            b[1].enabled = false;
            tutorialText.text = "This is the Corruption bar. It shows how bad the island's economy has become (Press to continue)";
            
        }
        
        if (lvl == 8)
        {
            b[0].enabled = true;
            b[1].enabled = false;
            tutorialText.text = "If it reaches to 100, its game over! Try to do eco friendly actions to lower the bar (Press to continue)";
            
        }
        
        if (lvl == 9)
        {
            b[1].enabled = true;
            b[2].enabled = false;
            tutorialText.text = "Last but not least, these are your resources. You can use them to build and upgrade things (Press to Continue)";
            
        }

        if (lvl == 10)
        {
            b[1].enabled = false;
            tutorialText.text = "You are now set. Good luck! (Press)";
        }
        
        if (lvl == 10)
        {
            textBubble.gameObject.SetActive(false);
            imgGroup.gameObject.SetActive(false);
        }

    }
    
    public void tutorialLvlOnClick(){
        if (lvl == 1 || lvl == 2 || lvl == 4) //should not be clickable
        {
            
        }
        else
        {
            lvl++;
        }

    }
}

