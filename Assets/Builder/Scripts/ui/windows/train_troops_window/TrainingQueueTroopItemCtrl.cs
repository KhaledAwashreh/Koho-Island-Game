using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrainingQueueTroopItemCtrl : MonoBehaviour {

	public Sprite ArcherThumb;
	public Sprite SwordManThumb;

	/* object references */
	public GameObject ProgressPanel;
	public Text CountText;
	public Text TimeText;
	public Image TroopImage;

	/* private variables */
	private int _troopId;
	private int _troopIndex;
	private int _count;

	public void SetId(int troopId)
	{
		this._troopId = troopId;
		ItemsCollection.ItemData itemData = Items.GetItem(troopId);
		if (itemData.name == "SwordMan")
			this.TroopImage.sprite = SwordManThumb;
		else if(itemData.name == "Archer")
			this.TroopImage.sprite = ArcherThumb;

		this.ProgressPanel.SetActive(false);

	}

	public void SetIndex(int index)
    {
		this._troopIndex = index;
    }

    public void SetCount(int count)
	{
		this._count = count;
		this.CountText.text = count.ToString() + "x";
	}

	public void IncrementCount()
	{
		this._count++;
		this.SetCount(this._count);
	}

    public void OnClickRemoveButton()
	{
		RemoveFromQueue();
	}

	public void RemoveFromQueue()
	{
		TrainTroopsWindowScript.instance.RemoveFromTrainingQueue(this._troopIndex);
	}

    public void StartProgress(float startTime)
	{      
		ItemsCollection.ItemData itemData = Items.GetItem(this._troopId);
		this.StartCoroutine(_StartProgress(startTime, startTime + itemData.configuration.buildTime));
		this.ProgressPanel.SetActive(true);
	}

	private float _startTime;
	private float _finishTime;
	private IEnumerator _StartProgress(float startTime, float finishTime)
	{
		this._startTime = startTime;
		this._finishTime = finishTime;

		while(Time.realtimeSinceStartup < finishTime)
		{
			float remainingTime = finishTime - Time.realtimeSinceStartup;

			int minutes = (int)(remainingTime / 60);
			int seconds = (int)remainingTime % 60;
			this.TimeText.text = minutes.ToString() + "m " + seconds.ToString() + "s";

			yield return new WaitForSeconds(1.0f);
		}
		RemoveFromQueue();
	}
}
