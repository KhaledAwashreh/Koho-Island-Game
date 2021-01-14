using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainTroopsWindowScript : WindowScript {
	public static TrainTroopsWindowScript instance;

	/* prefabs */
	public GameObject TrainingQueueTroopItem;

    /* object references */
	public ProgressPanelScript GoldInfo;
    public ProgressPanelScript ElixirInfo;
    public ProgressPanelScript DiamondInfo;

	public GameObject TrainingTroopsContainer;

	/* private vars */
	private BaseItemScript _selectedBarrack;

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
		this._selectedBarrack = SceneManager.instance.selectedItem;
        this.RenderTrainingQueue();
		this.UpdateResourcePanel();
    }

	public void RenderTrainingQueue()
	{
		this.ClearTrainingQueueTroopItems();

		int previous = -1;
		int index = 0;
		TrainingQueueTroopItemCtrl previousComponent = null;
		foreach(int troopId in _selectedBarrack.Training.trainingQueue)
		{
			TrainingQueueTroopItemCtrl comp = null;
			if (troopId == previous)
				comp = previousComponent;

			if (comp == null)
			{
				comp = _CreateTrainingQueueTroopItem();
				comp.SetId(troopId);
				comp.SetCount(1);
			}
			else
			{
				comp.IncrementCount();
			}
			comp.SetIndex(index);

			previous = troopId;
			previousComponent = comp;

			if (index == 0)
				comp.StartProgress(_selectedBarrack.Training.trainingStartTime);
			index++;
		}
	}

    public void UpdateResourcePanel()
	{
		this.GoldInfo.hasMaxValue = false;
        this.GoldInfo.value = SceneManager.instance.numberOfGoldInStorage;

		this.ElixirInfo.hasMaxValue = false;
        this.ElixirInfo.value = SceneManager.instance.numberOfElixirInStorage;

        this.DiamondInfo.hasMaxValue = false;
        this.DiamondInfo.value = SceneManager.instance.numberOfDiamondsInStorage;
	}

    public void AddToTrainingQueue(int troopId)
	{
		ItemsCollection.ItemData data = Items.GetItem(troopId);
		if (SceneManager.instance.ConsumeResource("elixir", 10))
		{
			_selectedBarrack.Training.AddToTrainingQueue(troopId);
			this.RenderTrainingQueue();
		}
	}

	public void RemoveFromTrainingQueue(int troopId)
    {
		_selectedBarrack.Training.RemoveFromTrainingQueue(troopId);
        this.RenderTrainingQueue();
    }

	private TrainingQueueTroopItemCtrl _CreateTrainingQueueTroopItem()
	{
		return Utilities.CreateInstance(this.TrainingQueueTroopItem, this.TrainingTroopsContainer, true).GetComponent<TrainingQueueTroopItemCtrl>();
	}

    private void ClearTrainingQueueTroopItems()
	{
		foreach(Transform child in this.TrainingTroopsContainer.transform)
		{
			Destroy(child.gameObject);
		}
	}
}
