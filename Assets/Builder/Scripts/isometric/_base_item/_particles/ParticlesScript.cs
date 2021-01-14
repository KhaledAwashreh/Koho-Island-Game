using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesScript : MonoBehaviour
{
    /* prefabs */
    public GameObject GoldCollectionParticle;
    public GameObject ElixirCollectionParticle;
    public GameObject DestructionParticle;
    public GameObject LeavesCollectionParticle;
    public GameObject WoodCollectionParticle;

    private BaseItemScript _baseItem;

    public void SetData(BaseItemScript baseItem)
    {
        this._baseItem = baseItem;
    }


    public void ShowCollectionParticle(string type)
    {
        Vector3 position = this._baseItem.GetPosition() + this._baseItem.GetSize() / 2;
        if (type == "gold")
            SceneManager.instance.ShowParticle(this.GoldCollectionParticle, position);
        else if (type == "elixir")
            SceneManager.instance.ShowParticle(this.ElixirCollectionParticle, position);
        else if (type == "leaves")
            SceneManager.instance.ShowParticle(this.LeavesCollectionParticle, position);
    }

    public void ShowDestructionParticle()
    {
        Vector3 position = this._baseItem.GetPosition() + this._baseItem.GetSize() / 2;
        SceneManager.instance.ShowParticle(this.DestructionParticle, position);
    }
}