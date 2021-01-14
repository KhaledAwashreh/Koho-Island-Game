using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UIElements;

[System.Serializable]
public class UserData
{
    public int id;
    public string name;
}


[System.Serializable]
public class ItemData
{
    public int instanceId;
    public int itemId;
    public int posX;
    public int posZ;
}

[System.Serializable]
public class ResourceData
{
    public string resourceName;
    public int resourceAmount;
    public int productionRate;
    public string resourceType;
    public int energyCollectionCost;
}

[System.Serializable]
public class ConsequencesData
{
    public double air;
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
}

[System.Serializable]
public class Score
{
    public int score;
    public string name;
}

[System.Serializable]
public class ObjectiveData
{
    public int region;
    public bool[] completed;

    public ObjectiveData(int region, bool[] completed)
    {
        this.region = region;
        this.completed = completed;
    }
}

[System.Serializable]
public class SceneData
{
    public List<ItemData> items;
    public List<ConsequencesData> ConsequencesDatas;
    public List<ResourceData> resources;
    public List<ObjectiveData> objectives;
    public List<Score> Leaderboard;
    public UserData user;

    public SceneData()
    {
        items = new List<ItemData>();
        resources = new List<ResourceData>();
        objectives = new List<ObjectiveData>();
        ConsequencesDatas = new List<ConsequencesData>();
        Leaderboard = new List<Score>();
    }

    public void updateConsueqncesValues(double air, double Co2Level, double O2Level, double soilFertility,
        double flowersAndGrass, double otherGasses, double totalPollution, double airPollution, double waterPollution,
        double landPollution)
    {
        ConsequencesData temp = ConsequencesDatas[0];
        if (ConsequencesDatas.Count > 0)
        {
            ConsequencesDatas[0].airPollution = airPollution;
            ConsequencesDatas[0].waterPollution = waterPollution;
            ConsequencesDatas[0].landPollution = landPollution;
            ConsequencesDatas[0].totalPollution = totalPollution;
            ConsequencesDatas[0].Co2Level = Co2Level;
            ConsequencesDatas[0].O2Level = O2Level;
            ConsequencesDatas[0].soilFertility = soilFertility;
            ConsequencesDatas[0].air = air;
            ConsequencesDatas[0].otherGasses = otherGasses;
            ConsequencesDatas[0].flowersAndGrass = flowersAndGrass;
        }


        if (temp == null)
        {
            temp = new ConsequencesData();
            temp.airPollution = airPollution;
            temp.waterPollution = waterPollution;
            temp.landPollution = landPollution;
            temp.totalPollution = totalPollution;
            temp.Co2Level = Co2Level;
            temp.O2Level = O2Level;
            temp.soilFertility = soilFertility;
            temp.air = air;
            temp.otherGasses = otherGasses;
            temp.flowersAndGrass = flowersAndGrass;
            ConsequencesDatas.Add(temp);

            user = new UserData();
        }
    }

    public void updateLeaderboardData()
    {
    }

    public void AddOrUpdateResourse(string resourceName, int resourceAmount, bool addORemove)
    {
        ResourceData resData = null;
        foreach (ResourceData res in this.resources)
        {
            if (res.resourceName == resourceName)
            {
                resData = res;
            }
        }

        if (resData == null)
        {
            resData = new ResourceData();
            resData.resourceName = resourceName;
            resData.resourceAmount = resourceAmount;
            this.resources.Add(resData);
        }

        if (resData != null)
        {
            if (addORemove)
            {
                resData.resourceAmount += resourceAmount;
            }
            else
            {
                resData.resourceAmount -= resourceAmount;
            }
        }
    }

    public void AddUser()
    {
        string name = GetUsername.userDataName;
        int id = 0;

        user.id = 0;
        user.name = name;
    }


    public void AddOrUpdateObjective(int region, bool[] completed)
    {
        ObjectiveData objData = null;
        foreach (ObjectiveData res in this.objectives)
        {
            if (res.region == region)
            {
                objData = res;
            }
        }

        if (objData == null)
        {
            objData = new ObjectiveData(region, completed);
            this.objectives.Add(objData);
        }
    }

