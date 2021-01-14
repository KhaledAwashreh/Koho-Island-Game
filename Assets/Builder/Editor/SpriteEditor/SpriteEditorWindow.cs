using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.IMGUI.Controls;

public class SpriteEditorWindow : EditorWindow {

	public class SimpleTreeView : TreeView {
		//	public static int selectedId = -1;

		public SimpleTreeView(TreeViewState treeViewState) : base(treeViewState){
			if (SpriteEditorWindow.spriteCollection != null && SpriteEditorWindow.spriteCollection.list.Count > 0) {
				Reload ();
			}
		}

		protected override TreeViewItem BuildRoot (){
			var mainRoot = new TreeViewItem {id = 0, depth = -1, displayName = "MainRoot"};
			List<TreeViewItem> items = new List<TreeViewItem> ();

			if (SpriteEditorWindow.spriteCollection != null && SpriteEditorWindow.spriteCollection.list.Count > 0) {
				int id = 1;
				foreach (SpriteCollection.SpriteData sprite in SpriteEditorWindow.spriteCollection.list) {
					var rootItem = new TreeViewItem { id = id, displayName = sprite.name };
					mainRoot.AddChild (rootItem);
					id++;
				}
			}
			//		var rootItem = new TreeViewItem { id = 1, displayName = "Empty" };
			//		mainRoot.AddChild (rootItem);

			SetupDepthsFromParentsAndChildren (mainRoot);
			return mainRoot;
		}


		protected override void SelectionChanged (IList<int> selectedIds) {
			base.SelectionChanged (selectedIds);

			if (selectedIds.Count > 0) {
				int selectedId = selectedIds [0];
				SpriteEditorWindow.instance.SelectSprite (selectedId - 1);
			}
		}

		protected override void RowGUI (RowGUIArgs args) {
			Rect position = args.rowRect;
			position.x += GetContentIndent (args.item);

			GUI.Label (position, args.item.displayName, EditorStyles.boldLabel);
			GUI.color = Color.white;
		}
	}

	public static SpriteCollection spriteCollection;

	const float WidthOfLetPanel = 200;
	const float WidthOfRightPanel = 260;

	Vector2 leftPanelScrollPos;
	Vector2 rightPanelScrollPos;



	public static Texture2D gridTexture;
	[SerializeField] TreeViewState _treeViewState;
	SimpleTreeView _simpleTreeView;

	[MenuItem("Window/Sprite Editor")]
	static void Init() {
		
		EnsureSpriteCollection ();

		gridTexture = Resources.Load("grid_4x4", typeof(Texture2D)) as Texture2D;

		// Get existing open window or if none, make a new one:
		SpriteEditorWindow window = (SpriteEditorWindow)EditorWindow.GetWindow(typeof(SpriteEditorWindow));
		window.minSize = new Vector2 (800, 500);
		window.Show();
	}

	void Update() {
		UpdateAnimationViewFrames ();
	}
		

	public static SpriteEditorWindow instance {
		get { return GetWindow< SpriteEditorWindow >(); }
	}

	static void EnsureSpriteCollection(){
		if (spriteCollection != null) {
			return;
		}

		spriteCollection = Resources.Load("SpriteCollection", typeof(SpriteCollection)) as SpriteCollection;
		if (spriteCollection == null) {
			SpriteCollection asset = ScriptableObject.CreateInstance<SpriteCollection> ();
			string path = "Assets/City Builder Template/Resources/SpriteCollection.asset";
			AssetDatabase.CreateAsset (asset, path);
			AssetDatabase.SaveAssets ();
			AssetDatabase.Refresh();
			EditorUtility.FocusProjectWindow ();
			Selection.activeObject = asset;
		}
	}

	void OnEnable(){
		EnsureSpriteCollection ();

		if (_treeViewState == null) {
			_treeViewState = new TreeViewState ();
		}

		_simpleTreeView = new SimpleTreeView(_treeViewState);
	}

	void OnFocus() {
		OnEnable ();
	}

	void OnGUI(){
		GUILayout.BeginHorizontal ();

		//PREVIEW AREA
		RenderPreviewArea();

		//LEFT SIDE PANEL
		RenderLeftPanel();

		//RIGHT SIDE PANEL
		RenderRightPanel();

		GUILayout.EndHorizontal ();
	}

