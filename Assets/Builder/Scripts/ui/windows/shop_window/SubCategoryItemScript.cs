using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SubCategoryItemScript : MonoBehaviour
{
    /* prefabs */
    public Sprite BarrackSprite;
    public Sprite BoatSprite;
    public Sprite BuilderHutSprite;
    public Sprite CampSprite;
    public Sprite CannonSprite;
    public Sprite ElixirCollectorSprite;
    public Sprite ElixirStorageSprite;
    public Sprite GemsSprite;
    public Sprite GoldMineSprite;
    public Sprite GoldStorageSprite;
    public Sprite TowerSprite;
    public Sprite TownCenterSprite;
    public Sprite Tree1Sprite;
    public Sprite Tree2Sprite;
    public Sprite WindMillSprite;
    public Sprite WallSprite;
    public Sprite Tree3Sprite;
    public Sprite CoconutTreeSprite;
    public Sprite TentSprite;

    /* references */
    public Text Name;
    public Image Image;
    public Text goldPrice;
    public Text woodPrice;
    public Text leavesPrice;

    public GameObject LockPanel;
    public GameObject LockImage;
    public GameObject GoldCostPanel;
    public GameObject WoodCostPanel;
    public GameObject LeavesCostPanel;

    public Button subCategoryItemButton;

    /* private variables */
    private ShopWindowScript.SubCategory _subCategory;

    public void SetSubCategory(ShopWindowScript.SubCategory subCategory)
    {
        GoldCostPanel.SetActive(false);
        WoodCostPanel.SetActive(false);
        LeavesCostPanel.SetActive(false);
        this._subCategory = subCategory;

        switch (this._subCategory.subCategoryName)
        {
            case "BUILDER_HUT":
                this.Name.text = "BUILDER HUT";
                this.Image.sprite = this.BuilderHutSprite;
                if (this._subCategory.reqMap[DataBaseManager.GOLD_RESOURCE_NAME] > 0)
                {
                    GoldCostPanel.SetActive(true);
                    goldPrice.text = this._subCategory.reqMap[DataBaseManager.GOLD_RESOURCE_NAME] + "";
                }

                if (this._subCategory.reqMap[DataBaseManager.WOOD_RESOURCE_NAME] > 0)
                {
                    WoodCostPanel.SetActive(true);
                    woodPrice.text = this._subCategory.reqMap[DataBaseManager.WOOD_RESOURCE_NAME] + "";
                }

                this.LockImage.SetActive(this._subCategory.Locked);
                this.LockPanel.SetActive(this._subCategory.Locked);
                subCategoryItemButton.interactable = !this._subCategory.Locked;
                break;

            case "CAMP":
                this.Name.text = "CAMP";
                this.Image.sprite = this.CampSprite;
                if (this._subCategory.reqMap[DataBaseManager.GOLD_RESOURCE_NAME] > 0)
                {
                    GoldCostPanel.SetActive(true);
                    goldPrice.text = this._subCategory.reqMap[DataBaseManager.GOLD_RESOURCE_NAME] + "";
                }

                if (this._subCategory.reqMap[DataBaseManager.WOOD_RESOURCE_NAME] > 0)
                {
                    WoodCostPanel.SetActive(true);
                    woodPrice.text = this._subCategory.reqMap[DataBaseManager.WOOD_RESOURCE_NAME] + "";
                }

                this.LockImage.SetActive(this._subCategory.Locked);
                this.LockPanel.SetActive(this._subCategory.Locked);
                subCategoryItemButton.interactable = !this._subCategory.Locked;
                break;

            case "ELIXIR_COLLECTOR":
                this.Name.text = "ELIXIR COLLECTOR";
                this.Image.sprite = this.ElixirCollectorSprite;
                if (this._subCategory.reqMap[DataBaseManager.GOLD_RESOURCE_NAME] > 0)
                {
                    GoldCostPanel.SetActive(true);
                    goldPrice.text = this._subCategory.reqMap[DataBaseManager.GOLD_RESOURCE_NAME] + "";
                }

                if (this._subCategory.reqMap[DataBaseManager.WOOD_RESOURCE_NAME] > 0)
                {
                    WoodCostPanel.SetActive(true);
                    woodPrice.text = this._subCategory.reqMap[DataBaseManager.WOOD_RESOURCE_NAME] + "";
                }

                this.LockImage.SetActive(this._subCategory.Locked);
                this.LockPanel.SetActive(this._subCategory.Locked);
                subCategoryItemButton.interactable = !this._subCategory.Locked;
                break;

            case "ELIXIR_STORAGE":
                this.Name.text = "ELIXIR STORAGE";
                this.Image.sprite = this.ElixirStorageSprite;
                if (this._subCategory.reqMap[DataBaseManager.GOLD_RESOURCE_NAME] > 0)
                {
                    GoldCostPanel.SetActive(true);
                    goldPrice.text = this._subCategory.reqMap[DataBaseManager.GOLD_RESOURCE_NAME] + "";
                }

                if (this._subCategory.reqMap[DataBaseManager.WOOD_RESOURCE_NAME] > 0)
                {
                    WoodCostPanel.SetActive(true);
                    woodPrice.text = this._subCategory.reqMap[DataBaseManager.WOOD_RESOURCE_NAME] + "";
                }

                this.LockImage.SetActive(this._subCategory.Locked);
                this.LockPanel.SetActive(this._subCategory.Locked);
                subCategoryItemButton.interactable = !this._subCategory.Locked;
                break;

            case "GEMS":
                this.Name.text = "GEMS";
                this.Image.sprite = this.GemsSprite;
                if (this._subCategory.reqMap[DataBaseManager.GOLD_RESOURCE_NAME] > 0)
                {
                    GoldCostPanel.SetActive(true);
                    goldPrice.text = this._subCategory.reqMap[DataBaseManager.GOLD_RESOURCE_NAME] + "";
                }

                if (this._subCategory.reqMap[DataBaseManager.WOOD_RESOURCE_NAME] > 0)
                {
                    WoodCostPanel.SetActive(true);
                    woodPrice.text = this._subCategory.reqMap[DataBaseManager.WOOD_RESOURCE_NAME] + "";
                }

                this.LockImage.SetActive(this._subCategory.Locked);
                this.LockPanel.SetActive(this._subCategory.Locked);
                subCategoryItemButton.interactable = !this._subCategory.Locked;
                break;

            case "GOLD_MINE":
                this.Name.text = "GOLD MINE";
                this.Image.sprite = this.GoldMineSprite;
                if (this._subCategory.reqMap[DataBaseManager.GOLD_RESOURCE_NAME] > 0)
                {
                    GoldCostPanel.SetActive(true);
                    goldPrice.text = this._subCategory.reqMap[DataBaseManager.GOLD_RESOURCE_NAME] + "";
                }

                if (this._subCategory.reqMap[DataBaseManager.WOOD_RESOURCE_NAME] > 0)
                {
                    WoodCostPanel.SetActive(true);
                    woodPrice.text = this._subCategory.reqMap[DataBaseManager.WOOD_RESOURCE_NAME] + "";
                }

                this.LockImage.SetActive(this._subCategory.Locked);
                this.LockPanel.SetActive(this._subCategory.Locked);
                subCategoryItemButton.interactable = !this._subCategory.Locked;
                break;

            case "GOLD_STORAGE":
                this.Name.text = "GOLD STORAGE";
                this.Image.sprite = this.GoldStorageSprite;
                if (this._subCategory.reqMap[DataBaseManager.GOLD_RESOURCE_NAME] > 0)
                {
                    GoldCostPanel.SetActive(true);
                    goldPrice.text = this._subCategory.reqMap[DataBaseManager.GOLD_RESOURCE_NAME] + "";
                }

                if (this._subCategory.reqMap[DataBaseManager.WOOD_RESOURCE_NAME] > 0)
                {
                    WoodCostPanel.SetActive(true);
                    woodPrice.text = this._subCategory.reqMap[DataBaseManager.WOOD_RESOURCE_NAME] + "";
                }

                this.LockImage.SetActive(this._subCategory.Locked);
                this.LockPanel.SetActive(this._subCategory.Locked);
                subCategoryItemButton.interactable = !this._subCategory.Locked;
                break;

            case "TOWER":
                this.Name.text = "TOWER";
                this.Image.sprite = this.TowerSprite;
                if (this._subCategory.reqMap[DataBaseManager.GOLD_RESOURCE_NAME] > 0)
                {
                    GoldCostPanel.SetActive(true);
                    goldPrice.text = this._subCategory.reqMap[DataBaseManager.GOLD_RESOURCE_NAME] + "";
                }

                if (this._subCategory.reqMap[DataBaseManager.WOOD_RESOURCE_NAME] > 0)
                {
                    WoodCostPanel.SetActive(true);
                    woodPrice.text = this._subCategory.reqMap[DataBaseManager.WOOD_RESOURCE_NAME] + "";
                }

                this.LockImage.SetActive(this._subCategory.Locked);
                this.LockPanel.SetActive(this._subCategory.Locked);
                subCategoryItemButton.interactable = !this._subCategory.Locked;
                break;

            case "TOWN_CENTER":
                this.Name.text = "TOWN CENTER";
                this.Image.sprite = this.TownCenterSprite;
                if (this._subCategory.reqMap[DataBaseManager.GOLD_RESOURCE_NAME] > 0)
                {
                    GoldCostPanel.SetActive(true);
                    goldPrice.text = this._subCategory.reqMap[DataBaseManager.GOLD_RESOURCE_NAME] + "";
                }

                if (this._subCategory.reqMap[DataBaseManager.WOOD_RESOURCE_NAME] > 0)
                {
                    WoodCostPanel.SetActive(true);
                    woodPrice.text = this._subCategory.reqMap[DataBaseManager.WOOD_RESOURCE_NAME] + "";
                }

                this.LockImage.SetActive(this._subCategory.Locked);
                this.LockPanel.SetActive(this._subCategory.Locked);
                subCategoryItemButton.interactable = !this._subCategory.Locked;
                break;

            case "TREE1":
                this.Name.text = "TREE1";
                this.Image.sprite = this.Tree1Sprite;
                if (this._subCategory.reqMap[DataBaseManager.GOLD_RESOURCE_NAME] > 0)
                {
                    GoldCostPanel.SetActive(true);
                    goldPrice.text = this._subCategory.reqMap[DataBaseManager.GOLD_RESOURCE_NAME] + "";
                }

                if (this._subCategory.reqMap[DataBaseManager.WOOD_RESOURCE_NAME] > 0)
                {
                    WoodCostPanel.SetActive(true);
                    woodPrice.text = this._subCategory.reqMap[DataBaseManager.WOOD_RESOURCE_NAME] + "";
                }

                this.LockImage.SetActive(this._subCategory.Locked);
                this.LockPanel.SetActive(this._subCategory.Locked);
                subCategoryItemButton.interactable = !this._subCategory.Locked;
                break;

            case "TREE2":
                this.Name.text = "TREE2";
                this.Image.sprite = this.Tree2Sprite;
                if (this._subCategory.reqMap[DataBaseManager.GOLD_RESOURCE_NAME] > 0)
                {
                    GoldCostPanel.SetActive(true);
                    goldPrice.text = this._subCategory.reqMap[DataBaseManager.GOLD_RESOURCE_NAME] + "";
                }

                if (this._subCategory.reqMap[DataBaseManager.WOOD_RESOURCE_NAME] > 0)
                {
                    WoodCostPanel.SetActive(true);
                    woodPrice.text = this._subCategory.reqMap[DataBaseManager.WOOD_RESOURCE_NAME] + "";
                }

                this.LockImage.SetActive(this._subCategory.Locked);
                this.LockPanel.SetActive(this._subCategory.Locked);
                subCategoryItemButton.interactable = !this._subCategory.Locked;
                break;

            case "WINDMILL":
                this.Name.text = "WINDMILL";
                this.Image.sprite = this.WindMillSprite;
                if (this._subCategory.reqMap[DataBaseManager.GOLD_RESOURCE_NAME] > 0)
                {
                    GoldCostPanel.SetActive(true);
                    goldPrice.text = this._subCategory.reqMap[DataBaseManager.GOLD_RESOURCE_NAME] + "";
                }

                if (this._subCategory.reqMap[DataBaseManager.WOOD_RESOURCE_NAME] > 0)
                {
                    WoodCostPanel.SetActive(true);
                    woodPrice.text = this._subCategory.reqMap[DataBaseManager.WOOD_RESOURCE_NAME] + "";
                }

                this.LockImage.SetActive(this._subCategory.Locked);
                this.LockPanel.SetActive(this._subCategory.Locked);
                subCategoryItemButton.interactable = !this._subCategory.Locked;
                break;

            case "WALL":
                this.Name.text = "WALL";
                this.Image.sprite = this.WallSprite;
                if (this._subCategory.reqMap[DataBaseManager.GOLD_RESOURCE_NAME] > 0)
                {
                    GoldCostPanel.SetActive(true);
                    goldPrice.text = this._subCategory.reqMap[DataBaseManager.GOLD_RESOURCE_NAME] + "";
                }

                if (this._subCategory.reqMap[DataBaseManager.WOOD_RESOURCE_NAME] > 0)
                {
                    WoodCostPanel.SetActive(true);
                    woodPrice.text = this._subCategory.reqMap[DataBaseManager.WOOD_RESOURCE_NAME] + "";
                }

                this.LockImage.SetActive(this._subCategory.Locked);
                this.LockPanel.SetActive(this._subCategory.Locked);
                subCategoryItemButton.interactable = !this._subCategory.Locked;
                break;

            case "TREE3":
                this.Name.text = "TREE3";
                this.Image.sprite = this.Tree3Sprite;
                if (this._subCategory.reqMap[DataBaseManager.GOLD_RESOURCE_NAME] > 0)
                {
                    GoldCostPanel.SetActive(true);
                    goldPrice.text = this._subCategory.reqMap[DataBaseManager.GOLD_RESOURCE_NAME] + "";
                }

                if (this._subCategory.reqMap[DataBaseManager.WOOD_RESOURCE_NAME] > 0)
                {
                    WoodCostPanel.SetActive(true);
                    woodPrice.text = this._subCategory.reqMap[DataBaseManager.WOOD_RESOURCE_NAME] + "";
                }

                this.LockImage.SetActive(this._subCategory.Locked);
                this.LockPanel.SetActive(this._subCategory.Locked);
                subCategoryItemButton.interactable = !this._subCategory.Locked;
                break;
            case "Tent":
                this.Name.text = "Tent";
                this.Image.sprite = this.TentSprite;
                if (this._subCategory.reqMap[DataBaseManager.GOLD_RESOURCE_NAME] > 0)
                {
                    GoldCostPanel.SetActive(true);
                    goldPrice.text = this._subCategory.reqMap[DataBaseManager.GOLD_RESOURCE_NAME] + "";
                }

                if (this._subCategory.reqMap[DataBaseManager.WOOD_RESOURCE_NAME] > 0)
                {
                    WoodCostPanel.SetActive(true);
                    woodPrice.text = this._subCategory.reqMap[DataBaseManager.WOOD_RESOURCE_NAME] + "";
                }

                if (this._subCategory.reqMap[DataBaseManager.LEAVES_RESOURCE_NAME] > 0)
                {
                    LeavesCostPanel.SetActive(true);
                    leavesPrice.text = this._subCategory.reqMap[DataBaseManager.LEAVES_RESOURCE_NAME] + "";
                }

                this.LockImage.SetActive(this._subCategory.Locked);
                this.LockPanel.SetActive(this._subCategory.Locked);
                subCategoryItemButton.interactable = !this._subCategory.Locked;
                break;
            case "CoconutTree":
                this.Name.text = "Coconut Tree";
                this.Image.sprite = this.CoconutTreeSprite;
                if (this._subCategory.reqMap[DataBaseManager.GOLD_RESOURCE_NAME] > 0)
                {
                    GoldCostPanel.SetActive(true);
                    goldPrice.text = this._subCategory.reqMap[DataBaseManager.GOLD_RESOURCE_NAME] + "";
                }

                if (this._subCategory.reqMap[DataBaseManager.WOOD_RESOURCE_NAME] > 0)
                {
                    WoodCostPanel.SetActive(true);
                    woodPrice.text = this._subCategory.reqMap[DataBaseManager.WOOD_RESOURCE_NAME] + "";
                }

                this.LockImage.SetActive(this._subCategory.Locked);
                this.LockPanel.SetActive(this._subCategory.Locked);
                subCategoryItemButton.interactable = !this._subCategory.Locked;
                break;
        }
    }

    public void OnClick()
    {
        bool goldAlert = false;
        bool woodAlert = false;
        bool leavesAlert = false;

        if (SceneManager.instance.numberOfGoldInStorage >= _subCategory.reqMap[DataBaseManager.GOLD_RESOURCE_NAME]
            && SceneManager.instance.numberOfWoodsInStorage >= _subCategory.reqMap[DataBaseManager.WOOD_RESOURCE_NAME]
            && SceneManager.instance.numberOfLeavesInStorage >=
            _subCategory.reqMap[DataBaseManager.LEAVES_RESOURCE_NAME])
        {
            addItem();
        }
        else
        {
            if (_subCategory.reqMap[DataBaseManager.GOLD_RESOURCE_NAME] != 0)
            {
                goldAlert = true;
            }

            if (_subCategory.reqMap[DataBaseManager.WOOD_RESOURCE_NAME] != 0)
            {
                woodAlert = true;
            }

            if (_subCategory.reqMap[DataBaseManager.GOLD_RESOURCE_NAME] != 0)
            {
                leavesAlert = true;
            }

            if (goldAlert && leavesAlert && woodAlert)
            {
                UIManager.instance.ShowAlertWindow("Alert", "Not Enough Gold, Wood and Leaves !");
            }
            else if (goldAlert && leavesAlert)
            {
                UIManager.instance.ShowAlertWindow("Alert", "Not Enough Gold and Leaves !");
            }
            else if (woodAlert && leavesAlert)
            {
                UIManager.instance.ShowAlertWindow("Alert", "Not Enough Wood and Leaves !");
            }
            else if (woodAlert && goldAlert)
            {
                UIManager.instance.ShowAlertWindow("Alert", "Not Enough Wood and Gold !");
            }
        }
    }

    public void addItem()
    {
        int itemId = 0;

        switch (this._subCategory.subCategoryName)
        {
            case "BARRACK":
                itemId = 8833;
                break;
            case "BOAT":
                itemId = 6871;
                break;
            case "BUILDER_HUT":
                itemId = 3635;
                break;
            case "CAMP":
                itemId = 2728;
                break;
            case "CANNON":
                itemId = 1712;
                break;
            case "ELIXIR_COLLECTOR":
                itemId = 4856;
                break;
            case "ELIXIR_STORAGE":
                itemId = 2090;
                break;
            case "GEMS":
                itemId = 3336;
                break;
            case "GOLD_MINE":
                itemId = 3265;
                break;
            case "GOLD_STORAGE":
                itemId = 9074;
                break;
            case "TOWER":
                itemId = 4764;
                break;
            case "TOWN_CENTER":
                itemId = 2496;
                break;
            case "TREE1":
                itemId = 2949;
                break;
            case "TREE2":
                itemId = 1251;
                break;
            case "WINDMILL":
                itemId = 6677;
                break;
            case "WALL":
                itemId = 7666;
                break;
            case "TREE3":
                itemId = 5341;
                break;
            case "CoconutTree":
                itemId = 7802;
                break;
            case "Tent":
                itemId = 1604;
                break;
        }

        ItemsCollection.ItemData itemData = Items.GetItem(itemId);
        Vector3 freePosition =
            GroundManager.instance.GetRandomFreePositionForItem(itemData.gridSize, itemData.gridSize);

        BaseItemScript item = SceneManager.instance.AddItem(itemId, false, true);


        if (item != null)
        {
            item.SetPosition(freePosition);
            DataBaseManager.instance.UpdateItemData(item);
            if (this._subCategory.reqMap[DataBaseManager.GOLD_RESOURCE_NAME] > 0)
            {
                DataBaseManager.instance.UpdateResourceData(DataBaseManager.GOLD_RESOURCE_NAME,
                    this._subCategory.reqMap[DataBaseManager.GOLD_RESOURCE_NAME], false);
                SceneManager.instance.ConsumeResource("gold",
                    this._subCategory.reqMap[DataBaseManager.GOLD_RESOURCE_NAME]);
            }

            if (this._subCategory.reqMap[DataBaseManager.WOOD_RESOURCE_NAME] > 0)
            {
                DataBaseManager.instance.UpdateResourceData(DataBaseManager.GOLD_RESOURCE_NAME,
                    this._subCategory.reqMap[DataBaseManager.WOOD_RESOURCE_NAME], false);
            }

            if (this._subCategory.reqMap[DataBaseManager.LEAVES_RESOURCE_NAME] > 0)
            {
                DataBaseManager.instance.UpdateResourceData(DataBaseManager.LEAVES_RESOURCE_NAME,
                    this._subCategory.reqMap[DataBaseManager.LEAVES_RESOURCE_NAME], false);
            }
        }

        SceneManager.instance.UpdateResourcesValues();

        this.GetComponentInParent<ShopWindowScript>().Close();
    }
}