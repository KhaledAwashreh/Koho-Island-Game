using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AttackerScript : MonoBehaviour
{

	private BaseItemScript _baseItem;
	private BaseItemScript _currentTarget;
	private Vector3 _currentTargetPoint;

	public void SetData(BaseItemScript baseItem)
	{
		this._baseItem = baseItem;
	}

	public void AttackNearestTarget()
	{
		BaseItemScript target = _GetNearestTargetItem();
		if (target != null)
		{
			this.Attack(target);
		}
		else
		{
			int t = 0;
		}
	}

	public void Attack(BaseItemScript target)
	{
		this._baseItem.Walker.OnBetweenWalk = null;

		if (target != null)
		{
			this._currentTarget = target;

			Vector3[] outerCells = target.GetOuterCells();
			if (outerCells.Length > 0)
			{
				//finding nearest outer cell
				Vector3 nearestCell = Vector3.one * 999999;
				float smallerDistance = 999999;
				for (int index = 0; index < outerCells.Length; index++)
				{
					float distance = Vector3.Distance(this._baseItem.GetPosition(), outerCells[index]);
					if (distance < smallerDistance)
					{
						smallerDistance = distance;
						nearestCell = outerCells[index];
					}
				}

				this._currentTargetPoint = nearestCell;

				this._baseItem.Walker.WalkToPosition(_currentTargetPoint);
				this._baseItem.Walker.OnFinishWalk += this.AttackLoop;

				//check any walls on root
				BaseItemScript nearestWallOnPath = this._baseItem.Walker.GetNearestWallOnPath();
				if (nearestWallOnPath != null)
				{
					this._currentTarget = nearestWallOnPath;
					//					Debug.Log (nearestWallOnPath);
				}

			}

			if (this._baseItem.itemData.configuration.attackRange > 0)
			{
				this._baseItem.Walker.OnBetweenWalk += this.CheckTargetBeyondRange;
				//check on start walk because the unit will already in range of target maybe
				this.CheckTargetBeyondRange();
			}
		}
	}


	public void AttackLoop()
	{
		if (this._baseItem.isDestroyed)
		{
			return;
		}

		if (this._currentTarget.healthPoints > 0)
		{
			this._baseItem.LookAt(this._currentTarget);
			this._baseItem.SetState(Common.State.ATTACK);
			this.StartCoroutine(_HitTarget());
		}
		else
		{
			this._baseItem.SetState(Common.State.IDLE);
			this._baseItem.Walker.OnFinishWalk -= this.AttackLoop;
			this.AttackNearestTarget(); //start again
		}
	}

	public void CheckTargetBeyondRange()
	{
		//		Debug.Log ("CheckTargetBeyondRange");
		if (Vector3.Distance(_currentTarget.GetCenterPosition(), this._baseItem.GetPosition()) <= this._baseItem.itemData.configuration.attackRange)
		{
			this._baseItem.Walker.CancelWalk();
			this.AttackLoop();
		}
	}

	public void CheckWallsOnRoot()
	{
		//		Debug.Log ("CheckWallsOnRoot");
		if (Vector3.Distance(_currentTarget.GetCenterPosition(), this._baseItem.GetPosition()) <= this._baseItem.itemData.configuration.attackRange)
		{
			this._baseItem.Walker.CancelWalk();
			this.AttackLoop();
		}
	}

	private IEnumerator _HitTarget()
	{
		yield return new WaitForSeconds(1.0f);
		if (!this._baseItem.isDestroyed)
		{
			this._currentTarget.OnReceiveHit(this._baseItem);
			this.AttackLoop();

			this._PlayHitSound();
		}
	}

	private void _PlayHitSound()
	{
		AudioClip clip = null;
		if (this._baseItem.itemData.name == "SwordMan")
			clip = SoundManager.instance.Sword;
		else if (this._baseItem.itemData.name == "Archer" || this._baseItem.itemData.name == "ArcherTower")
			clip = SoundManager.instance.Bow;

		SoundManager.instance.PlaySound(clip, false);
	}

	private BaseItemScript _GetNearestTargetItem()
	{
		BaseItemScript nearestItem = null;
		float nearestDistance = 999999;

		foreach (BaseItemScript item in SceneManager.instance.GetAllItems())
		{
			if (item == this._baseItem)
			{
				continue;
			}

			if (item.itemData.configuration.isCharacter)
			{
				continue;
			}

			if (item.isDestroyed)
			{
				continue;
			}

			if (item.itemData.name == "Wall")
			{
				continue;
			}

			float d = Vector3.Distance(item.GetPosition(), this._baseItem.GetPosition());
			if (d < nearestDistance)
			{
				nearestDistance = d;
				nearestItem = item;
			}
		}

		return nearestItem;
	}

}