	private string searchText;
	void RenderLeftPanel(){
		GUI.Box (new Rect (0, 0, WidthOfLetPanel, position.height),"", EditorStyles.helpBox);
		leftPanelScrollPos = EditorGUILayout.BeginScrollView(leftPanelScrollPos, GUILayout.Width(WidthOfLetPanel), GUILayout.Height(position.height));

		GUI.Box(new Rect(0, 0, WidthOfLetPanel-1, 17), "", "toolbar");
		if (GUI.Button (new Rect (4, 0, 45, 14), "Create", "toolbarbutton")) {
			TreeViewMenu (new Rect (4, 0, 45, 14), -1);
		};

		GUI.BeginGroup(new Rect(76,2,WidthOfLetPanel-2,14));
		searchText = GUI.TextField(new Rect(-16, 0, WidthOfLetPanel-78, 14), searchText, "toolbarSeachTextField");
		GUI.EndGroup();
//		selSearchParam = EditorGUI.Popup(new Rect(60, 2, 16, 16), "", selSearchParam, SearchParams, "ToolbarSeachTextFieldPopup");

		if (GUI.Button(new Rect(WidthOfLetPanel-18, 2, 16, 14), "", "ToolbarSeachCancelButton"))
		{
			searchText = "";
			Repaint();
		}

		if (spriteCollection != null && spriteCollection.list.Count > 0) {
			TreeView (new Rect (0, 18, WidthOfLetPanel-2, this.position.height-18));
		}

		EditorGUILayout.EndScrollView ();
	}

	void TreeView(Rect position){
		if (Event.current.button == 0 && Event.current.type == EventType.MouseDown && position.Contains (Event.current.mousePosition)) {
			_simpleTreeView.SetSelection (new List<int>());
			this.selectedSpriteIndex = -1;
		}

		_simpleTreeView.OnGUI(position);

		if (Event.current.button == 1 && Event.current.type == EventType.MouseUp && position.Contains (Event.current.mousePosition)) {
			IList<int> selections = _simpleTreeView.GetSelection ();
			int selectedId = -1;
			if (selections.Count != 0) {
				selectedId = _simpleTreeView.GetSelection () [0];
			}
			TreeViewMenu (new Rect (Event.current.mousePosition.x, Event.current.mousePosition.y - 10, 0, 0), selectedId);
		}
	}

	private int selectedSpriteIndex = -1;
	public void SelectSprite(int index){
		selectedSpriteIndex = index;
		if (spriteCollection != null && spriteCollection.list.Count > 0 && index <= spriteCollection.list.Count-1) {
			LoadDataValues ();
		}
		direction = 0;
	}

	private void LoadDataValues(){
		if (selectedSpriteIndex == -1) {
			return;
		}
		SpriteCollection.SpriteData spriteData = spriteCollection.list [selectedSpriteIndex];

		name = spriteData.name;

		//update textures
		textureBottom = spriteData.bottomTexture.texture;
		textureBottomRight = spriteData.bottomRightTexture.texture;
		textureRight = spriteData.rightTexture.texture;
		textureTopRight = spriteData.topRightTexture.texture;
		textureTop = spriteData.topTexture.texture;

		SpriteCollection.TextureData selectedTextureData = spriteData.bottomTexture;
		if (direction == 0) {
			selectedTextureData = spriteData.bottomTexture;
		} else if (direction == 1) {
			selectedTextureData = spriteData.bottomRightTexture;
		} else if (direction == 2) {
			selectedTextureData = spriteData.rightTexture;
		} else if (direction == 3) {
			selectedTextureData = spriteData.topRightTexture;
		} else if (direction == 4) {
			selectedTextureData = spriteData.topTexture;
		}

		gridSize = spriteData.gridSize;
		if (gridSize == 0) {
			gridTexture = null;
		} else if (gridSize == 1) {
			gridTexture = Resources.Load("grid_1x1", typeof(Texture2D)) as Texture2D;
		} else if (gridSize == 2) {
			gridTexture = Resources.Load("grid_2x2", typeof(Texture2D)) as Texture2D;
		} else if (gridSize == 3) {
			gridTexture = Resources.Load("grid_3x3", typeof(Texture2D)) as Texture2D;
		} else if (gridSize == 4) {
			gridTexture = Resources.Load("grid_4x4", typeof(Texture2D)) as Texture2D;
		} else if (gridSize == 5) {
			gridTexture = Resources.Load("grid_5x5", typeof(Texture2D)) as Texture2D;
		} else if (gridSize == 6) {
			gridTexture = Resources.Load("grid_6x6", typeof(Texture2D)) as Texture2D;
		}

		renderingLayer = (int)spriteData.renderingLayer;

		renderingOrder = spriteData.renderingOrder;
		renderingOrderString = spriteData.renderingOrder.ToString();


		offsetX = (int)selectedTextureData.offsetX;
		offsetXString = offsetX.ToString ();

		offsetY = (int)selectedTextureData.offsetY;
		offsetYString = offsetY.ToString ();

		scale = (int)selectedTextureData.scale;
		scaleString = scale.ToString ();


		numberOfColumns = (int)selectedTextureData.numberOfColumns;
		numberOfColumnsString = numberOfColumns.ToString ();

		numberOfRows = (int)selectedTextureData.numberOfRows;
		numberOfRowsString = numberOfRows.ToString ();

		framesCount = (int)selectedTextureData.framesCount;
		framesCountString = framesCount.ToString ();

		fps = (int)selectedTextureData.fps;
		fpsString = fps.ToString ();

	}

