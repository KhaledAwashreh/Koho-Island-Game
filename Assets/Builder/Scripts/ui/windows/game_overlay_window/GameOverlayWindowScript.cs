using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverlayWindowScript : WindowScript
{
    public Button Expand;
    public Button Close;
    public GameObject ExpandedObj;
    

    public static GameOverlayWindowScript instance;

    /* object references */

    private PollutionWindow test;
    public ProgressPanelScript GoldInfo;
    public ProgressPanelScript ElixirInfo;
    public ProgressPanelScript DiamondInfo;
    public ProgressPanelScript totalPollution;
    public ProgressPanelScript XpInfo;
    public Text levelLabelPanel;
    public Text namePanel;

    private void Awake()
    {
        if (SceneManager.instance == null)
        {
            return;
        }

        instance = this;
    }

    private void Start()
    {
        //Expand = gameObject.GetComponent<Button>();
        //Expand.onClick.AddListener(OnClickMaximizeObjectives);
        //Close = gameObject.GetComponent<Button>();
        //ExpandedObj.SetActive(false);


        this.GoldInfo.hasMaxValue = true;
        this.GoldInfo.maxValue = SceneManager.instance.goldStorageCapacity;
        this.GoldInfo.value = SceneManager.instance.numberOfGoldInStorage;

        this.ElixirInfo.hasMaxValue = true;
        this.ElixirInfo.maxValue = SceneManager.instance.elixirStorageCapacity;
        this.ElixirInfo.value = SceneManager.instance.numberOfElixirInStorage;

        this.DiamondInfo.hasMaxValue = false;
        this.DiamondInfo.value = SceneManager.instance.numberOfDiamondsInStorage;

        this.XpInfo.hasMaxValue = true;
        this.XpInfo.maxValue = SceneManager.instance.currentXpCapacity;
        this.XpInfo.value = SceneManager.instance.currentXpCount;
        this.levelLabelPanel.text = SceneManager.instance.currentLevel + "";
        this.totalPollution.hasMaxValue = true;
        this.totalPollution.maxValue = 100;

        this.namePanel.text = "Name: " + DataBaseManager.instance.GetScene().user.name;
    }

    public void OnClickShopButton()
    {
        UIManager.instance.ShowShopWidow();
    }

    public void OnClickInventoryButton()
    {
        UIManager.instance.ShowInventoryWidow();
    }

    public void OnClickMaximizeObjectives() //for objective button
    {
        UIManager.instance.ShowMaximizedObjectivesWindow();
    }

    public void OnClickLeaderboardButton()
    {
        UIManager.instance.showLeaderBoardWindow();
    }

    public void onClickPollution()
    {
        UIManager.instance.ShowPollutionWindow();
    }

    //RESOURCE  COLLECTION
    public void CollectResource(string resourceType, int value)
    {
        if (resourceType == "gold")
        {
            GoldInfo.TweenValueChange(value);
        }
        else if (resourceType == "elixir")
        {
            ElixirInfo.TweenValueChange(value);
        }
    }
}