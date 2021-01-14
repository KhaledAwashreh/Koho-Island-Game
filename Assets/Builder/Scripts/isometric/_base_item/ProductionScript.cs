using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ProductionScript : MonoBehaviour
{
	/* public vars */
	public bool readyForCollection = false;
	public int collectedAmount = 0;

    /* private vars */
	private BaseItemScript _baseItem;
	private float _productionRate = 0.0f;
	private string _productType;
	private float _lastCollectedTime = 0.0f;

       
	public void SetData(BaseItemScript baseItem)
	{
		this._baseItem = baseItem;
		this._productionRate = baseItem.itemData.configuration.productionRate;
		this._productType = baseItem.itemData.configuration.product;
		this._lastCollectedTime = Time.realtimeSinceStartup;
	}
       
	/// <summary>
	/// Update on walker. which call every frame if _isWalking is true
	/// </summary>
	public void UpdateProduction()
	{
		float time = Time.realtimeSinceStartup - this._lastCollectedTime;
		int productAmount = (int)((time / 60 / 60) * this._productionRate);
		if (productAmount >= 1 && !readyForCollection)
		{
			this.readyForCollection = true;
			this._baseItem.UI.ShowCollectNotificationUI(true, this._productType);
		}
	}

	public void Collect()
	{
		float time = Time.realtimeSinceStartup - this._lastCollectedTime;
        int productAmount = (int)((time / 60 / 60) * this._productionRate);
		if(productAmount > 0)
		{
			this._baseItem.Particles.ShowCollectionParticle(this._productType);
			this._baseItem.UI.ShowCollectNotificationUI(false, this._productType);
			this._lastCollectedTime = Time.realtimeSinceStartup;
			this.readyForCollection = false;

			SceneManager.instance.CollectResource(this._productType, 100);

			SoundManager.instance.PlaySound(SoundManager.instance.Collect, false);
		}
	}
    
}