	void TreeViewMenu(Rect position, int selectedId){
		GenericMenu toolsMenu = new GenericMenu();

		if (selectedId == -1) {
			toolsMenu.AddItem (new GUIContent ("Create Sprite"), false, data => {
				TriggerMenuEvent ((string)data, selectedId);
			}, "Create Sprite");
		}


		if (selectedId > 0) {
			toolsMenu.AddItem (new GUIContent ("Delete Sprite"), false, data => {
				TriggerMenuEvent((string)data, selectedId);
			}, "Delete Sprite");
		}

		toolsMenu.DropDown(position);
		EditorGUIUtility.ExitGUI();
	}

	void TriggerMenuEvent(string evt, int selectedId){
		EnsureSpriteCollection ();

		switch (evt) {
		case "Create Sprite":
			if (selectedId == 1) {
				selectedId = -1;
			}
			spriteCollection.AddNewSprite ();
			AssetDatabase.SaveAssets ();
			_simpleTreeView.Reload ();
			_simpleTreeView.SetSelection (new List<int> (){ spriteCollection.list.Count });
			this.SelectSprite (spriteCollection.list.Count-1);
			break;

		case "Delete Sprite":
			spriteCollection.RemoveSprite (selectedId);
			if (spriteCollection != null && spriteCollection.list.Count > 0) {
				_simpleTreeView.Reload ();
			}
			break;
		}
	}
		

	public string name;
	private string _oldName = "";

	public Texture2D textureBottom;
	public Texture2D textureBottomRight;
	public Texture2D textureRight;
	public Texture2D textureTopRight;
	public Texture2D textureTop;

	private int offsetX = 0;
	private string offsetXString = "0";
	private int offsetY = 0;
	private string offsetYString = "0";
	private int scale = 100;
	private string scaleString = "100";

	private bool texturesFoldout;
	private string[] directionsOptions = new string[] {
		"Bottom", "Bottom Right", "Right", "Top Right", "Top"
	};
	private int direction = 0;
	private int _oldDirection = 0;

	private string[] gridSizeOptions = new string[] {
		"None", "1x1", "2x2", "3x3", "4x4", "5x5", "6x6"
	};
	private int gridSize = 4;
	private int _oldGridSize = 4;

	private string[] renderingLayerOptions = new string[] {
		"Ground", "Shadow", "Sprite"
	};
	private int renderingLayer = 2;
	private int _oldRenderingLayer = 2;

	private int renderingOrder = 0;
	private string renderingOrderString = "0";

	private bool animationFoldout;
	private int numberOfColumns = 1;
	private string numberOfColumnsString = "1";

	private int numberOfRows = 1;
	private string numberOfRowsString = "1";

	private int framesCount = 1;
	private string framesCountString = "1";

	private int fps = 1;
	private string fpsString = "1";

