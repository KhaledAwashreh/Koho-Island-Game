using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WalkerScript : MonoBehaviour
{

	public Action OnFinishWalk;
	public Action OnBetweenWalk;

	private BaseItemScript _baseItem;
	private float _speed = 5f;
	private Vector3 _targetPosition;

	private bool _isWalking = false;

	public void SetData(BaseItemScript baseItem)
	{
		this._baseItem = baseItem;
		this._speed = baseItem.itemData.configuration.speed;
	}

	void Update()
	{
		if (!this._isWalking)
		{
			return;
		}

		this.WalkUpdate();
	}

	/// <summary>
	/// Update on walker. which call every frame if _isWalking is true
	/// </summary>
	public void WalkUpdate()
	{
		float frameDistance = Time.deltaTime * _speed;
		float interpolationValue = frameDistance / (_targetPosition - transform.localPosition).magnitude;
		transform.localPosition = Vector3.Lerp(transform.localPosition, _targetPosition, interpolationValue);

		if (transform.localPosition == this._targetPosition)
		{
			this._WalkNextNode();
		}
	}

	/// <summary>
	/// Finishs the walk.
	/// </summary>
	public void FinishWalk()
	{
		//		Debug.Log ("FinishWalk");
		this._baseItem.SetState(Common.State.IDLE);

		this._isWalking = false;
		this._targetPosition = transform.position;

		if (this.OnFinishWalk != null)
		{
			this.OnFinishWalk.Invoke();
		}

		OnBetweenWalk = null;
	}

	/// <summary>
	/// Moves to position.
	/// </summary>
	/// <param name="position">Position.</param>
	public void MoveToPosition(Vector3 position)
	{
		this._isWalking = true;
		this._targetPosition = position;
		this._baseItem.LookAt(position);
	}

	/// <summary>
	/// Walks to position.
	/// </summary>
	/// <param name="position">Position.</param>
	public void WalkToPosition(Vector3 position)
	{
		this.CancelWalk();

		if (SceneManager.instance.gameMode == Common.GameMode.NORMAL)
		{
			//on user city walls are not considered
			this.WalkThePath(GroundManager.instance.GetPath(this.transform.position, position, false));

		}
		else if (SceneManager.instance.gameMode == Common.GameMode.ATTACK)
		{
			GroundManager.Path pathWithoutWalls = GroundManager.instance.GetPath(this.transform.position, position, false);
			GroundManager.Path pathWithWalls = GroundManager.instance.GetPath(this.transform.position, position, true);

			//			Debug.Log (pathWithWalls.GetDistanceAlongPath ());
			//			Debug.Log (pathWithoutWalls.GetDistanceAlongPath ());

			float difference = pathWithWalls.GetDistanceAlongPath() - pathWithoutWalls.GetDistanceAlongPath();
			if (difference > 0 && difference <= 4)
			{
				this.WalkThePath(pathWithWalls);
			}
			else
			{
				this.WalkThePath(pathWithoutWalls);
			}
		}
	}

	private GroundManager.Path _path;
	private int _currentNodeIndex;
	/// <summary>
	/// Walks the path.
	/// </summary>
	/// <param name="path">Path.</param>
	public void WalkThePath(GroundManager.Path path)
	{
		if (path.nodes == null || path.nodes.Length == 0)
		{
			//no path found
			this.FinishWalk();
			return;
		}

		this._baseItem.SetState(Common.State.WALK);

		this._path = path;
		this._currentNodeIndex = 0;
		if (path != null && path.nodes != null && path.nodes.Length > 0)
		{
			this.MoveToPosition(this._path.nodes[0]);
		}
	}

	private void _WalkNextNode()
	{
		if (this._path != null && this._path.nodes != null && this._currentNodeIndex < this._path.nodes.Length - 1)
		{
			this._currentNodeIndex++;
			this.MoveToPosition(this._path.nodes[this._currentNodeIndex]);
			if (this.OnBetweenWalk != null)
			{
				this.OnBetweenWalk.Invoke();
			}
		}
		else
		{
			this.FinishWalk();
		}
	}

	public void CancelWalk()
	{
		this._isWalking = false;
		this._targetPosition = transform.position;
		this.OnFinishWalk = null;
		this.OnBetweenWalk = null;
	}

	public BaseItemScript GetNearestWallOnPath()
	{
		if (this._path != null && this._path.nodes != null && this._path.nodes.Length > 0)
		{

			for (int index = 0; index < this._path.nodes.Length; index++)
			{
				BaseItemScript item = GroundManager.instance.GetItemInPosition(this._path.nodes[index]);
				if (item != null && !item.isDestroyed && item.itemData.name == "Wall")
				{
					return item;
				}
			}
		}

		return null;
	}

}
