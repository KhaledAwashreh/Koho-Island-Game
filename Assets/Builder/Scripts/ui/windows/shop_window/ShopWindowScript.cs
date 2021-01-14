using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopWindowScript : WindowScript
{
    /* prefabs */
    public GameObject CategoryItem;
    public GameObject SubCategoryItem;

    /* references */
    public ScrollRect ScrollView;
    public GameObject ItemsList;
    public GameObject BackButton;

    public enum Category
    {
        ARMY,
        DEFENCE,
        RESOURCES,
        OTHER,
        TREASURE,
        DECORATIONS
    }


    public class SubCategory
    {
        public string subCategoryName;
        public bool Locked;
        public int unlockingLevel;
        public Dictionary<string, int> reqMap = new Dictionary<string, int>();

        public SubCategory(string subCategoryName, int unlockingLevel)
        {
            this.subCategoryName = subCategoryName;
            this.unlockingLevel = unlockingLevel;
            if (SceneManager.instance.currentLevel < this.unlockingLevel)
            {
                this.Locked = false;
            }
            else
            {
                this.Locked = true;
            }

            reqMap.Add(DataBaseManager.GOLD_RESOURCE_NAME, 0);
            reqMap.Add(DataBaseManager.WOOD_RESOURCE_NAME, 0);
            reqMap.Add(DataBaseManager.LEAVES_RESOURCE_NAME, 0);
        }
    }

    void Awake()
    {
        this.Init();
    }

    public void Init()
    {
        this.RenderCategories();
    }

    public void RenderCategories()
    {
        Vector2 tempSpacing = new Vector2(10, 26);
        this.ItemsList.GetComponent<GridLayoutGroup>().spacing = tempSpacing;
        this.BackButton.SetActive(false);
        this.ClearItemsList();

        Category[] categories = new Category[]
        {
            // Category.ARMY,
            Category.DECORATIONS,
            // Category.DEFENCE,
            Category.OTHER,
            Category.RESOURCES,
            Category.TREASURE
        };

        for (int index = 0; index < categories.Length; index++)
        {
            GameObject inst = Utilities.CreateInstance(this.CategoryItem, this.ItemsList, true);
            inst.GetComponent<CategoryItemScript>().SetCategory(categories[index]);
        }

        RectTransform rt = this.ItemsList.GetComponent<RectTransform>();
        Vector2 sizeDelta = this.ItemsList.GetComponent<RectTransform>().sizeDelta;
        sizeDelta.x = categories.Length * 250 +
                      categories.Length * this.ItemsList.GetComponent<GridLayoutGroup>().spacing.x;
        rt.sizeDelta = sizeDelta;

        this.ResetScrollPosition();
    }

    public void RenderSubCategories(Category category)
    {
        Vector2 tempSpacing = new Vector2(95, 26);
        this.ItemsList.GetComponent<GridLayoutGroup>().spacing = tempSpacing;

        this.BackButton.SetActive(true);

        this.ClearItemsList();

        List<SubCategory> subItems = new List<SubCategory>();
        SubCategory tempItem;
        switch (category)
        {
            // case Category.ARMY:
            //     subItems = new SubCategory[] {SubCategory.BARRACK, SubCategory.CAMP, SubCategory.BOAT};
            //     break;
            case Category.DECORATIONS:
                tempItem = new SubCategory("TREE1", 3);
                tempItem.reqMap[DataBaseManager.GOLD_RESOURCE_NAME] = 5;
                subItems.Add(tempItem);
                tempItem = new SubCategory("TREE2", 2);
                tempItem.reqMap[DataBaseManager.GOLD_RESOURCE_NAME] = 5;
                subItems.Add(tempItem);
                tempItem = new SubCategory("TREE3", 3);
                tempItem.reqMap[DataBaseManager.GOLD_RESOURCE_NAME] = 5;
                subItems.Add(tempItem);
                tempItem = new SubCategory("CoconutTree", 3);
                tempItem.reqMap[DataBaseManager.GOLD_RESOURCE_NAME] = 5;
                subItems.Add(tempItem);
                break;
            // case Category.DEFENCE:
            //     subItems = new SubCategory[] {SubCategory.CANNON, SubCategory.TOWER};
            //     break;
            case Category.OTHER:
                tempItem = new SubCategory("TOWN_CENTER", 3);
                tempItem.reqMap[DataBaseManager.GOLD_RESOURCE_NAME] = 40;
                tempItem.reqMap[DataBaseManager.WOOD_RESOURCE_NAME] = 25;
                subItems.Add(tempItem);


                tempItem = new SubCategory("BUILDER_HUT", 2);
                tempItem.reqMap[DataBaseManager.GOLD_RESOURCE_NAME] = 25;
                tempItem.reqMap[DataBaseManager.WOOD_RESOURCE_NAME] = 15;
                subItems.Add(tempItem);

                tempItem = new SubCategory("WALL", 3);
                tempItem.reqMap[DataBaseManager.GOLD_RESOURCE_NAME] = 5;
                subItems.Add(tempItem);

                tempItem = new SubCategory("Tent", 3);
                tempItem.reqMap[DataBaseManager.GOLD_RESOURCE_NAME] = 5;
                tempItem.reqMap[DataBaseManager.WOOD_RESOURCE_NAME] = 10;
                tempItem.reqMap[DataBaseManager.LEAVES_RESOURCE_NAME] = 5;
                subItems.Add(tempItem);
                break;
            case Category.RESOURCES:

                tempItem = new SubCategory("ELIXIR_COLLECTOR", 3);
                tempItem.reqMap[DataBaseManager.GOLD_RESOURCE_NAME] = 15;
                subItems.Add(tempItem);

                tempItem = new SubCategory("ELIXIR_STORAGE", 3);
                tempItem.reqMap[DataBaseManager.GOLD_RESOURCE_NAME] = 15;
                subItems.Add(tempItem);

                tempItem = new SubCategory("GOLD_MINE", 3);
                tempItem.reqMap[DataBaseManager.GOLD_RESOURCE_NAME] = 15;
                subItems.Add(tempItem);

                tempItem = new SubCategory("GOLD_STORAGE", 2);
                tempItem.reqMap[DataBaseManager.GOLD_RESOURCE_NAME] = 15;
                subItems.Add(tempItem);

                tempItem = new SubCategory("WINDMILL", 3);
                tempItem.reqMap[DataBaseManager.GOLD_RESOURCE_NAME] = 15;
                subItems.Add(tempItem);

                break;
            case Category.TREASURE:
                tempItem = new SubCategory("GEMS", 3);
                tempItem.reqMap[DataBaseManager.GOLD_RESOURCE_NAME] = 15;
                tempItem.reqMap[DataBaseManager.WOOD_RESOURCE_NAME] = 10;
                subItems.Add(tempItem);
                break;
        }

        for (int index = 0; index < subItems.Count; index++)
        {
            GameObject inst = Utilities.CreateInstance(this.SubCategoryItem, this.ItemsList, true);
            inst.GetComponent<SubCategoryItemScript>().SetSubCategory(subItems[index]);
        }

        RectTransform rt = this.ItemsList.GetComponent<RectTransform>();
        Vector2 sizeDelta = this.ItemsList.GetComponent<RectTransform>().sizeDelta;
        sizeDelta.x = subItems.Count * 250 +
                      subItems.Count * this.ItemsList.GetComponent<GridLayoutGroup>().spacing.x;
        rt.sizeDelta = sizeDelta;

        this.ResetScrollPosition();
    }

    public void OnClickCategory(Category category)
    {
        this.RenderSubCategories(category);
    }

    public void ClearItemsList()
    {
        foreach (Transform child in this.ItemsList.transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void OnClickBackButton()
    {
        this.RenderCategories();
    }

    public void ResetScrollPosition()
    {
        this.ScrollView.horizontalNormalizedPosition = 0.0f;
    }
}