    public void AddOrUpdateItem(int instanceId, int itemId, int posX, int posZ)
    {
        ItemData itemData = null;
        foreach (ItemData item in this.items)
        {
            if (item.instanceId == instanceId)
            {
                itemData = item;
            }
        }

        if (itemData == null)
        {
            itemData = new ItemData();
            itemData.instanceId = instanceId;
            itemData.itemId = itemId;
            this.items.Add(itemData);
        }

        itemData.posX = posX;
        itemData.posZ = posZ;
    }

    public void RemoveItem(int instanceId)
    {
        ItemData targetItem = this.GetItem(instanceId);

        if (targetItem != null)
        {
            this.items.Remove(targetItem);
        }
    }

    public void RemoveResource(string resourceName)
    {
        ResourceData targetRes = this.GetResource(resourceName);

        if (targetRes != null)
        {
            this.resources.Remove(targetRes);
        }
    }

 

    public ConsequencesData GetConsequencesInfo()
    {
        ConsequencesData temp = null;
        if (ConsequencesDatas.Count > 0)
        {
            temp = ConsequencesDatas[0];
        }

        if (temp == null)
        {
            temp = new ConsequencesData();
            temp.airPollution = 0;
            temp.waterPollution = 0;
            temp.landPollution = 0;
            temp.totalPollution = 0;
            temp.Co2Level = 0.045;
            temp.O2Level = 0.195;
            temp.soilFertility = 10;
            temp.air = 1;
            temp.otherGasses = temp.air - temp.Co2Level - temp.O2Level;
            temp.flowersAndGrass = 5;

            ConsequencesDatas.Add(temp);
        }

        return temp;
    }


    public ResourceData GetResource(string resourceName)
    {
        ResourceData targetRes = null;
        foreach (ResourceData resData in this.resources)
        {
            if (resData.resourceName == resourceName)
            {
                targetRes = resData;
            }
        }

        if (targetRes == null)
        {
            targetRes = new ResourceData();
            targetRes.resourceName = resourceName;
            targetRes.resourceAmount = 0;
            this.resources.Add(targetRes);
        }

        return targetRes;
    }

    public UserData getUserInfo()
    {
        return this.user;
    }
    

    public ItemData GetItem(int instanceId)
    {
        ItemData targetItem = null;
        foreach (ItemData itemData in this.items)
        {
            if (itemData.instanceId == instanceId)
            {
                targetItem = itemData;
            }
        }

        return targetItem;
    }


    public ObjectiveData GetObjective(int region)
    {
        ObjectiveData targetObj = null;
        foreach (ObjectiveData temp in this.objectives)
        {
            if (temp.region == region)
            {
                targetObj = temp;
            }
        }

        if (targetObj == null)
        {
            bool[] complete = {false, false, false};
            objectives.Add(new ObjectiveData(region, complete));
        }

        return targetObj;
    }

    public void makeObjectivesDefault()
    {
        foreach (ObjectiveData temp in this.objectives)
        {
            bool[] defaultCompleted = {false, false, false};
            temp.completed = defaultCompleted;
        }

        foreach (ResourceData res in this.resources)
        {
            if (res.resourceName == "GOLD_RESOURCE_NAME")
            {
                res.resourceAmount = 1660; 
            }
            else if (res.resourceName == "WOOD_RESOURCE_NAME")
            {
                res.resourceAmount = 880;
            }

            res.productionRate = 0;
            res.energyCollectionCost = 0;
        }

        Debug.Log(resources[0].resourceAmount);
    }

    public void CreateObjectives()
    {
        //ObjectiveData 
    }
}

[System.Serializable]
public class GameData
{
    public SceneData sceneData;
}

public class DataBaseManager : MonoBehaviour
{
    public static DataBaseManager instance;
    public const string GOLD_RESOURCE_NAME = "GOLD_RESOURCE_NAME";
    public const string WOOD_RESOURCE_NAME = "WOOD_RESOURCE_NAME";
    public const string XP_RESOURCE_NAME = "XP_RESOURCE_NAME";
    public const string LEVEL_RESOURCE_NAME = "LEVEL_RESOURCE_NAME";
    public const string LEAVES_RESOURCE_NAME = "LEAVES_RESOURCE_NAME";
    public const string FRUIT_RESOURCE_NAME = "FRUIT_RESOURCE_NAME";
    public const string FOOD_RESOURCE_NAME = "FOOD_RESOURCE_NAME";
    private string gameDataFilePath = "/StreamingAssets/db.json";
    public GameData _gameData;

