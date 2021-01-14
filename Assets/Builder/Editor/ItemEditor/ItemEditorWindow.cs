using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using System.Text.RegularExpressions;

public class ItemEditorWindow : EditorWindow
{

    public class SimpleTreeView : TreeView
    {
        //	public static int selectedId = -1;

        public SimpleTreeView(TreeViewState treeViewState) : base(treeViewState)
        {
            if (ItemEditorWindow.itemsCollection != null && ItemEditorWindow.itemsCollection.list.Count > 0)
                Reload();
        }

        protected override TreeViewItem BuildRoot()
        {
            var mainRoot = new TreeViewItem { id = 0, depth = -1, displayName = "MainRoot" };
            List<TreeViewItem> items = new List<TreeViewItem>();

            if (ItemEditorWindow.itemsCollection != null && ItemEditorWindow.itemsCollection.list.Count > 0)
            {
                int id = 1;
                foreach (ItemsCollection.ItemData item in ItemEditorWindow.itemsCollection.list)
                {
                    var rootItem = new TreeViewItem { id = id, displayName = item.name };
                    mainRoot.AddChild(rootItem);
                    id++;
                }
            }
            //		var rootItem = new TreeViewItem { id = 1, displayName = "Empty" };
            //		mainRoot.AddChild (rootItem);

            SetupDepthsFromParentsAndChildren(mainRoot);
            return mainRoot;
        }


        protected override void SelectionChanged(IList<int> selectedIds)
        {
            base.SelectionChanged(selectedIds);

            if (selectedIds.Count > 0)
            {
                int selectedId = selectedIds[0];
                ItemEditorWindow.instance.SelectItem(selectedId - 1);
            }
        }

        protected override void RowGUI(RowGUIArgs args)
        {
            Rect position = args.rowRect;
            position.x += GetContentIndent(args.item);

            GUI.Label(position, args.item.displayName, EditorStyles.boldLabel);
            GUI.color = Color.white;
        }
    }

    public class SpriteAnimationItem
    {
        public SpriteCollection.SpriteData sprite;
        public float lastUpdatedTime;
        public int currentFrame = 0;

        public int numberOfColumns = 1;
        public int numberOfRows = 1;
    }


    public static ItemsCollection itemsCollection;
    public static SpriteCollection spriteCollection;


    const float WidthOfLetPanel = 200;
    const float WidthOfRightPanel = 260;

    Vector2 leftPanelScrollPos;
    Vector2 rightPanelScrollPos;


    public static Texture2D gridTexture;
    [SerializeField] TreeViewState _treeViewState;
    SimpleTreeView _simpleTreeView;

    [MenuItem("Window/Item Editor")]
    static void Init()
    {
        EnsureItemsCollection();
        EnsureSpriteCollection();

        gridTexture = Resources.Load("grid_4x4", typeof(Texture2D)) as Texture2D;

        // Get existing open window or if none, make a new one:
        ItemEditorWindow window = (ItemEditorWindow)ItemEditorWindow.GetWindow(typeof(ItemEditorWindow));
        window.minSize = new Vector2(800, 500);
        window.Show();
    }

    void Update()
    {
        UpdateSpriteAnimations();
    }


    public static ItemEditorWindow instance
    {
        get { return GetWindow<ItemEditorWindow>(); }
    }

