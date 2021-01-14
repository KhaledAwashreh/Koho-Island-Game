using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BaseItemScript : MonoBehaviour
{
    /* object refs */
    public GameObject Container;
    public Collider BoxCollider;
    public BaseItemRendererScript Renderer;
    public GameObject UIRoot;
    public GameObject ParticlesRoot;

    public WalkerScript Walker;
    public AttackerScript Attacker;
    public DefenderScript Defender;
    public ProductionScript Production;
    public TrainingScript Training;
    public UIScript UI;
    public ParticlesScript Particles;

    /* public vars */
    public int instanceId;
    public Common.State state;
    public Common.Direction direction;

    public ItemsCollection.ItemData itemData;
    public float healthPoints = 0;

    public bool isDestroyed = false;
    public bool ownedItem;

    public List<BaseItemScript> connectedItems;

    /* events */
    public UnityAction<BaseItemScript> OnItemDestroy;

    /* private vars */
    private Dictionary<float, Common.Direction> _angleToDirectionMap;


    void Awake()
    {
        this._angleToDirectionMap = new Dictionary<float, Common.Direction>();
        this._angleToDirectionMap.Add(0, Common.Direction.BOTTOM_RIGHT);
        this._angleToDirectionMap.Add(51, Common.Direction.BOTTOM);
        this._angleToDirectionMap.Add(110, Common.Direction.BOTTOM_LEFT);
        this._angleToDirectionMap.Add(153, Common.Direction.LEFT);
        this._angleToDirectionMap.Add(190, Common.Direction.TOP_LEFT);
        this._angleToDirectionMap.Add(220, Common.Direction.TOP);
        this._angleToDirectionMap.Add(290, Common.Direction.TOP_RIGHT);
        this._angleToDirectionMap.Add(357, Common.Direction.RIGHT);
    }

    private void OnDestroy()
    {
        if (connectedItems != null)
        {
            for (int index = 0; index < connectedItems.Count; index++)
            {
                SceneManager.instance.RemoveItem(connectedItems[index]);
            }
        }
    }

    public void SetItemData(int itemId, int posX, int posZ)
    {
        this.itemData = Items.GetItem(itemId);
        this.gameObject.name = itemData.name + " [INSTANCE]";
        this.SetSize(Vector3.one * itemData.gridSize);

        this.healthPoints = this.itemData.configuration.healthPoints;

        this.Renderer.Init();
        this.Walker.SetData(this);
        this.Attacker.SetData(this);
        this.Training.SetData(this);
        this.UI.SetData(this);
        this.Particles.SetData(this);

        if (this.itemData.configuration.productionRate > 0)
            this.Production.SetData(this);

        this.connectedItems = new List<BaseItemScript>();

        if (this.itemData.configuration.defenceRange > 0)
        {
            this.Defender.SetData(this);
        }

        //disable box collider for characters, otherwise characters can select by tap
        this.BoxCollider.enabled = !this.itemData.configuration.isCharacter;

        this.SetPosition(new Vector3(posX, 0, posZ));
        this.UpdateConnectedItems();

        //if(this.itemData.configuration.productionRate > 0)
        //{
        //	this.ShowCollectNotificationUI(true, this.itemData.configuration.product);
        //}
    }

    public void UpdateConnectedItems()
    {
        //add archers if it is a tower item
        if (this.itemData.name == "Tower")
        {
            BaseItemScript towerArcher = null;
            if (this.connectedItems.Count > 0)
            {
                towerArcher = this.connectedItems[0];
            }

            if (towerArcher == null)
            {
                towerArcher = SceneManager.instance.AddItem(1502, true, ownedItem);
                this.connectedItems.Add(towerArcher);
            }

            towerArcher.SetPosition(this.GetCenterPosition() + new Vector3(0, 1.2f, 0));

            towerArcher.SetState(state);
            towerArcher.SetDirection(direction);
        }

        //add builder to builder hut
        if (this.itemData.name == "BuilderHut" && SceneManager.instance.gameMode == Common.GameMode.NORMAL)
        {
            if (SceneManager.instance.selectedItem == this)
            {
                //that means the hut is on drag
                //builder comes to hut only after on stop drag
                return;
            }

            BaseItemScript builder = null;
            if (this.connectedItems.Count > 0)
            {
                builder = this.connectedItems[0];
            }

            if (builder == null)
            {
                builder = SceneManager.instance.AddItem(3823, true, ownedItem);
                builder.SetPosition(this.GetRandomFrontCellPosition());

                //connect builder item to the builder hut
                this.connectedItems.Add(builder);

                //connect this builder hut item to builder
                builder.connectedItems.Add(this);
            }

            builder.ReturnBuilder();
        }
    }

    void Update()
    {
        this.Renderer.Refresh();

        if (SceneManager.instance.gameMode == Common.GameMode.NORMAL && this.itemData.configuration.productionRate > 0)
            this.Production.UpdateProduction();
    }

    /// <summary>
    /// Sets the angle.
    /// </summary>
    /// <param name="angle">Angle.</param>
    public void SetAngle(float angle)
    {
        Common.Direction direction = Common.Direction.BOTTOM_RIGHT;
        float minAnge = 999;
        foreach (KeyValuePair<float, Common.Direction> entry in this._angleToDirectionMap)
        {
            float a = Mathf.Abs(angle - entry.Key);
            if (a < minAnge)
            {
                minAnge = a;
                direction = entry.Value;
            }
        }

        this.SetDirection(direction);
    }

    /// <summary>
    /// Sets the direction.
    /// </summary>
    /// <param name="direction">Direction.</param>
    public void SetDirection(Common.Direction direction)
    {
        this.direction = direction;
        this.UpdateConnectedItems();
    }

    /// <summary>
    /// Sets the state.
    /// </summary>
    /// <param name="state">State.</param>
    public void SetState(Common.State state)
    {
        if (state != this.state)
        {
            this.state = state;
            this.UpdateConnectedItems();
        }
    }

    /// <summary>
    /// Sets the position.
    /// </summary>
    /// <param name="position">Position.</param>
    public void SetPosition(Vector3 position)
    {
        this.transform.localPosition = position;
    }

    public int GetPositionX()
    {
        return (int) this.GetPosition().x;
    }

    public int GetPositionZ()
    {
        return (int) this.GetPosition().z;
    }

    /// <summary>
    /// Gets the position.
    /// </summary>
    /// <returns>The position.</returns>
    public Vector3 GetPosition()
    {
        if (this != null)
        {
            return this.transform.localPosition;
        }
        else
        {
            return Vector3.zero;
        }
    }

    public Vector3 GetCenterPosition()
    {
        return this.GetPosition() + this.GetSize() / 2.0f;
    }

    /// <summary>
    /// Gets the position.
    /// </summary>
    /// <returns>The position.</returns>
    public Vector3 GetSize()
    {
        return new Vector3(this.transform.localScale.x, 0, this.transform.localScale.z);
    }

    /// <summary>
    /// Sets the size.
    /// </summary>
    /// <param name="size">Size.</param>
    public void SetSize(Vector3 size)
    {
        this.transform.localScale = size;
    }

    public void SetSelected(bool isTrue)
    {
        this.UI.ShowSelectionUI(isTrue);

        if (!isTrue)
        {
            if (!_IsInPlacablePosition())
            {
                //move to old position if it is not in placable position
                this.SetPosition(this._oldPosition);
                this.OnItemDragStop(null);
            }

            UIManager.instance.HideItemOptions();
        }

        this.UpdateConnectedItems();

        if (isTrue)
        {
            //play sound for tap item , here loop is false
            SoundManager.instance.PlaySound(SoundManager.instance.TapOnItem, false);
            UIManager.instance.ShowItemOptions();
        }
    }

    public void LookAt(BaseItemScript item)
    {
        this.LookAt(item.GetCenterPosition());
    }

    public void LookAt(Vector3 point)
    {
        Vector2 a = Utilities.GetScreenPosition(this.transform.position - new Vector3(0, 0, 1));
        Vector2 b = Utilities.GetScreenPosition(this.transform.position);
        Vector2 c = Utilities.GetScreenPosition(point);

        float angle = Utilities.ClockwiseAngleOf3Points(a, b, c);
        this.SetAngle(angle);
    }

    /* DRAG FUNCTIONALITY */
    private Vector3 _oldPosition;
    private Vector3 _deltaDistance;

    public void OnItemDragStart(CameraManager.CameraEvent evt)
    {
        this._deltaDistance = this.GetPosition() - evt.point;
        GroundManager.instance.UpdateBaseItemNodes(this, GroundManager.Action.REMOVE);

        bool isPlacable = _IsInPlacablePosition();
        if (isPlacable)
        {
            this.Renderer.ShowGroundPatch(false);
            if (this.UI.selectionUIInstance != null)
            {
                this.UI.selectionUIInstance.ShowGrid(true);
            }

            this._oldPosition = this.GetPosition();
        }
    }

    public void OnItemDrag(CameraManager.CameraEvent evt)
    {
        Vector3 point = evt.point + _deltaDistance;
        point.x = Mathf.Floor(point.x);
        point.z = Mathf.Floor(point.z);

        if (point != this.transform.localPosition)
        {
            this.SetPosition(new Vector3(Mathf.Floor(point.x), 0, Mathf.Floor(point.z)));
            this.UpdateConnectedItems();

            bool isPlacable = _IsInPlacablePosition();
            if (isPlacable)
            {
                this.UI.selectionUIInstance.SetGridColor(Color.green);
            }
            else
            {
                this.UI.selectionUIInstance.SetGridColor(Color.red);
            }

            //play sound for drag item , here loop is false
            SoundManager.instance.PlaySound(SoundManager.instance.Tap1, false);
        }
    }

    public void OnItemDragStop(CameraManager.CameraEvent evt)
    {
        bool isPlacable = _IsInPlacablePosition();

        if (isPlacable)
        {
            this.Renderer.ShowGroundPatch(true);
            if (this.UI.selectionUIInstance != null)
            {
                this.UI.selectionUIInstance.ShowGrid(false);
            }

            GroundManager.instance.UpdateBaseItemNodes(this, GroundManager.Action.ADD);
            DataBaseManager.instance.UpdateItemData(this);

            //play sound for end drag item , here loop is false
            SoundManager.instance.PlaySound(SoundManager.instance.TapOnItem, false);
        }
    }

    /* DRAG FUNCTIONALITY END*/

    private bool _IsInPlacablePosition()
    {
        bool isPlacable = GroundManager.instance.IsPositionPlacable(this.GetPosition(), this.itemData.gridSize,
            this.itemData.gridSize, this.instanceId);
        return isPlacable;
    }


    public Vector3[] GetFrontCells()
    {
        int sizeX = (int) this.GetSize().x;
        int size = 2 * sizeX + 1;
        Vector3[] cells = new Vector3[size];
        int index = 0;

        for (int x = 0; x <= sizeX; x++)
        {
            for (int z = 0; z <= sizeX; z++)
            {
                if (x == sizeX || z == sizeX)
                {
                    Vector3 cellPos = this.GetPosition() + new Vector3(x, 0, z);
                    if (cellPos.x < 0 || cellPos.x >= GroundManager.instance.nodeWidth || cellPos.z < 0 ||
                        cellPos.z >= GroundManager.instance.nodeHeight)
                    {
                        //avoid cells out of the grid
                        continue;
                    }

                    cells[index] = cellPos;
                    index++;
                }
            }
        }

        return cells;
    }

    public Vector3 GetRandomFrontCellPosition()
    {
        Vector3[] frontCells = GetFrontCells();
        return frontCells[Random.Range(0, frontCells.Length - 1)];
    }

    public Vector3[] GetOuterCells()
    {
        int sizeX = (int) this.GetSize().x;

        if (sizeX <= 1)
        {
            return new Vector3[0];
        }

        List<Vector3> cells = new List<Vector3>();
        for (int x = 0; x <= sizeX; x++)
        {
            for (int z = 0; z <= sizeX; z++)
            {
                if (x == sizeX || z == sizeX || x == 0 || z == 0)
                {
                    Vector3 cellPos = this.GetPosition() + new Vector3(x, 0, z);
                    if (!cells.Contains(cellPos))
                    {
                        cells.Add(cellPos);
                    }
                }
            }
        }

        return cells.ToArray();
    }


    /* BUILDER BUILDING LOOP */
    public BaseItemScript buildingItem;
    private float _buildStartTime;

    public void BuilderAction(BaseItemScript baseItem)
    {
        if (!this.itemData.configuration.isCharacter)
        {
            return;
        }

        this.buildingItem = baseItem;
        this._buildStartTime = Time.time;

        Debug.Log(this.buildingItem.GetPosition());

        this.Walker.WalkToPosition(this.buildingItem.GetRandomFrontCellPosition());
        this.Walker.OnFinishWalk += this.BuilderLoop;
    }

    public void BuilderLoop()
    {
        if (Time.time - this._buildStartTime <= this.buildingItem.itemData.configuration.buildTime)
        {
            this.StartCoroutine(_BuilderLoop());
        }
        else
        {
            this.buildingItem = null;
            this.Walker.OnFinishWalk -= this.BuilderLoop;
            this.ReturnBuilder();
        }
    }

    private IEnumerator _BuilderLoop()
    {
        this.LookAt(this.buildingItem);
        this.SetState(Common.State.ATTACK);
        yield return new WaitForSeconds(2f);
        this.Walker.WalkToPosition(this.buildingItem.GetRandomFrontCellPosition());
        this.Walker.OnFinishWalk += this.BuilderLoop;
    }


    private BaseItemScript _randomWalkParentItem;

    public void WalkRandom(BaseItemScript parentItem)
    {
        if (!this.itemData.configuration.isCharacter)
        {
            return;
        }

        this._randomWalkParentItem = parentItem;
        Vector3 pos = Vector3.zero;

        if (parentItem == null)
        {
            Vector3 randomFreePosition = GroundManager.instance.GetRandomFreePosition();
            pos = randomFreePosition;
        }
        else
        {
            int posX = parentItem.GetPositionX();
            int posZ = parentItem.GetPositionZ();
            int sizeX = (int) parentItem.GetSize().x;
            int sizeZ = (int) parentItem.GetSize().z;

            pos.x = Random.Range(posX, posX + sizeX);
            pos.z = Random.Range(posZ, posZ + sizeZ);
        }

        this.Walker.WalkToPosition(pos);
        this.Walker.OnFinishWalk += this.WalkRandomLoop;
    }

    /* WALK RANDOM IN CITY*/
    public void WalkRandom()
    {
        this.WalkRandom(null);
    }

    public void WalkRandomLoop()
    {
        this.StartCoroutine(_WalkRandomLoop());
    }

    private IEnumerator _WalkRandomLoop()
    {
        yield return new WaitForSeconds(3);

        Vector3 pos = Vector3.zero;

        if (_randomWalkParentItem == null)
        {
            Vector3 randomFreePosition = GroundManager.instance.GetRandomFreePosition();
            pos = randomFreePosition;
        }
        else
        {
            int posX = _randomWalkParentItem.GetPositionX();
            int posZ = _randomWalkParentItem.GetPositionZ();
            int sizeX = (int) _randomWalkParentItem.GetSize().x;
            int sizeZ = (int) _randomWalkParentItem.GetSize().z;

            pos.x = Random.Range(posX, posX + sizeX);
            pos.z = Random.Range(posZ, posZ + sizeZ);
        }

        this.Walker.WalkToPosition(pos);
        this.Walker.OnFinishWalk += this.WalkRandomLoop;
    }

    public void OnReceiveHit(BaseItemScript enemy)
    {
        if (this.isDestroyed)
        {
            return;
        }

        this.UI.ShowEnergyBarUI(true);

        this.healthPoints -= enemy.itemData.configuration.hitPoints;
        float progress = this.healthPoints / this.itemData.configuration.healthPoints;
        this.UI.energyBarUIInstance.SetProgress(progress);

        if (this.healthPoints <= 0)
        {
            this.healthPoints = 0;
            this.Walker.CancelWalk();
            this.SetState(Common.State.DESTROYED);
            this.isDestroyed = true;
            this.UI.ShowEnergyBarUI(false);
            GroundManager.instance.UpdateBaseItemNodes(this, GroundManager.Action.REMOVE);

            if (!this.itemData.configuration.isCharacter && this.itemData.name != "Wall")
            {
                /* shake camera */
                CameraManager.instance.ShakeCamera();
                /* destruction particle */
                this.Particles.ShowDestructionParticle();
            }

            if (this.OnItemDestroy != null)
                this.OnItemDestroy.Invoke(this);
        }
    }

    /* WALL FUNCTIONS */
    /* if the item is a wall */

    public void UpdateWall()
    {
        bool hasRighNeighbourWall = false;
        bool hasLeftNeighbourWall = false;
        bool hasTopNeighbourWall = false;
        bool hasBottomNeighbourWall = false;

        //check item in right
        BaseItemScript item = GroundManager.instance.GetItemInPosition(this.GetPosition() + new Vector3(1, 0, 0));
        if (item != null && item.itemData.name == "Wall")
        {
            hasRighNeighbourWall = true;
        }

        //check item in left
        item = GroundManager.instance.GetItemInPosition(this.GetPosition() + new Vector3(-1, 0, 0));
        if (item != null && item.itemData.name == "Wall")
        {
            hasLeftNeighbourWall = true;
        }

        //check item in top
        item = GroundManager.instance.GetItemInPosition(this.GetPosition() + new Vector3(0, 0, 1));
        if (item != null && item.itemData.name == "Wall")
        {
            hasTopNeighbourWall = true;
        }

        //check item in bottom
        item = GroundManager.instance.GetItemInPosition(this.GetPosition() + new Vector3(0, 0, -1));
        if (item != null && item.itemData.name == "Wall")
        {
            hasBottomNeighbourWall = true;
        }


        int frame = 0;
        if (hasTopNeighbourWall && hasRighNeighbourWall)
        {
            frame = 3;
        }
        else if (hasRighNeighbourWall)
        {
            frame = 1;
        }
        else if (hasTopNeighbourWall)
        {
            frame = 2;
        }
        else
        {
            frame = 0;
        }

        this.Renderer.GetRenderQuads()[0].GetComponent<TextureSheetAnimationScript>().SetFrame(frame);
    }

    //walk the item to home for builder
    public void ReturnBuilder()
    {
        if (this.buildingItem != null)
        {
            return;
        }

        if (this.itemData.name == "Builder")
        {
            this.Walker.WalkToPosition(this.connectedItems[0].GetRandomFrontCellPosition());
            this.Walker.OnFinishWalk += _OnFinishWalkReturnBuilder;
        }
    }

    private void _OnFinishWalkReturnBuilder()
    {
        this.SetState(Common.State.IDLE);
        this.Walker.OnFinishWalk -= _OnFinishWalkReturnBuilder;
    }
}