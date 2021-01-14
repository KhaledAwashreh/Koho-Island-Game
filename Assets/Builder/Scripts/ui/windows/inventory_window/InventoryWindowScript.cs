using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryWindowScript : WindowScript
{
    public static InventoryWindowScript instance;

    /* object references */
    public ProgressPanelScript GoldInfo;
    public ProgressPanelScript ElixirInfo;
    public ProgressPanelScript DiamondInfo;
    public ProgressPanelScript WoodInfo;
    public ProgressPanelScript LeavesInfo;
    public ProgressPanelScript FruitsInfo;
    public ProgressPanelScript FoodInfo;

    void Awake()
    {
        instance = this;
        if (SceneManager.instance == null)
            return;

        this.Init();
    }

    private void Start()
    {
        this.GoldInfo.hasMaxValue = true;
        this.GoldInfo.maxValue = SceneManager.instance.goldStorageCapacity;
        this.GoldInfo.value = SceneManager.instance.numberOfGoldInStorage;

        this.ElixirInfo.hasMaxValue = true;
        this.ElixirInfo.maxValue = SceneManager.instance.elixirStorageCapacity;
        this.ElixirInfo.value = SceneManager.instance.numberOfElixirInStorage;

        this.WoodInfo.hasMaxValue = true;
        this.WoodInfo.maxValue = SceneManager.instance.woodStorageCapacity;
        this.WoodInfo.value = SceneManager.instance.numberOfWoodsInStorage;

        this.LeavesInfo.hasMaxValue = true;
        this.LeavesInfo.maxValue = SceneManager.instance.leavsStorageCapacity;
        this.LeavesInfo.value = SceneManager.instance.numberOfLeavesInStorage;

        this.FruitsInfo.hasMaxValue = true;
        this.FruitsInfo.maxValue = SceneManager.instance.fruitsStorageCapacity;
        this.FruitsInfo.value = SceneManager.instance.numberOfFruitsInStorage;

        this.FoodInfo.hasMaxValue = true;
        this.FoodInfo.maxValue = SceneManager.instance.foodCapacity;
        this.FoodInfo.value = SceneManager.instance.numberOfFoodInStorage;

        this.DiamondInfo.hasMaxValue = false;
        this.DiamondInfo.value = SceneManager.instance.numberOfDiamondsInStorage;
    }

    private void OnDestroy()
    {
        instance = null;
    }

    public void Init()
    {
        this.RenderInfo();
    }

    public void RenderInfo()
    {
    }
}