    static void EnsureItemsCollection()
    {
        if (itemsCollection != null)
            return;

        itemsCollection = Resources.Load("ItemsCollection", typeof(ItemsCollection)) as ItemsCollection;
        if (itemsCollection == null)
        {
            ItemsCollection asset = ScriptableObject.CreateInstance<ItemsCollection>();
            string path = "Assets/City Builder Template/Resources/ItemsCollection.asset";
            AssetDatabase.CreateAsset(asset, path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = asset;
        }
    }

    static void EnsureSpriteCollection()
    {
        if (spriteCollection != null)
            return;

        spriteCollection = Resources.Load("SpriteCollection", typeof(SpriteCollection)) as SpriteCollection;
        if (spriteCollection == null)
        {
            SpriteCollection asset = ScriptableObject.CreateInstance<SpriteCollection>();
            string path = "Assets/City Builder Template/Resources/SpriteCollection.asset";
            AssetDatabase.CreateAsset(asset, path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = asset;
        }
    }

    void OnEnable()
    {
        EnsureItemsCollection();
        EnsureSpriteCollection();

        if (_treeViewState == null)
        {
            _treeViewState = new TreeViewState();
        }

        _simpleTreeView = new SimpleTreeView(_treeViewState);
    }

    void OnFocus()
    {
        OnEnable();
    }

    void OnGUI()
    {
        GUILayout.BeginHorizontal();

        //PREVIEW AREA
        RenderPreviewArea();

        //LEFT SIDE PANEL
        RenderLeftPanel();

        //RIGHT SIDE PANEL
        RenderRightPanel();

        GUILayout.EndHorizontal();
    }

    private string searchText;
    void RenderLeftPanel()
    {
        GUI.Box(new Rect(0, 0, WidthOfLetPanel, position.height), "", EditorStyles.helpBox);
        leftPanelScrollPos = EditorGUILayout.BeginScrollView(leftPanelScrollPos, GUILayout.Width(WidthOfLetPanel), GUILayout.Height(position.height));

        GUI.Box(new Rect(0, 0, WidthOfLetPanel - 1, 17), "", "toolbar");
        if (GUI.Button(new Rect(4, 0, 45, 14), "Create", "toolbarbutton"))
            TreeViewMenu(new Rect(4, 0, 45, 14), -1);

        GUI.BeginGroup(new Rect(76, 2, WidthOfLetPanel - 2, 14));
        searchText = GUI.TextField(new Rect(-16, 0, WidthOfLetPanel - 78, 14), searchText, "toolbarSeachTextField");
        GUI.EndGroup();
        //		selSearchParam = EditorGUI.Popup(new Rect(60, 2, 16, 16), "", selSearchParam, SearchParams, "ToolbarSeachTextFieldPopup");

        if (GUI.Button(new Rect(WidthOfLetPanel - 18, 2, 16, 14), "", "ToolbarSeachCancelButton"))
        {
            searchText = "";
            Repaint();
        }

        if (itemsCollection != null && itemsCollection.list.Count > 0)
            TreeView(new Rect(0, 18, WidthOfLetPanel - 2, this.position.height - 18));

        EditorGUILayout.EndScrollView();
    }

    void TreeView(Rect position)
    {
        if (Event.current.button == 0 && Event.current.type == EventType.MouseDown && position.Contains(Event.current.mousePosition))
        {
            _simpleTreeView.SetSelection(new List<int>());
            this.selectedItemIndex = -1;
        }

        _simpleTreeView.OnGUI(position);

        if (Event.current.button == 1 && Event.current.type == EventType.MouseUp && position.Contains(Event.current.mousePosition))
        {
            IList<int> selections = _simpleTreeView.GetSelection();
            int selectedId = -1;
            if (selections.Count != 0)
            {
                selectedId = _simpleTreeView.GetSelection()[0];
            }
            TreeViewMenu(new Rect(Event.current.mousePosition.x, Event.current.mousePosition.y - 10, 0, 0), selectedId);
        }
    }

    private int selectedItemIndex = -1;
    public void SelectItem(int index)
    {
        selectedItemIndex = index;
        if (itemsCollection != null && itemsCollection.list.Count > 0 && index <= itemsCollection.list.Count - 1)
            LoadDataValues();
        
        this.InitSpriteAnimations();

        direction = 0;
        state = 0;
    }

    private void LoadDataValues()
    {
        if (selectedItemIndex == -1)
            return;
        
        ItemsCollection.ItemData itemData = itemsCollection.list[selectedItemIndex];

        id = itemData.id.ToString();
        name = itemData.name;
		thumb = itemData.thumb;

        gridSize = itemData.gridSize;
        if (gridSize == 0)
            gridTexture = null;
        else if (gridSize == 1)
            gridTexture = Resources.Load("grid_1x1", typeof(Texture2D)) as Texture2D;
        else if (gridSize == 2)
            gridTexture = Resources.Load("grid_2x2", typeof(Texture2D)) as Texture2D;
        else if (gridSize == 3)
            gridTexture = Resources.Load("grid_3x3", typeof(Texture2D)) as Texture2D;
        else if (gridSize == 4)
            gridTexture = Resources.Load("grid_4x4", typeof(Texture2D)) as Texture2D;
        else if (gridSize == 5)
            gridTexture = Resources.Load("grid_5x5", typeof(Texture2D)) as Texture2D;
        else if (gridSize == 6)
            gridTexture = Resources.Load("grid_6x6", typeof(Texture2D)) as Texture2D;



    }

    void TreeViewMenu(Rect position, int selectedId)
    {
        GenericMenu toolsMenu = new GenericMenu();

        if (selectedId == -1)
        {
            toolsMenu.AddItem(new GUIContent("Create Item"), false, data =>
            {
                TriggerMenuEvent((string)data, selectedId);
            }, "Create Item");
        }


        if (selectedId > 0)
        {
            toolsMenu.AddItem(new GUIContent("Delete Item"), false, data =>
            {
                TriggerMenuEvent((string)data, selectedId);
            }, "Delete Item");
        }

        toolsMenu.DropDown(position);
        EditorGUIUtility.ExitGUI();
    }

    void TriggerMenuEvent(string evt, int selectedIndex)
    {
        EnsureItemsCollection();

        switch (evt)
        {
            case "Create Item":
                if (selectedIndex == 1)
                {
                    selectedIndex = -1;
                }
                itemsCollection.AddNewItem();
                _simpleTreeView.Reload();
                _simpleTreeView.SetSelection(new List<int>() { itemsCollection.list.Count });
                this.SelectItem(itemsCollection.list.Count - 1);
                break;

            case "Delete Item":
                itemsCollection.RemoveItem(selectedIndex);
                if (itemsCollection != null && itemsCollection.list.Count > 0)
                {
                    _simpleTreeView.Reload();
                }
                break;
        }
    }


    public string id;
    public string _oldId = "";

    public string name;
    private string _oldName = "";


    //	private int offsetX = 0;
    //	private string offsetXString = "0";
    //	private int offsetY = 0;
    //	private string offsetYString = "0";
    //	private int scale = 100;
    //	private string scaleString = "100";

    private bool spritesFoldout;
    private bool configFoldout;
    private bool aiConfigFoldout;
    private bool newConfigFoldout;

    private int state = 0;
    private int _oldState = 0;

    private int direction = 0;
    private int _oldDirection = 0;

    private string[] stateOptions = new string[] {
        "Idle", "Walk", "Attack", "Destroyed"
    };

    private string[] directionsOptions = new string[] {
        "Bottom", "Bottom Right", "Right", "Top Right", "Top"
    };

    private string[] gridSizeOptions = new string[] {
        "None", "1x1", "2x2", "3x3", "4x4", "5x5", "6x6"
    };
    private int gridSize = 4;
    private int _oldGridSize = 4;
	private Texture2D thumb;

    void RenderRightPanel()
    {
        GUILayout.BeginArea(new Rect(position.width - WidthOfRightPanel, 0, WidthOfRightPanel, position.height));
        GUI.Box(new Rect(0, 0, WidthOfRightPanel, position.height), "", EditorStyles.helpBox);

        if (selectedItemIndex != -1)
        {
            rightPanelScrollPos = EditorGUILayout.BeginScrollView(rightPanelScrollPos, GUILayout.Width(WidthOfRightPanel), GUILayout.Height(position.height));

            GUILayout.BeginHorizontal();
            GUILayout.Label("ID");
            //			id = GUILayout.TextField (id);
            EditorGUILayout.SelectableLabel(id, EditorStyles.textField, GUILayout.Height(EditorGUIUtility.singleLineHeight));
            if (id != _oldId)
                _oldId = id;

            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Name");
            name = GUILayout.TextField(name);
            if (name != _oldName)
            {
                _oldName = name;
                this._simpleTreeView.Reload();
            }

            GUILayout.EndHorizontal();

            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            GUILayout.Space(5);
            spritesFoldout = EditorGUILayout.Foldout(spritesFoldout, "SPRITE", true);

            if (spritesFoldout)
            {
				
                state = EditorGUILayout.Popup("State", state, stateOptions);
                if (state != _oldState)
                {
                    _oldState = state;
                    this.LoadDataValues();
                    this.InitSpriteAnimations();
                }

                direction = EditorGUILayout.Popup("Direction", direction, directionsOptions);
                if (direction != _oldDirection)
                {
                    _oldDirection = direction;
                    this.LoadDataValues();
                }
                GUILayout.Space(5);
                this.RenderSpritesList();
                GUILayout.BeginHorizontal();
                GUILayout.Space(20);
                if (GUILayout.Button("Add Sprite"))
                    AddSpriteButtonMenu();
                GUILayout.EndHorizontal();
				thumb = (Texture2D)EditorGUILayout.ObjectField("Thumb", thumb, typeof(Texture2D));
            }

            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            GUILayout.Space(5);
            configFoldout = EditorGUILayout.Foldout(configFoldout, "CONFIG", true);
            if (configFoldout)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Space(20);
                gridSize = EditorGUILayout.Popup("Grid Size", gridSize, gridSizeOptions);
                if (gridSize != _oldGridSize)
                {
                    _oldGridSize = gridSize;
                    this.UpdateDataValues();
                    this.LoadDataValues();
                }
                GUILayout.EndHorizontal();


                ItemsCollection.ItemData itemData = itemsCollection.list[selectedItemIndex];
                foreach (var property in itemData.configuration.GetType().GetFields())
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Space(20);
                    GUILayout.Label(property.Name);

                    var val = property.GetValue(itemData.configuration);
                    if (val.GetType() == typeof(System.Int32))
                    {
                        string temp = GUILayout.TextField(property.GetValue(itemData.configuration).ToString());
                        int parsedValue = 0;
                        int.TryParse(Utilities.CleanStringForInt(temp), out parsedValue);
                        property.SetValue(itemData.configuration, parsedValue);
                    }
                    else if (val.GetType() == typeof(System.Single))
                    {
                        string temp = GUILayout.TextField(property.GetValue(itemData.configuration).ToString());
                        float parsedValue = 0;
                        float.TryParse(Utilities.CleanStringForFloat(temp), out parsedValue);
                        property.SetValue(itemData.configuration, parsedValue);
                    }
                    else if (val.GetType() == typeof(System.Boolean))
                    {
                        bool temp = GUILayout.Toggle((bool)property.GetValue(itemData.configuration), "");
                        property.SetValue(itemData.configuration, temp);
                    }
                    else if (val.GetType() == typeof(System.String))
                    {
                        string temp = GUILayout.TextField((string)property.GetValue(itemData.configuration));
                        property.SetValue(itemData.configuration, temp);
                    }


                    GUILayout.EndHorizontal();
                    //Debug.Log(property.GetValue(itemData.confTest).GetType());
                    //Debug.Log("Name: " + property.Name + " Value: " + property.GetValue(this));
                    //property.SetValue(this, 65);
                }
            }

            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            //GUILayout.Space(5);
            //aiConfigFoldout = EditorGUILayout.Foldout(aiConfigFoldout, "AI CONFIG", true);
            //if (aiConfigFoldout)
            //{
            //    GUILayout.BeginHorizontal();
            //    GUILayout.Space(20);
            //    isCharacter = GUILayout.Toggle(isCharacter, "Is character");
            //    GUILayout.EndHorizontal();

            //    if (isCharacter)
            //    {
            //        GUILayout.BeginHorizontal();
            //        GUILayout.Space(20);
            //        GUILayout.Label("Walk speed");
            //        walkSpeed = GUILayout.TextField(walkSpeed);
            //        walkSpeed = Utilities.CleanStringForFloat(walkSpeed);
            //        GUILayout.EndHorizontal();
            //    }

            //    GUILayout.Space(10);
            //    GUILayout.BeginHorizontal();
            //    GUILayout.Space(20);
            //    GUILayout.Label("Attack range");
            //    attackRange = GUILayout.TextField(attackRange);
            //    attackRange = Utilities.CleanStringForFloat(attackRange);
            //    GUILayout.EndHorizontal();

            //    GUILayout.BeginHorizontal();
            //    GUILayout.Space(20);
            //    GUILayout.Label("Defence range");
            //    defenceRange = GUILayout.TextField(defenceRange);
            //    defenceRange = Utilities.CleanStringForFloat(defenceRange);
            //    GUILayout.EndHorizontal();

            //    GUILayout.BeginHorizontal();
            //    GUILayout.Space(20);
            //    GUILayout.Label("Health points");
            //    healthPoints = GUILayout.TextField(healthPoints);
            //    healthPoints = Utilities.CleanStringForFloat(healthPoints);
            //    GUILayout.EndHorizontal();

            //    GUILayout.BeginHorizontal();
            //    GUILayout.Space(20);
            //    GUILayout.Label("Hit points");
            //    hitPoints = GUILayout.TextField(hitPoints);
            //    hitPoints = Utilities.CleanStringForFloat(hitPoints);
            //    GUILayout.EndHorizontal();
            //}

            //GUILayout.Space(5);
            //newConfigFoldout = EditorGUILayout.Foldout(newConfigFoldout, "CONFIGURATIONS", true);
            //if (newConfigFoldout)
            //{
            //    ItemsCollection.ItemData itemData = itemsCollection.list[selectedItemIndex];
            //    foreach (var property in itemData.configuration.GetType().GetFields())
            //    {
            //        GUILayout.BeginHorizontal();
            //        GUILayout.Space(20);
            //        GUILayout.Label(property.Name);

            //        var val = property.GetValue(itemData.configuration);
            //        if (val.GetType() == typeof(System.Int32))
            //        {
            //            string temp = GUILayout.TextField(property.GetValue(itemData.configuration).ToString());
            //            int parsedValue = 0;
            //            int.TryParse(Utilities.CleanStringForInt(temp), out parsedValue);
            //            property.SetValue(itemData.configuration, parsedValue);
            //        }
            //        else if (val.GetType() == typeof(System.Single))
            //        {
            //            string temp = GUILayout.TextField(property.GetValue(itemData.configuration).ToString());
            //            float parsedValue = 0;
            //            float.TryParse(Utilities.CleanStringForFloat(temp), out parsedValue);
            //            property.SetValue(itemData.configuration, parsedValue);
            //        }
            //        else if (val.GetType() == typeof(System.Boolean))
            //        {
            //            bool temp = GUILayout.Toggle((bool)property.GetValue(itemData.configuration), "");
            //            property.SetValue(itemData.configuration, temp);
            //        }
            //        else if (val.GetType() == typeof(System.String))
            //        {
            //            string temp = GUILayout.TextField((string)property.GetValue(itemData.configuration));
            //            property.SetValue(itemData.configuration, temp);
            //        }

            //        GUILayout.EndHorizontal();
            //        //Debug.Log(property.GetValue(itemData.confTest).GetType());
            //        //Debug.Log("Name: " + property.Name + " Value: " + property.GetValue(this));
            //        //property.SetValue(this, 65);
            //    }
            //}

            EditorGUILayout.EndScrollView();

            if (GUI.changed)
                UpdateDataValues();
        }
        GUILayout.EndArea();
    }