	void RenderRightPanel(){
		GUILayout.BeginArea(new Rect (position.width-WidthOfRightPanel, 0, WidthOfRightPanel, position.height));
		GUI.Box (new Rect (0, 0, WidthOfRightPanel, position.height),"", EditorStyles.helpBox);

		if (selectedSpriteIndex != -1) {

			rightPanelScrollPos = EditorGUILayout.BeginScrollView (rightPanelScrollPos, GUILayout.Width (WidthOfRightPanel), GUILayout.Height (position.height));

			GUILayout.BeginHorizontal ();
			GUILayout.Label ("Name");
			name = GUILayout.TextField (name);
			if (name != _oldName) {
				_oldName = name;
				this._simpleTreeView.Reload ();
			}
			GUILayout.EndHorizontal ();

			texturesFoldout = EditorGUILayout.Foldout (texturesFoldout, "Textures", true);
			if (texturesFoldout) {
				textureBottom = TextureObjectField ("Bottom", textureBottom);
				textureBottomRight = TextureObjectField ("Bottom Right", textureBottomRight);
				textureRight = TextureObjectField ("Right", textureRight);
				textureTopRight = TextureObjectField ("Top Right", textureTopRight);
				textureTop = TextureObjectField ("Top", textureTop);
			}

			GUILayout.Space (20);
			gridSize = EditorGUILayout.Popup ("Grid Size", gridSize, gridSizeOptions);
			if (gridSize != _oldGridSize) {
				_oldGridSize = gridSize;
				this.UpdateDataValues ();
				this.LoadDataValues ();
			}

			renderingLayer = EditorGUILayout.Popup ("Rendering layer", renderingLayer, renderingLayerOptions);
			if (renderingLayer != _oldRenderingLayer) {
				_oldRenderingLayer = renderingLayer;
				this.UpdateDataValues ();
				this.LoadDataValues ();
			}

			ChangableIntField ("Rendering order", ref renderingOrderString, ref renderingOrder, 0, 100);

			direction = EditorGUILayout.Popup ("Direction", direction, directionsOptions);
			if (direction != _oldDirection) {
				_oldDirection = direction;
				this.LoadDataValues ();
			}

			ChangableIntField ("Offset X", ref offsetXString, ref offsetX, -1000, 1000);
			ChangableIntField ("Offset Y", ref offsetYString, ref offsetY, -1000, 1000);
			ChangableIntField ("Scale", ref scaleString, ref scale, 0, 1000);

			GUILayout.Space (10);
			animationFoldout = EditorGUILayout.Foldout (animationFoldout, "Animation", true);
			if (animationFoldout) {
				ChangableIntField ("Number Of Columns", ref numberOfColumnsString, ref numberOfColumns, 1, 20);
				ChangableIntField ("Number Of Rows", ref numberOfRowsString, ref numberOfRows, 1, 20);
				ChangableIntField ("Frames Count", ref framesCountString, ref framesCount, 1, 100);
				ChangableIntField ("FPS", ref fpsString, ref fps, 1, 30);
			}

			EditorGUILayout.EndScrollView ();

			if (GUI.changed) {
				UpdateDataValues ();
			}
		}
		GUILayout.EndArea ();
	}

	private void UpdateDataValues(){

		if (selectedSpriteIndex == -1) {
			return;
		}
		SpriteCollection.SpriteData spriteData = spriteCollection.list [selectedSpriteIndex];

		spriteData.name = this.name;

		//update textures
		spriteData.bottomTexture.texture = textureBottom;
		spriteData.bottomRightTexture.texture = textureBottomRight;
		spriteData.rightTexture.texture = textureRight;
		spriteData.topRightTexture.texture = textureTopRight;
		spriteData.topTexture.texture = textureTop;

		SpriteCollection.TextureData selectedTextureData = spriteData.bottomTexture;
		if (direction == 0) {
			selectedTextureData = spriteData.bottomTexture;
		} else if (direction == 1) {
			selectedTextureData = spriteData.bottomRightTexture;
		} else if (direction == 2) {
			selectedTextureData = spriteData.rightTexture;
		} else if (direction == 3) {
			selectedTextureData = spriteData.topRightTexture;
		} else if (direction == 4) {
			selectedTextureData = spriteData.topTexture;
		}
			
		if (offsetX != selectedTextureData.offsetX) {
			selectedTextureData.offsetX = offsetX;
		}

		if (offsetY != selectedTextureData.offsetY) {
			selectedTextureData.offsetY = offsetY;
		}

		if (scale != selectedTextureData.scale) {
			selectedTextureData.scale = scale;
		}

		if (numberOfColumns != selectedTextureData.numberOfColumns) {
			selectedTextureData.numberOfColumns = numberOfColumns;
		}

		if (numberOfRows != selectedTextureData.numberOfRows) {
			selectedTextureData.numberOfRows = numberOfRows;
		}

		if (framesCount != selectedTextureData.framesCount) {
			selectedTextureData.framesCount = framesCount;
		}

		if (fps != selectedTextureData.fps) {
			selectedTextureData.fps = fps;
		}

		if (gridSize != spriteData.gridSize) {
			spriteData.gridSize = gridSize;
		}

		if (renderingLayer != (int)spriteData.renderingLayer) {
			spriteData.renderingLayer = (Common.RenderingLayer)renderingLayer;
		}

		if (renderingOrder != spriteData.renderingOrder) {
			spriteData.renderingOrder = renderingOrder;
		}

		EditorUtility.SetDirty(spriteCollection);
	}

