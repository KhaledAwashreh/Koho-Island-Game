using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseItemProgressUIScript : MonoBehaviour
{

	/* object references */
	public Transform ProgressContainer;

	public TextMesh TimerLabel;
	public TextMesh TimerLabelShadow;

	public SpriteRenderer ProgressFiller;

	/* private vars */
	private BaseItemScript _baseItem;
	private float _buildTime;
	private float _buildStartTime;
	private float _fillerFullLength;

	void Start()
	{
		this._baseItem = this.GetComponentInParent<BaseItemScript>();
		this._fillerFullLength = this.ProgressFiller.size.x;

		Vector3 baseSize = this._baseItem.GetSize();
		this.ProgressContainer.localScale = this.ProgressContainer.localScale / baseSize.x;

		this.Init();
	}

	void Update()
	{
		this.UpdateProgress();
	}

	public void Init()
	{
		this._buildTime = this._baseItem.itemData.configuration.buildTime;
		this._buildStartTime = Time.time;
	}

	private Vector2 _tempSize;
	public void UpdateProgress()
	{
		if (this._buildTime <= 0)
		{
			this.OnFinishBuild();
			return;
		}

		float elapsedTime = Time.time - this._buildStartTime;
		float progress = elapsedTime / this._buildTime;

		_tempSize.x = progress * this._fillerFullLength;
		_tempSize.y = this.ProgressFiller.size.y;
		this.ProgressFiller.size = _tempSize;

		int timeToFinish = (int)(_buildTime - elapsedTime);
		TimerLabel.text = timeToFinish.ToString();
		TimerLabelShadow.text = timeToFinish.ToString();

		if (progress >= 1)
		{
			this.OnFinishBuild();
		}
	}

	public void SetFillerColor(Color color)
	{

	}

	public void OnFinishBuild()
	{
		this._baseItem.UI.ShowProgressUI(false);
	}
}
