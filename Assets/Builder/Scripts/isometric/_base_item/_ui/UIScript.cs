using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIScript : MonoBehaviour {

    /* prefabs */
	public GameObject BaseItemSelectionUI;
    public GameObject BaseItemProgressUI;
    public GameObject BaseItemEnergyBarUI;
    public GameObject BaseItemCollectNotificationUI;

    /* objects */
	[HideInInspector]
	public BaseItemSelectionUIScript selectionUIInstance;
	[HideInInspector]
	public BaseItemProgressUIScript progressUIInstance;
	[HideInInspector]
	public BaseItemEnergyBarUIScript energyBarUIInstance;
	[HideInInspector]
	public BaseItemCollectNotificationUIScript collectNotificationUIInstance;

	private BaseItemScript _baseItem;



	public void SetData(BaseItemScript baseItem)
    {
        this._baseItem = baseItem;
    }

    public void ShowSelectionUI(bool isTrue)
    {
        if (isTrue)
        {
            if (this.selectionUIInstance == null)
            {
                this.selectionUIInstance = this.ShowUI(this.BaseItemSelectionUI).GetComponent<BaseItemSelectionUIScript>();
            }
        }
        else
        {
            if (this.selectionUIInstance != null)
            {
                Destroy(this.selectionUIInstance.gameObject);
                this.selectionUIInstance = null;
            }
        }
    }

    public void ShowProgressUI(bool isTrue)
    {
        if (isTrue)
        {
            if (this.progressUIInstance == null)
            {
				this.progressUIInstance = this.ShowUI(this.BaseItemProgressUI).GetComponent<BaseItemProgressUIScript>();
            }
        }
        else
        {
            if (this.progressUIInstance != null)
            {
                Destroy(this.progressUIInstance.gameObject);
                this.progressUIInstance = null;
            }
        }
    }

    public void ShowEnergyBarUI(bool isTrue)
    {
        if (isTrue)
        {
            if (this.energyBarUIInstance == null)
            {
				this.energyBarUIInstance = this.ShowUI(this.BaseItemEnergyBarUI).GetComponent<BaseItemEnergyBarUIScript>();
            }
        }
        else
        {
            if (this.energyBarUIInstance != null)
            {
                Destroy(this.energyBarUIInstance.gameObject);
                this.energyBarUIInstance = null;
            }
        }
    }

    public void ShowCollectNotificationUI(bool isTrue, string type)
    {
        if (isTrue)
        {
            if (this.collectNotificationUIInstance == null)
            {
				this.collectNotificationUIInstance = this.ShowUI(this.BaseItemCollectNotificationUI).GetComponent<BaseItemCollectNotificationUIScript>();
                this.collectNotificationUIInstance.SetIcon(type);
            }
        }
        else
        {
            if (this.collectNotificationUIInstance != null)
            {
                Destroy(this.collectNotificationUIInstance.gameObject);
                this.collectNotificationUIInstance = null;
            }
        }
    }

    public GameObject ShowUI(GameObject prefab)
    {
        GameObject inst = Utilities.CreateInstance(prefab, this._baseItem.UIRoot, true);
        inst.transform.localPosition = Vector3.zero;
        return inst;
    }
}
