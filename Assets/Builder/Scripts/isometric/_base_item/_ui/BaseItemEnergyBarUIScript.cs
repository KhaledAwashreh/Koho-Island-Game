using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseItemEnergyBarUIScript : MonoBehaviour
{

	/* object references */
	public Transform ProgressContainer;

	public SpriteRenderer ProgressFiller;

	/* private vars */
	private BaseItemScript _baseItem;
	private float _buildTime;
	private float _buildStartTime;
	private float _fillerFullLength;

	void Awake()
	{
		this._baseItem = this.GetComponentInParent<BaseItemScript>();
		if (this._baseItem == null)
		{
			return;
		}

		this._fillerFullLength = this.ProgressFiller.size.x;

		Vector3 baseSize = this._baseItem.GetSize();
		this.ProgressContainer.localScale = this.ProgressContainer.localScale / baseSize.x;
	}

	private Vector2 _tempSize;
	public void SetProgress(float progress)
	{
		_tempSize.x = progress * this._fillerFullLength;
		_tempSize.y = this.ProgressFiller.size.y;
		this.ProgressFiller.size = _tempSize;

		if (progress >= 0.8f)
		{
			this.SetFillerColor(Color.green);
		}
		else if (progress >= 0.5f)
		{
			this.SetFillerColor(Color.yellow);
		}
		else
		{
			this.SetFillerColor(Color.red);
		}
	}

	public void SetFillerColor(Color color)
	{
		this.ProgressFiller.color = color;
	}

}
