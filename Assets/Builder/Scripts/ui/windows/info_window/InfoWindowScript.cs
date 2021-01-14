using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoWindowScript : WindowScript {
	public static InfoWindowScript instance;

	/* prefabs */
	public GameObject InfoItem;

	/* object references */
	public Text Title;
	public RawImage ThumbImage;
	public GameObject InfoPanel;

    
	/* private vars */
	private BaseItemScript _baseItem;

	void Awake()
    {
		instance = this;
		if (SceneManager.instance == null)
			return;
		
        this.Init();
    }

	private void OnDestroy()
	{
		instance = null;
	}

	public void Init()
    {
		this._baseItem = SceneManager.instance.selectedItem;
		this.RenderInfo();
    }

	public void RenderInfo()
	{
		this.Title.text = this._baseItem.itemData.name;
		this.ThumbImage.texture = this._baseItem.itemData.thumb;

		bool isCharacter = this._baseItem.itemData.configuration.isCharacter;

		if (!isCharacter)
		{
			//GRID SIZE
			string gridSize = this._baseItem.itemData.gridSize.ToString() + "x" + this._baseItem.itemData.gridSize.ToString();
			this._CreateInfoItem("Grid Size", gridSize);
		}

		string buildTime = this._baseItem.itemData.configuration.buildTime.ToString() + "s";
		this._CreateInfoItem("Build Time", buildTime);

		if(this._baseItem.itemData.configuration.speed > 0)
		{
			string speed = this._baseItem.itemData.configuration.speed.ToString();
			this._CreateInfoItem("Speed", speed);
		}

		if (this._baseItem.itemData.configuration.attackRange > 0)
        {
			string attackRange = this._baseItem.itemData.configuration.attackRange.ToString();
			this._CreateInfoItem("Attack Range", attackRange);
        }

		if (this._baseItem.itemData.configuration.defenceRange > 0)
        {
			string defenceRange = this._baseItem.itemData.configuration.defenceRange.ToString();
            this._CreateInfoItem("Defence Range", defenceRange);
        }

	
		string healthPoints = this._baseItem.itemData.configuration.healthPoints.ToString();
		this._CreateInfoItem("Health Points", healthPoints);

		if (this._baseItem.itemData.configuration.hitPoints > 0)
		{
			string hitPoints = this._baseItem.itemData.configuration.hitPoints.ToString();
			this._CreateInfoItem("Hit Points", hitPoints);
		}

		if (this._baseItem.itemData.configuration.productionRate > 0)
        {
			string productionRate = this._baseItem.itemData.configuration.productionRate.ToString();
			this._CreateInfoItem("Production Rate", productionRate);

			string product = this._baseItem.itemData.configuration.product;
			this._CreateInfoItem("Product", product);
        }
	}

	private void _CreateInfoItem(string property, string value)
	{
		InfoItemCtrl comp = Utilities.CreateInstance(this.InfoItem, this.InfoPanel, true).GetComponent<InfoItemCtrl>();
		comp.SetData(property, value);
	}
 
}
