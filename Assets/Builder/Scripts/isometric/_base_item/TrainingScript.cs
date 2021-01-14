using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingScript : MonoBehaviour {
	private BaseItemScript _baseItem;
    
    /* private vars */
	public List<int> trainingQueue;
	public float trainingStartTime;

	public void SetData(BaseItemScript baseItem)
    {
        this._baseItem = baseItem;
    }

	//TRAINING QUEUE
    private int _currentTrainingTroop = -1; //-1 means null
	public void AddToTrainingQueue(int troopId)
    {
		trainingQueue.Add(troopId);

		if (_currentTrainingTroop == -1)
        {
			_Train(troopId);
        }
    }

    private void _Train(int troopId)
	{
		_currentTrainingTroop = troopId;
		_startTrainingCoroutine = StartCoroutine(_StartTraining());
	}

	private Coroutine _startTrainingCoroutine;
	private IEnumerator _StartTraining()
	{
		ItemsCollection.ItemData itemData = Items.GetItem(_currentTrainingTroop);
		float buildTime = itemData.configuration.buildTime;

		trainingStartTime = Time.realtimeSinceStartup;
		yield return new WaitForSeconds(buildTime);

		Vector3 randomFrontCell = _baseItem.GetRandomFrontCellPosition();
		BaseItemScript newUnit = SceneManager.instance.AddItem(_currentTrainingTroop, -1, (int)randomFrontCell.x, (int)randomFrontCell.z, true, true);
		BaseItemScript nearestArmyCamp = SceneManager.instance.GetNearestArmyCamp(newUnit.GetPosition());
		if (nearestArmyCamp != null)
		{
			newUnit.WalkRandom(nearestArmyCamp);
		}

		_currentTrainingTroop = -1;
		trainingQueue.RemoveAt(0);

		if (trainingQueue.Count > 0)
			_Train(trainingQueue[0]);

		if(TrainTroopsWindowScript.instance != null)
		    TrainTroopsWindowScript.instance.RenderTrainingQueue();
	}


	public void RemoveFromTrainingQueue(int index)
    {
		//_currentTrainingTroop = -1;
        trainingQueue.RemoveAt(index);
        
		if (index == 0)
        {
			StopCoroutine(_startTrainingCoroutine);
			_startTrainingCoroutine = null;
         
			if (trainingQueue.Count > 0)
				_Train(trainingQueue[0]);
        }
    }
}