	private Texture2D TextureObjectField(string name, Texture2D texture){
		Texture2D outTexture;
		GUILayout.BeginHorizontal ();
		GUILayout.Space (20);
		outTexture = (Texture2D)EditorGUILayout.ObjectField (
			name,
			texture,
			typeof(Texture2D));
		GUILayout.EndHorizontal ();
		return outTexture;
	}

	private Vector2 defaultGridSize = new Vector2 (512, 512);
	private Vector2 defaultImgSize = new Vector2 (256, 256);
	void RenderPreviewArea(){

		defaultImgSize.x = 256 * scale / 100;
		defaultImgSize.x = 256 * scale / 100;

		if(gridTexture == null){
			gridTexture = Resources.Load("grid", typeof(Texture2D)) as Texture2D;
		}

		//DRAW GRID
		Vector2 centerPointOfPreviewArea = new Vector2 ((WidthOfLetPanel + position.width - WidthOfRightPanel) / 2, position.height / 2);
		GUI.Label(new Rect (centerPointOfPreviewArea.x-defaultGridSize.x/2, centerPointOfPreviewArea.y-defaultGridSize.y/2, defaultGridSize.x, defaultGridSize.y), gridTexture);

		//DRAW ITEM SPRITE
		if (selectedSpriteIndex != -1) {
			Texture2D texture = null;
			if (direction == 0) {
				texture = textureBottom;
			} else if (direction == 1) {
				texture = textureBottomRight;
			} else if (direction == 2) {
				texture = textureRight;
			} else if (direction == 3) {
				texture = textureTopRight;
			} else if (direction == 4) {
				texture = textureTop;
			}

			if (texture != null) {
				
				float heightFactor = ((float)texture.height / (float)texture.width) * ((float)numberOfColumns/numberOfRows);

				float upFactor = Mathf.Cos (Mathf.Deg2Rad * 45) * gridSize / 2.0f;

				float x = (centerPointOfPreviewArea.x - defaultImgSize.x / 2) + offsetX;
				float y = (centerPointOfPreviewArea.y - defaultImgSize.x * heightFactor / 2) - offsetY;

				float framePaddingX = defaultImgSize.x * (currentFrame % numberOfColumns);
				float framePaddingY = defaultImgSize.x * heightFactor * (currentFrame / numberOfColumns);

				Rect imgRect = new Rect (x, y, defaultImgSize.x, defaultImgSize.x * heightFactor);

				GUI.BeginGroup(imgRect);
				GUI.DrawTexture (new Rect (-framePaddingX, -framePaddingY, defaultImgSize.x*numberOfColumns, defaultImgSize.x*numberOfRows * heightFactor ), texture);
				GUI.EndGroup();

			}
		}
	}

	private int currentFrame = 0;
	private float lastUpdatedTime = 0.0f;
	public void UpdateAnimationViewFrames(){
		if (framesCount > 1) {

			if (Time.realtimeSinceStartup - lastUpdatedTime >= 1.0f / fps) {
				lastUpdatedTime = Time.realtimeSinceStartup;
				Repaint ();
				currentFrame++;
				if (currentFrame >= framesCount) {
					currentFrame = 0;
				}
			}
		} else {
			currentFrame = 0;
		}
	}

	void ChangableIntField(string label, ref string valueString, ref int valueInt, int min, int max){

		GUILayout.BeginHorizontal ();
		valueString = EditorGUILayout.TextField(label, valueString);
		if (GUI.changed) {
			int oldValueInt = valueInt;
			if(!int.TryParse(valueString, out valueInt)){
				valueInt = oldValueInt;
				valueString = valueInt.ToString ();
			}

			if (valueInt < min) {
				valueInt = min;
				valueString = valueInt.ToString ();
			}

			if (valueInt > max) {
				valueInt = max;
				valueString = valueInt.ToString ();
			}
		}

		if (GUILayout.Button ("-")) {
			valueInt--;
			if (valueInt < min) {
				valueInt = min;
			}
			valueString = valueInt.ToString ();
		};

		if (GUILayout.Button ("+")) {
			valueInt++;
			if (valueInt > max) {
				valueInt = max;
			}
			valueString = valueInt.ToString ();	
		};

		GUILayout.EndHorizontal ();
	}
}