    List<SpriteCollection.SpriteData> removedSprites = new List<SpriteCollection.SpriteData>();
    void RenderSpritesList()
    {
        ItemsCollection.ItemData itemData = itemsCollection.list[selectedItemIndex];

        foreach (int spriteId in itemData.GetSprites((Common.State)state))
        {
            SpriteCollection.SpriteData sprite = spriteCollection.GetSprite(spriteId);

            GUILayout.BeginHorizontal();
            GUILayout.Space(20);

            //BOX
            Rect pos = GUILayoutUtility.GetLastRect();
            pos.height = 25;
            pos.x += 20;
            pos.width = WidthOfRightPanel - 25;
            GUI.Box(pos, "", "box");

            GUILayout.Label(sprite.name);
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("-"))
                removedSprites.Add(sprite);

            GUILayout.EndHorizontal();
            GUILayout.Space(5);
        }

        foreach (SpriteCollection.SpriteData sprite in removedSprites)
        {
            itemData.RemoveSprite(sprite, (Common.State)state);
            this.InitSpriteAnimations();
        }

        if (removedSprites.Count != null)
        {
            removedSprites = new List<SpriteCollection.SpriteData>();
        }

    }

    void AddSpriteButtonMenu()
    {
        Rect position = new Rect(Event.current.mousePosition.x, Event.current.mousePosition.y - 10, 0, 0);

        GenericMenu toolsMenu = new GenericMenu();

        if (spriteCollection != null)
        {
            foreach (SpriteCollection.SpriteData sprite in spriteCollection.list)
            {
                toolsMenu.AddItem(new GUIContent(sprite.name), false, data =>
                {
                    ItemsCollection.ItemData itemData = itemsCollection.list[selectedItemIndex];
                    itemData.AddSprite((SpriteCollection.SpriteData)data, (Common.State)state);
                    this.InitSpriteAnimations();
                }, sprite);
            }
        }

        toolsMenu.DropDown(position);
        EditorGUIUtility.ExitGUI();
    }

    private void UpdateDataValues()
    {

        if (selectedItemIndex == -1)
            return;
        
        ItemsCollection.ItemData itemData = itemsCollection.list[selectedItemIndex];

        itemData.name = this.name;
        itemData.gridSize = this.gridSize;
		itemData.thumb = thumb;

        EditorUtility.SetDirty(itemsCollection);
    }

    private Texture2D TextureObjectField(string name, Texture2D texture)
    {
        Texture2D outTexture;
        GUILayout.BeginHorizontal();
        GUILayout.Space(20);
        outTexture = (Texture2D)EditorGUILayout.ObjectField(
            name,
            texture,
            typeof(Texture2D));
        GUILayout.EndHorizontal();
        return outTexture;
    }

    private Vector2 defaultGridSize = new Vector2(512, 512);
    private Vector2 defaultImgSize = new Vector2(256, 256);

    private List<SpriteAnimationItem> spriteAnimationList = new List<SpriteAnimationItem>();
    void RenderPreviewArea()
    {

        if (gridTexture == null)
            gridTexture = Resources.Load("grid_4x4", typeof(Texture2D)) as Texture2D;

        //DRAW GRID
        Vector2 centerPointOfPreviewArea = new Vector2((WidthOfLetPanel + position.width - WidthOfRightPanel) / 2, position.height / 2);
        GUI.Label(new Rect(centerPointOfPreviewArea.x - defaultGridSize.x / 2, centerPointOfPreviewArea.y - defaultGridSize.y / 2, defaultGridSize.x, defaultGridSize.y), gridTexture);

        //DRAW ITEM SPRITE
        if (selectedItemIndex != -1)
        {
            for (int index = 0; index < this.spriteAnimationList.Count; index++)
            {
                SpriteAnimationItem spriteAnimationItem = this.spriteAnimationList[index];

                SpriteCollection.TextureData textureData = GetTextureDataDirection(spriteAnimationItem.sprite);
                Texture2D texture = textureData.texture;

                if (texture == null)
                    continue;

                float scale = textureData.scale;
                int numberOfColumns = spriteAnimationItem.numberOfColumns;
                int numberOfRows = spriteAnimationItem.numberOfRows;
                float offsetX = textureData.offsetX;
                float offsetY = textureData.offsetY;
                int gridSize = spriteAnimationItem.sprite.gridSize;

                defaultImgSize.x = 256 * scale / 100;
                defaultImgSize.x = 256 * scale / 100;

                float heightFactor = ((float)texture.height / (float)texture.width) * ((float)numberOfColumns / numberOfRows);

                float upFactor = Mathf.Cos(Mathf.Deg2Rad * 45) * gridSize / 2.0f;

                float x = (centerPointOfPreviewArea.x - defaultImgSize.x / 2) + offsetX;
                float y = (centerPointOfPreviewArea.y - defaultImgSize.x * heightFactor / 2) - offsetY;

                float framePaddingX = defaultImgSize.x * (spriteAnimationItem.currentFrame % numberOfColumns);
                float framePaddingY = defaultImgSize.x * heightFactor * (spriteAnimationItem.currentFrame / numberOfColumns);

                Rect imgRect = new Rect(x, y, defaultImgSize.x, defaultImgSize.x * heightFactor);

                GUI.BeginGroup(imgRect);
                GUI.DrawTexture(new Rect(-framePaddingX, -framePaddingY, defaultImgSize.x * numberOfColumns, defaultImgSize.x * spriteAnimationItem.numberOfRows * heightFactor), texture);
                GUI.EndGroup();
            }
        }
    }

    static int SortByDepth(SpriteAnimationItem sp1, SpriteAnimationItem sp2)
    {
        int sp1Depth = (int)sp1.sprite.renderingLayer + sp1.sprite.renderingOrder;
        int sp2Depth = (int)sp2.sprite.renderingLayer + sp2.sprite.renderingOrder;
        return sp1Depth.CompareTo(sp2Depth);
    }

    private SpriteCollection.TextureData GetTextureDataDirection(SpriteCollection.SpriteData sprite)
    {
        SpriteCollection.TextureData selectedTextureData = sprite.bottomTexture;

        switch ((Common.Direction)direction)
        {
            case Common.Direction.BOTTOM:
                selectedTextureData = sprite.bottomTexture;
                break;
            case Common.Direction.BOTTOM_RIGHT:
                selectedTextureData = sprite.bottomRightTexture;
                break;
            case Common.Direction.RIGHT:
                selectedTextureData = sprite.rightTexture;
                break;
            case Common.Direction.TOP_RIGHT:
                selectedTextureData = sprite.topRightTexture;
                break;
            case Common.Direction.TOP:
                selectedTextureData = sprite.topTexture;
                break;
        }

        return selectedTextureData;
    }

    public void InitSpriteAnimations()
    {
        if (selectedItemIndex == -1)
            return;
        
        ItemsCollection.ItemData itemData = itemsCollection.list[selectedItemIndex];

        this.spriteAnimationList = new List<SpriteAnimationItem>();
        foreach (int spriteId in itemData.GetSprites((Common.State)state))
        {
            SpriteCollection.SpriteData sprite = spriteCollection.GetSprite(spriteId);

            SpriteAnimationItem spriteAnimationData = new SpriteAnimationItem();
            spriteAnimationData.sprite = sprite;
            spriteAnimationData.lastUpdatedTime = Time.realtimeSinceStartup;

            SpriteCollection.TextureData textureData = GetTextureDataDirection(sprite);

            spriteAnimationData.numberOfColumns = textureData.numberOfColumns;
            spriteAnimationData.numberOfRows = textureData.numberOfRows;

            this.spriteAnimationList.Add(spriteAnimationData);
        }
        this.spriteAnimationList.Sort(SortByDepth);
    }

    public void UpdateSpriteAnimations()
    {
        for (int index = 0; index < this.spriteAnimationList.Count; index++)
        {
            SpriteAnimationItem spriteAnimationItem = this.spriteAnimationList[index];
            SpriteCollection.TextureData textureData = GetTextureDataDirection(spriteAnimationItem.sprite);

            if (textureData.framesCount > 1)
            {

                if (Time.realtimeSinceStartup - spriteAnimationItem.lastUpdatedTime >= 1.0f / textureData.fps)
                {
                    spriteAnimationItem.lastUpdatedTime = Time.realtimeSinceStartup;
                    spriteAnimationItem.currentFrame++;
                    if (spriteAnimationItem.currentFrame >= textureData.framesCount)
                        spriteAnimationItem.currentFrame = 0;
                }
            }
            else
            {
                spriteAnimationItem.currentFrame = 0;
            }
        }
        Repaint();
    }


    void ChangableIntField(string label, ref string valueString, ref int valueInt, int min, int max)
    {

        GUILayout.BeginHorizontal();
        valueString = EditorGUILayout.TextField(label, valueString);
        if (GUI.changed)
        {
            int oldValueInt = valueInt;
            if (!int.TryParse(valueString, out valueInt))
            {
                valueInt = oldValueInt;
                valueString = valueInt.ToString();
            }

            if (valueInt < min)
            {
                valueInt = min;
                valueString = valueInt.ToString();
            }

            if (valueInt > max)
            {
                valueInt = max;
                valueString = valueInt.ToString();
            }
        }

        if (GUILayout.Button("-"))
        {
            valueInt--;
            if (valueInt < min)
                valueInt = min;
            
            valueString = valueInt.ToString();
        };

        if (GUILayout.Button("+"))
        {
            valueInt++;
            if (valueInt > max)
                valueInt = max;
            
            valueString = valueInt.ToString();
        };

        GUILayout.EndHorizontal();
    }
}