    void Awake()
    {
        instance = this;
        this.EnsureGameDataFileExists();
    }

    public void EnsureGameDataFileExists()
    {
        this._gameData = new GameData();
        this._gameData.sceneData = new SceneData();

        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            return;
        }

        string filePath = Application.persistentDataPath + gameDataFilePath;
        string directoryPath = Application.persistentDataPath + "/StreamingAssets";

        if (Application.platform == RuntimePlatform.OSXEditor ||
            Application.platform == RuntimePlatform.WindowsEditor ||
            Application.platform == RuntimePlatform.LinuxEditor)
        {
            filePath = Application.dataPath + gameDataFilePath;
            directoryPath = Application.dataPath + "/StreamingAssets";
        }

        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        if (File.Exists(filePath))
        {
            string jsonData = File.ReadAllText(filePath);
            this._gameData = JsonUtility.FromJson<GameData>(jsonData);
        }
        else
        {
            this.SaveDataBase();
        }
    }

    public SceneData GetScene()
    {
        if (this._gameData.sceneData.items.Count == 0)
        {
            // this._gameData.sceneData = JsonUtility.FromJson<SceneData>(this._defaultSceneData);
            // this.SaveDataBase();
        }

        return this._gameData.sceneData;
    }


    public void SaveScene()
    {
        foreach (BaseItemScript item in SceneManager.instance.GetAllItems())
        {
            this._gameData.sceneData.AddOrUpdateItem(item.instanceId, item.itemData.id, item.GetPositionX(),
                item.GetPositionZ());
        }

        this.SaveDataBase();
    }

    public void UpdateItemData(BaseItemScript item)
    {
        this._gameData.sceneData.AddOrUpdateItem(item.instanceId, item.itemData.id, item.GetPositionX(),
            item.GetPositionZ());
        this.SaveDataBase();
    }

    public void UpdateResourceData(string resourceName, int resourceAmount, bool addORemove)
    {
        this._gameData.sceneData.AddOrUpdateResourse(resourceName, resourceAmount, addORemove);
        this.SaveDataBase();
    }


    public void updateConsequencesData(double air, double Co2Level, double O2Level, double soilFertility,
        double flowersAndGrass, double otherGasses, double totalPollution, double airPollution, double waterPollution,
        double landPollution)
    {
        this._gameData.sceneData.updateConsueqncesValues(air, Co2Level, O2Level, soilFertility,
            flowersAndGrass, otherGasses, totalPollution, airPollution, waterPollution,
            landPollution);

        this.SaveDataBase();
    }

    public void updateLeaderboardData()
    {
        this.SaveDataBase();
    }

    public int GetResourceData(string resourceName)
    {
        return this._gameData.sceneData.GetResource(resourceName).resourceAmount;
    }
    

    public void UpdateObjectiveData(int region, bool[] completed)
    {
        this._gameData.sceneData.AddOrUpdateObjective(region, completed);
        this.SaveDataBase();
    }

    public ConsequencesData getConsequencesData()
    {
        return this._gameData.sceneData.GetConsequencesInfo();
    }

    public UserData getUserData()
    {
        return this._gameData.sceneData.getUserInfo();
    }


    public ObjectiveData GetObjective(int region)
    {
        return this._gameData.sceneData.GetObjective(region);
    }

    public void RemoveItem(BaseItemScript item)
    {
        this._gameData.sceneData.RemoveItem(item.instanceId);
        this.SaveDataBase();
    }

    public void DefaultObjectives()
    {
        this._gameData.sceneData.makeObjectivesDefault();
        this.SaveDataBase();
    }

    public void addUser()
    {
        this._gameData.sceneData.AddUser();
        this.SaveDataBase();
    }


    public void SaveDataBase()
    {
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            return;
        }

        string filePath = Application.persistentDataPath + gameDataFilePath;

        if (Application.platform == RuntimePlatform.OSXEditor ||
            Application.platform == RuntimePlatform.WindowsEditor ||
            Application.platform == RuntimePlatform.LinuxEditor)
        {
            filePath = Application.dataPath + gameDataFilePath;
        }

        string jsonData = JsonUtility.ToJson(this._gameData);
        File.WriteAllText(filePath, jsonData);
    }
}