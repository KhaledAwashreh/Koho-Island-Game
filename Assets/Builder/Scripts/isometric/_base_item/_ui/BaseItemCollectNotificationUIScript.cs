using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseItemCollectNotificationUIScript : MonoBehaviour
{
    /* object references */
    public Transform Container;
    public GameObject GoldIcon;
    public GameObject ElixirIcon;
    public GameObject WoodIcon;
    public GameObject LeavesIcon;

    /* private vars */
    private BaseItemScript _baseItem;


    void Awake()
    {
        this._baseItem = this.GetComponentInParent<BaseItemScript>();
        if (this._baseItem == null)
        {
            return;
        }

        Vector3 baseSize = this._baseItem.GetSize();
        this.Container.localScale = this.Container.localScale / baseSize.x;
    }

    public void SetIcon(string type)
    {
        this.GoldIcon.SetActive(type == "gold");
        this.ElixirIcon.SetActive(type == "elixir");
        this.LeavesIcon.SetActive(type == "leaves");
        this.WoodIcon.SetActive(type == "wood");
    }
}