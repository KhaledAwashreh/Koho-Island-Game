using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DefenderScript : MonoBehaviour
{

	private BaseItemScript _baseItem;
	private BaseItemScript _currentTarget;
	private Vector3 _currentTargetPoint;

	public void SetData(BaseItemScript baseItem)
	{
		this._baseItem = baseItem;
		this.SearchForAttacker();
	}

	private IEnumerator _lastCoroutine = null;
	public void SearchForAttacker()
	{
		//		return;
		if (_lastCoroutine != null)
		{
			this.StopCoroutine(_lastCoroutine);
			this._lastCoroutine = null;
		}

		_lastCoroutine = _SearchForAttacker();
		this.StartCoroutine(_lastCoroutine);
	}

	private IEnumerator _SearchForAttacker()
	{
		yield return new WaitForSeconds(0.5f);
		if (!this._baseItem.isDestroyed)
		{
			this._baseItem.SetState(Common.State.IDLE);
			this.Defend();
		}
	}

	public void Defend()
	{
		BaseItemScript target = _GetNearestTargetItem();

		if (target != null)
		{
			this._currentTarget = target;
			this._currentTargetPoint = target.GetPosition();

			this.DefendLoop();
		}
		else
		{
			this.SearchForAttacker();
		}
	}

	public void DefendLoop()
	{
		if (this._currentTarget.healthPoints > 0)
		{
			this._baseItem.SetState(Common.State.ATTACK);
			this._baseItem.LookAt(this._currentTarget);
			this.StartCoroutine(_HitTarget());
		}
		else
		{
			this.SearchForAttacker();
		}
	}


	private IEnumerator _HitTarget()
	{
		yield return new WaitForSeconds(1.0f);
		if (!this._baseItem.isDestroyed)
		{
			this._currentTarget.OnReceiveHit(this._baseItem);
			this.DefendLoop();
		}
	}

	private BaseItemScript _GetNearestTargetItem()
	{
		BaseItemScript nearestItem = null;
		float nearestDistance = 999999;

		foreach (BaseItemScript item in SceneManager.instance.GetAllItems())
		{
			if(this._baseItem.ownedItem == item.ownedItem)
			{
				continue;
			}

			if (item == this._baseItem)
			{
				continue;
			}

			if (!item.itemData.configuration.isCharacter)
			{
				continue;
			}

			if (item.healthPoints == 0)
			{
				continue;
			}

			float d = Vector3.Distance(item.GetPosition(), this._baseItem.GetPosition());
			if (d <= this._baseItem.itemData.configuration.defenceRange)
			{
				nearestDistance = d;
				nearestItem = item;
			}
		}

		return nearestItem;
	}

}
