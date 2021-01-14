using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemOptionsWindowScript : WindowScript
{
    public static ItemOptionsWindowScript instance;

    /* object references */
    public GameObject InfoButton;
    public GameObject UpgradeButton;
    public GameObject TrainButton;
    public GameObject RemoveButton;
    public GameObject CollectLeavesButton;
    public GameObject CollectFruitButton;
    public GameObject CutTreeButton;
    public GameObject FoodButton;
    public static int chopCount = 0;

    private void Awake()
    {
        if (SceneManager.instance == null)
        {
            return;
        }

        instance = this;
        this.ShowOptions();
    }

    public void ShowOptions()
    {
        this.StartCoroutine(this._ShowOptions());
    }

    private float _waitTime = 0.08f;
    bool haveInfoButton = true;
    bool haveUpgradeButton = true;
    bool haveTrainButton = false;
    bool haveRemoveButton = true;
    bool haveCollectLeavesButton = false;
    bool haveCollectFruitButton = false;
    bool haveCutTreeButton = false;
    bool haveFoodButton = false;

    private IEnumerator _ShowOptions()
    {
        BaseItemScript selectedItem = SceneManager.instance.selectedItem;

        if (selectedItem.itemData.name == "Tree1" || selectedItem.itemData.name == "Tree2" ||
            selectedItem.itemData.name == "Tree3" || selectedItem.itemData.name == "coconutTree")
        {
            haveCollectLeavesButton = true;
            haveCollectFruitButton = true;
            haveCutTreeButton = true;
            haveUpgradeButton = false;
            haveRemoveButton = false;
        }
        else
        {
            haveUpgradeButton = true;
            haveRemoveButton = true;
        }

        if (selectedItem.itemData.name == "WindMill")
        {
            haveFoodButton = true;
        }

        haveInfoButton = true;

        InfoButton.SetActive(haveInfoButton);
        UpgradeButton.SetActive(haveUpgradeButton);
        TrainButton.SetActive(haveTrainButton);
        RemoveButton.SetActive(haveRemoveButton);
        CutTreeButton.SetActive(haveCutTreeButton);
        CollectFruitButton.SetActive(haveCollectFruitButton);
        CollectLeavesButton.SetActive(haveCollectLeavesButton);

        if (haveInfoButton)
        {
            InfoButton.GetComponent<Animator>().SetTrigger("show");
            yield return new WaitForSeconds(_waitTime);
        }

        if (haveUpgradeButton)
        {
            UpgradeButton.GetComponent<Animator>().SetTrigger("show");
            yield return new WaitForSeconds(_waitTime);
        }

        if (haveRemoveButton)
        {
            RemoveButton.GetComponent<Animator>().SetTrigger("show");
            yield return new WaitForSeconds(_waitTime);
        }

        if (haveCutTreeButton)
        {
            CutTreeButton.GetComponent<Animator>().SetTrigger("show");
            yield return new WaitForSeconds(_waitTime);
        }

        if (haveCollectLeavesButton)
        {
            CollectLeavesButton.GetComponent<Animator>().SetTrigger("show");
            yield return new WaitForSeconds(_waitTime);
        }

        if (haveCollectFruitButton)
        {
            CollectFruitButton.GetComponent<Animator>().SetTrigger("show");
            yield return new WaitForSeconds(_waitTime);
        }

        if (haveFoodButton)
        {
            FoodButton.GetComponent<Animator>().SetTrigger("show");
            yield return new WaitForSeconds(_waitTime);
        }
    }

    public void HideOptions()
    {
        this.StartCoroutine(this._HideOptions());
    }

    private IEnumerator _HideOptions()
    {
        if (haveInfoButton)
        {
            InfoButton.GetComponent<Animator>().SetTrigger("hide");
            yield return new WaitForSeconds(_waitTime);
        }

        if (haveUpgradeButton)
        {
            UpgradeButton.GetComponent<Animator>().SetTrigger("hide");
            yield return new WaitForSeconds(_waitTime);
        }

        if (haveTrainButton)
        {
            TrainButton.GetComponent<Animator>().SetTrigger("hide");
            yield return new WaitForSeconds(_waitTime);
        }

        if (haveRemoveButton)
        {
            RemoveButton.GetComponent<Animator>().SetTrigger("hide");
            yield return new WaitForSeconds(_waitTime);
        }

        if (haveCutTreeButton)
        {
            CutTreeButton.GetComponent<Animator>().SetTrigger("hide");
            yield return new WaitForSeconds(_waitTime);
        }

        if (haveCollectLeavesButton)
        {
            CollectLeavesButton.GetComponent<Animator>().SetTrigger("hide");
            yield return new WaitForSeconds(_waitTime);
        }

        if (haveCollectFruitButton)
        {
            CollectFruitButton.GetComponent<Animator>().SetTrigger("hide");
            yield return new WaitForSeconds(_waitTime);
        }

        if (haveFoodButton)
        {
            FoodButton.GetComponent<Animator>().SetTrigger("hide");
            yield return new WaitForSeconds(_waitTime);
        }

        base.Close();
    }

    public void OnClickInfoButton()
    {
        UIManager.instance.ShowInfoWindow();
    }


    public void OnClickTrainButton()
    {
        UIManager.instance.ShowTrainTroopsWindow();
    }

    public void OnClickRemoveButton()
    {
        UIManager.instance.HideItemOptions();
        DataBaseManager.instance.RemoveItem(SceneManager.instance.selectedItem);
        SceneManager.instance.RemoveItem(SceneManager.instance.selectedItem);
    }

    public void OnClickCutTreeButton()
    {
        UIManager.instance.HideItemOptions();
        DataBaseManager.instance.UpdateResourceData(DataBaseManager.WOOD_RESOURCE_NAME, 5, true);
        SceneManager.instance.RemoveItem(SceneManager.instance.selectedItem);
        DataBaseManager.instance.RemoveItem(SceneManager.instance.selectedItem);
        SceneManager.instance.UpdateResourcesValues();
        chopCount++;
        ConsequencesSystem.instance.save();
    }

    public void OnClickCollectLeavesButton()
    {
        UIManager.instance.HideItemOptions();
        DataBaseManager.instance.UpdateResourceData(DataBaseManager.LEAVES_RESOURCE_NAME, 5, true);
        SceneManager.instance.UpdateResourcesValues();
    }

    public void OnClickCollectFruitsButton()
    {
        UIManager.instance.HideItemOptions();
        DataBaseManager.instance.UpdateResourceData(DataBaseManager.FRUIT_RESOURCE_NAME, 5, true);
        SceneManager.instance.UpdateResourcesValues();
    }

    public void OnClickProduceFoodButton()
    {
        UIManager.instance.HideItemOptions();
        DataBaseManager.instance.UpdateResourceData(DataBaseManager.FRUIT_RESOURCE_NAME, 5, false);
        DataBaseManager.instance.UpdateResourceData(DataBaseManager.FOOD_RESOURCE_NAME, 2, true);
        SceneManager.instance.UpdateResourcesValues();
    }


    public override void Close()
    {
        HideOptions();
    }
}