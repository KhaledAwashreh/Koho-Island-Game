using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class SceneManager : MonoBehaviour
{
    public static SceneManager instance;

    /* prefabs */
    public GameObject BaseItem;
    public GameObject LockedGround1;
    public GameObject LockedGround2;
    public GameObject RenderQuad;
    public Material RenderQuadMaterial;

    /* object refs */
    public GameObject Design;
    public GameObject ItemsContainer;
    public GameObject ParticlesContainer;

    public GameObject Grid;

    /* public vars */
    public Common.GameMode gameMode = Common.GameMode.NORMAL;

    /* private vars */
    private Dictionary<int, BaseItemScript> _itemInstances;

    //resource values
    public int numberOfGoldInStorage;
    public int numberOfElixirInStorage;
    public int numberOfDiamondsInStorage;
    public int numberOfWoodsInStorage;
    public int numberOfLeavesInStorage;
    public int numberOfFruitsInStorage;
    public int numberOfFoodInStorage;
    public int currentXpCount;
    public int currentLevel;

    public int goldStorageCapacity;
    public int elixirStorageCapacity;
    public int woodStorageCapacity;
    public int leavsStorageCapacity;
    public int fruitsStorageCapacity;
    public int currentXpCapacity;
    public int foodCapacity;


    private int flag = GetUsername.flag;

    void Awake()
    {
        instance = this;
        this.Design.SetActive(false);

        this._itemInstances = new Dictionary<int, BaseItemScript>();

        /* registering events */
        CameraManager.instance.OnItemTap += this.OnItemTap;
        CameraManager.instance.OnItemDragStart += this.OnItemDragStart;
        CameraManager.instance.OnItemDrag += this.OnItemDrag;
        CameraManager.instance.OnItemDragStop += this.OnItemDragStop;
        CameraManager.instance.OnTapGround += this.OnTapGround;

        GroundManager.instance.UpdateAllNodes();
        this.Init();
    }

    /// <summary>
    /// Init this instance.
    /// </summary>
    public void Init()
    {
        if (flag == 1)
        {
            DataBaseManager.instance.DefaultObjectives();
        }

        this.EnterNormalMode();

        this.numberOfGoldInStorage = DataBaseManager.instance.GetResourceData(DataBaseManager.GOLD_RESOURCE_NAME);
        this.numberOfElixirInStorage = 150;
        this.numberOfDiamondsInStorage = 300;
        this.numberOfWoodsInStorage = DataBaseManager.instance.GetResourceData(DataBaseManager.WOOD_RESOURCE_NAME);
        this.numberOfLeavesInStorage = DataBaseManager.instance.GetResourceData(DataBaseManager.LEAVES_RESOURCE_NAME);
        this.numberOfFruitsInStorage = DataBaseManager.instance.GetResourceData(DataBaseManager.FRUIT_RESOURCE_NAME);
        this.numberOfFoodInStorage = DataBaseManager.instance.GetResourceData(DataBaseManager.FOOD_RESOURCE_NAME);

        // this.currentXpCount = DataBaseManager.instance.GetResourceData(DataBaseManager.XP_RESOURCE_NAME);
        // this.currentLevel = DataBaseManager.instance.GetResourceData(DataBaseManager.LEVEL_RESOURCE_NAME);
        this.currentXpCount = 150;
        this.currentLevel = 2;

        this.woodStorageCapacity = 2500;
        this.goldStorageCapacity = 5000;
        this.elixirStorageCapacity = 5000;
        this.leavsStorageCapacity = 5000;
        this.fruitsStorageCapacity = 5000;
        this.foodCapacity = 5000;
        this.currentXpCapacity = this.currentLevel * 100;

        if (currentLevel >= 3)
        {
            LockedGround1.transform.gameObject.SetActive(false);
            LockedGround2.transform.gameObject.SetActive(false);
            GroundManager.instance.nodeHeight = 44;
            GroundManager.instance.nodeWidth = 44;
        }
    }

    public void UpdateResourcesValues()
    {
        this.numberOfGoldInStorage = DataBaseManager.instance.GetResourceData(DataBaseManager.GOLD_RESOURCE_NAME);
        this.numberOfWoodsInStorage = DataBaseManager.instance.GetResourceData(DataBaseManager.WOOD_RESOURCE_NAME);
        this.numberOfLeavesInStorage = DataBaseManager.instance.GetResourceData(DataBaseManager.LEAVES_RESOURCE_NAME);
        this.numberOfFruitsInStorage = DataBaseManager.instance.GetResourceData(DataBaseManager.FRUIT_RESOURCE_NAME);
        this.numberOfFoodInStorage = DataBaseManager.instance.GetResourceData(DataBaseManager.FOOD_RESOURCE_NAME);
    }


    /// <summary>
    /// Adds the item with itemId. where itemId is the id which we registered with item prefab as unique.
    /// </summary>
    /// <returns>The item.</returns>
    /// <param name="itemId">Item identifier.</param>
    public BaseItemScript AddItem(int itemId, int instanceId, int posX, int posZ, bool immediate, bool ownedItem)
    {
        BaseItemScript builder = null;

        if (!immediate)
        {
            builder = this.GetFreeBuilder();
            if (builder == null)
            {
                Debug.Log("All builders are busy!");
                UIManager.instance.ShowBuildersBusyWindow();
                return null;
            }
        }

        BaseItemScript instance = Utilities.CreateInstance(this.BaseItem, this.ItemsContainer, true)
            .GetComponent<BaseItemScript>();

        if (instanceId == -1)
        {
            instanceId = this._GetUnusedInstanceId();
        }

        instance.instanceId = instanceId;
        this._itemInstances.Add(instanceId, instance);

        instance.SetItemData(itemId, posX, posZ);
        instance.SetState(Common.State.IDLE);

        // GroundManager.Cell freeCell = GroundManager.instance.GetRandomFreeCellForItem(instance);
        // instance.SetPosition(GroundManager.instance.CellToPosition(freeCell));

        if (!immediate)
        {
            instance.UI.ShowProgressUI(true);

            if (!instance.itemData.configuration.isCharacter && instance.itemData.configuration.buildTime > 0)
            {
                builder.BuilderAction(instance);
            }
        }

        if (!instance.itemData.configuration.isCharacter)
        {
            GroundManager.instance.UpdateBaseItemNodes(instance, GroundManager.Action.ADD);
        }

        if (instance.itemData.name == "Wall")
        {
            this.UpdateWalls();
        }

        instance.ownedItem = ownedItem;
        return instance;
    }

    public BaseItemScript AddItem(int itemId, bool immediate, bool ownedItem)
    {
        int posX = 0;
        int posZ = 0;
        if (!immediate)
        {
            ItemsCollection.ItemData itemData = Items.GetItem(itemId);
            Vector3 freePosition =
                GroundManager.instance.GetRandomFreePositionForItem(itemData.gridSize, itemData.gridSize);
            posX = (int) freePosition.x;
            posZ = (int) freePosition.z;
        }

        return this.AddItem(itemId, -1, posX, posZ, immediate, ownedItem);
    }

    /// <summary>
    /// Removes the item.
    /// </summary>
    /// <param name="item">Item.</param>
    public void RemoveItem(BaseItemScript item)
    {
        this._itemInstances.Remove(item.instanceId);
        if (item != null)
        {
            Destroy(item.gameObject);
        }
    }

    private int _GetUnusedInstanceId()
    {
        int instanceId = Random.Range(10000, 99999);
        if (this._itemInstances.ContainsKey(instanceId))
        {
            return _GetUnusedInstanceId();
        }

        return instanceId;
    }


    private BaseItemScript _dragItem;

    /// <summary>
    /// Raises the item drag start event.
    /// </summary>
    /// <param name="evt">Evt.</param>
    public void OnItemDragStart(CameraManager.CameraEvent evt)
    {
        if (this.gameMode == Common.GameMode.ATTACK)
        {
            return;
        }

        this._dragItem = evt.baseItem;
        this._dragItem.OnItemDragStart(evt);

        if (this._dragItem.itemData.name == "Wall")
        {
            this.UpdateWalls();
        }
    }

    /// <summary>
    /// Raises the item drag event.
    /// </summary>
    /// <param name="evt">Evt.</param>
    public void OnItemDrag(CameraManager.CameraEvent evt)
    {
        if (this.gameMode == Common.GameMode.ATTACK)
        {
            return;
        }

        if (this._dragItem != null)
        {
            this._dragItem.OnItemDrag(evt);
            //			this.ShowGrid ();
        }
    }

    /// <summary>
    /// Raises the item drag stop event.
    /// </summary>
    /// <param name="evt">Evt.</param>
    public void OnItemDragStop(CameraManager.CameraEvent evt)
    {
        if (this.gameMode == Common.GameMode.ATTACK)
        {
            return;
        }


        if (this._dragItem != null)
        {
            this._dragItem.OnItemDragStop(evt);

            if (this._dragItem.itemData.name == "Wall")
            {
                this.UpdateWalls();
            }

            this._dragItem = null;
        }
    }

    public BaseItemScript selectedItem;

    /// <summary>
    /// Raises the item tap event.
    /// </summary>
    /// <param name="evt">Evt.</param>
    public void OnItemTap(CameraManager.CameraEvent evt)
    {
        //		Debug.Log ("OnItemTap");
        if (this.gameMode == Common.GameMode.ATTACK)
        {
            return;
        }

        BaseItemScript tappedItem = evt.baseItem;
        if (tappedItem.Production.readyForCollection)
        {
            tappedItem.Production.Collect();
            return;
        }

        if (this.selectedItem != null)
        {
            this.selectedItem.SetSelected(false);
        }

        this.selectedItem = tappedItem;
        tappedItem.SetSelected(true);
    }


    private BaseItemScript _unit;
    public int selectedUnit = 0;
    private int _swordManCount = 10;
    private int _archerCount = 10;

    private int _swordManExpended = 0;
    private int _archerExpended = 0;

    /// <summary>
    /// Raises the tap ground event.
    /// </summary>
    /// <param name="evt">Evt.</param>
    public void OnTapGround(CameraManager.CameraEvent evt)
    {
        //		Debug.Log ("OnTapGround");

        if (this.gameMode == Common.GameMode.NORMAL)
        {
            if (this.selectedItem != null)
            {
                BaseItemScript temp = this.selectedItem;
                this.selectedItem = null;
                temp.SetSelected(false);
            }

            //			if (this._unit == null) {
            //				this._unit = this.AddItem (5492, true);
            //				this._unit.SetState (Common.State.WALK);
            //				this._unit.SetPosition (evt.point);
            //			} else {
            //				this._unit.LookAt (evt.point);
            //			}
        }

        if (this.gameMode == Common.GameMode.ATTACK)
        {
            if (selectedUnit == 0)
            {
                if (this._swordManExpended == _swordManCount)
                {
                    return;
                }

                this._swordManExpended++;

                AttackOverlayWindowScript.instance.SwordManCounter.text =
                    (this._swordManCount - _swordManExpended).ToString() + "x";
            }
            else if (selectedUnit == 1)
            {
                if (this._archerExpended == _archerCount)
                {
                    return;
                }

                this._archerExpended++;

                AttackOverlayWindowScript.instance.ArcherCounter.text =
                    (this._archerCount - _archerExpended).ToString() + "x";
            }

            int[] unitIds = new int[] {_swordMan_ID, _archer_ID};

            evt.point.x = Mathf.Clamp(evt.point.x, 0, GroundManager.instance.nodeWidth - 1);
            evt.point.z = Mathf.Clamp(evt.point.z, 0, GroundManager.instance.nodeHeight - 1);

            this._unit = this.AddItem(unitIds[selectedUnit], true, true);
            this._unit.SetPosition(evt.point);
            this._unit.Attacker.AttackNearestTarget();
            this._unit.OnItemDestroy += this.OnUnitDied;

            SoundManager.instance.PlaySound(SoundManager.instance.Yeah, false);
        }

        //		if (testCharacter != null) {
        //			testCharacter.LookAt (_builderHutInstance);
        //		}

        //		if (this._builderInstance != null) {
        //			this._builderInstance.gameObject.SetActive (true);
        //			this._builderInstance.Walker.WalkToPosition (evt.point);
        //		}

        //		if (this._swordManInstance != null) {
        //			this._swordManInstance.gameObject.SetActive (true);
        //			this._swordManInstance.Walker.WalkToPosition (evt.point);
        //		}
    }

    public Dictionary<int, BaseItemScript> GetItemInstances()
    {
        return this._itemInstances;
    }

    /// <summary>
    /// Shows the grid.
    /// </summary>
    private IEnumerator _ShowGridEnumerator;

    public void ShowGrid()
    {
        if (this._ShowGridEnumerator != null)
        {
            this.StopCoroutine(_ShowGridEnumerator);
            this._ShowGridEnumerator = null;
        }

        this._ShowGridEnumerator = this._ShowGrid();
        this.StartCoroutine(this._ShowGridEnumerator);
    }

    private IEnumerator _ShowGrid()
    {
        this.Grid.SetActive(true);
        yield return new WaitForSeconds(3);
        this.Grid.SetActive(false);
        this._ShowGridEnumerator = null;
    }


    //private int _builderHut_ID = 3635;
    //private int _townCenter_ID = 2496;
    //private int _builder_ID = 3823;

    private int _swordMan_ID = 6704;
    private int _archer_ID = 5492;

    //private int _barrackID = 8833;
    //private int _elixirCollectorID = 4856;
    //private int _elixirStorageID = 2090;
    //private int _goldMineID = 3265;
    //private int _goldStorageID = 9074;
    //private int _towerID = 4764;
    //private int _townCenterID = 2496;
    //private int _windMillID = 6677;
    //private int _armyCampID = 2728;

    //private BaseItemScript _townCenterInstance;
    //private BaseItemScript _builderHutInstance;
    //private BaseItemScript _builderInstance;
    //private BaseItemScript _towerInstance;
    //private BaseItemScript _armyCampForSwordManInstance;
    //private BaseItemScript _armyCampForArcherInstance;

    //public BaseItemScript testCharacter;
    //private BaseItemScript[] _swordManInstances;
    //private BaseItemScript[] _archerInstances;

    public void LoadUserScene()
    {
        this.ClearScene();
        SceneData sceneData = DataBaseManager.instance.GetScene();

        foreach (ItemData itemData in sceneData.items)
        {
            this.AddItem(itemData.itemId, itemData.instanceId, itemData.posX, itemData.posZ, true, true);
        }

        //LOAD UNITS ON CAMP
        BaseItemScript[] armyCamps = GetArmyCamps();
        if (armyCamps.Length > 0)
        {
            for (int index = 0; index < _swordManCount; index++)
            {
                var camp = armyCamps[Random.Range(0, armyCamps.Length)];
                BaseItemScript unit = this.AddItem(_swordMan_ID, -1, camp.GetPositionX(), camp.GetPositionZ(), true,
                    true);
                unit.WalkRandom(camp);
            }

            for (int index = 0; index < _archerCount; index++)
            {
                var camp = armyCamps[Random.Range(0, armyCamps.Length)];
                BaseItemScript unit = this.AddItem(_archer_ID, -1, camp.GetPositionX(), camp.GetPositionZ(), true,
                    true);
                unit.WalkRandom(camp);
            }
        }

        //for (int index = 0; index < 25; index++)
        //{
        //	//tree
        //	BaseItemScript tree = this.AddItem(5341, true, true);
        //	tree.SetPosition(GroundManager.instance.GetRandomFreePosition());
        //}

        GroundManager.instance.UpdateAllNodes();
        this.UpdateWalls();

        UIManager.instance.ShowGameOverlayWindow();
    }

    public BaseItemScript[] GetArmyCamps()
    {
        List<BaseItemScript> armyCamps = new List<BaseItemScript>();
        foreach (KeyValuePair<int, BaseItemScript> entry in _itemInstances)
        {
            if (entry.Value.itemData.name == "ArmyCamp")
                armyCamps.Add(entry.Value);
        }

        return armyCamps.ToArray();
    }


    public void ClearScene()
    {
        foreach (KeyValuePair<int, BaseItemScript> entry in _itemInstances)
        {
            Destroy(entry.Value.gameObject);
        }

        this._itemInstances = new Dictionary<int, BaseItemScript>();
    }


    public void EnterNormalMode()
    {
        UIManager.instance.CloseAllWindows();
        UIManager.instance.ShowSceneEnteringWindow(() =>
        {
            this.gameMode = Common.GameMode.NORMAL;
            LoadUserScene();
        });

        SoundManager.instance.StopAllSounds();
        SoundManager.instance.PlaySound(SoundManager.instance.BGM, true);
    }

    public List<BaseItemScript> GetAllItems()
    {
        List<BaseItemScript> items = new List<BaseItemScript>();
        foreach (KeyValuePair<int, BaseItemScript> entry in this._itemInstances)
        {
            items.Add(entry.Value);
        }

        return items;
    }

    public void UpdateWalls()
    {
        foreach (KeyValuePair<int, BaseItemScript> entry in _itemInstances)
        {
            BaseItemScript item = entry.Value;
            if (item.itemData.name == "Wall")
            {
                item.UpdateWall();
            }
        }
    }

    public BaseItemScript GetFreeBuilder()
    {
        BaseItemScript builder = null;
        List<BaseItemScript> builderHuts = new List<BaseItemScript>();
        foreach (KeyValuePair<int, BaseItemScript> entry in _itemInstances)
        {
            if (entry.Value.itemData.name == "BuilderHut")
            {
                builderHuts.Add(entry.Value);
            }
        }

        foreach (BaseItemScript hut in builderHuts)
        {
            if (hut.connectedItems[0].buildingItem == null)
            {
                builder = hut.connectedItems[0];
                break;
            }
        }

        return builder;
    }

    public void OnEnemyItemDestroy(BaseItemScript item)
    {
        bool isEverythingDestroyed = true;
        foreach (KeyValuePair<int, BaseItemScript> entry in this._itemInstances)
        {
            BaseItemScript baseItem = entry.Value;
            if (!baseItem.itemData.configuration.isCharacter && baseItem.itemData.name != "Wall")
            {
                //item is not character and not a wall
                //check the item is destroyed or not, if not then everything in the city is not destroyed
                if (!baseItem.isDestroyed)
                    isEverythingDestroyed = false;
            }
        }

        if (isEverythingDestroyed)
        {
            //war ends
            AttackOverlayWindowScript.instance.Close();
            UIManager.instance.ShowResultWindow(true, _swordManExpended, _archerExpended);
        }
    }

    private int _diedUnitCount = 0;

    public void OnUnitDied(BaseItemScript unit)
    {
        _diedUnitCount++;
        if (_diedUnitCount == (_swordManCount + _archerCount))
        {
            //war ends
            AttackOverlayWindowScript.instance.Close();
            UIManager.instance.ShowResultWindow(false, _swordManExpended, _archerExpended);
        }
    }

    //RESOURCE  COLLECTION
    public void CollectResource(string resourceType, int amount)
    {
        if (resourceType == "gold")
        {
            this.numberOfGoldInStorage = Mathf.Clamp(this.numberOfGoldInStorage + amount, 0, goldStorageCapacity);
            DataBaseManager.instance.UpdateResourceData(DataBaseManager.GOLD_RESOURCE_NAME,
                amount, true);
        }
        else if (resourceType == "elixir")
        {
            this.numberOfElixirInStorage = Mathf.Clamp(this.numberOfElixirInStorage + amount, 0, elixirStorageCapacity);
        }
        else if (resourceType == "diamond")
        {
            this.numberOfDiamondsInStorage = this.numberOfDiamondsInStorage + amount;
        }

        this.RefreshResourceUIs(resourceType);
    }

    //RESOURCE COLLECTION
    public bool ConsumeResource(string resourceType, int count)
    {
        if (resourceType == "gold")
        {
            if (this.numberOfGoldInStorage >= count)
            {
                this.numberOfGoldInStorage -= count;
                this.RefreshResourceUIs(resourceType);
                return true;
            }
        }
        else if (resourceType == "elixir")
        {
            if (this.numberOfElixirInStorage >= count)
            {
                this.numberOfElixirInStorage -= count;
                this.RefreshResourceUIs(resourceType);
                return true;
            }
        }
        else if (resourceType == "diamond")
        {
            if (this.numberOfDiamondsInStorage >= count)
            {
                this.numberOfDiamondsInStorage -= count;
                this.RefreshResourceUIs(resourceType);
                return true;
            }
        }

        return false;
    }

    public void RefreshResourceUIs(string resourceType)
    {
        if (GameOverlayWindowScript.instance != null)
        {
            if (resourceType == "gold")
                GameOverlayWindowScript.instance.CollectResource("gold", this.numberOfGoldInStorage);
            else if (resourceType == "elixir")
                GameOverlayWindowScript.instance.CollectResource("elixir", this.numberOfElixirInStorage);
            else if (resourceType == "diamond")
                GameOverlayWindowScript.instance.CollectResource("diamond", this.numberOfDiamondsInStorage);
        }

        if (TrainTroopsWindowScript.instance != null)
        {
            TrainTroopsWindowScript.instance.UpdateResourcePanel();
        }
    }

    //PARTICLES
    public GameObject ShowParticle(GameObject prefab, Vector3 position)
    {
        GameObject inst = Utilities.CreateInstance(prefab, this.ParticlesContainer, true);
        inst.transform.position = position;
        return inst;
    }

    public BaseItemScript GetNearestArmyCamp(Vector3 from)
    {
        BaseItemScript[] armyCamps = this.GetArmyCamps();

        if (armyCamps.Length == 0)
            return null;

        if (armyCamps.Length == 1)
            return armyCamps[0];

        float smallDistance = 999999;
        BaseItemScript nearestArmyCamp = null;
        foreach (BaseItemScript armyCamp in armyCamps)
        {
            float dist = Vector3.Distance(armyCamp.GetPosition(), from);
            if (dist < smallDistance)
            {
                smallDistance = dist;
                nearestArmyCamp = armyCamp;
            }
        }

        return nearestArmyCamp;
    }
}