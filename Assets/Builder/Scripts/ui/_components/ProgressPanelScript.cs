using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressPanelScript : MonoBehaviour {
	/* object references */
	public RectTransform Filler;
	public Text ValueLabel;

	/* public variables */
	public bool hasMaxValue;
	private float _maxValue;
	public float maxValue
	{
		get
		{
			return _maxValue;
		}
		set
		{
			_maxValue = value;
			UpdateComponents();
		}
	}

	private float _value;
    public float value
    {
        get
        {
			return _value;
        }
        set
        {
			_value = value;
            UpdateComponents();
        }
    }

    /* private variables */
	private float _fillerInitialSize;

	void Awake () {
		this._fillerInitialSize = Filler.sizeDelta.x;
	}

	public void UpdateComponents()
	{
		this.ValueLabel.text = ((int)value).ToString();
		if (hasMaxValue)
		{
			float progress = (value / maxValue) * 100;
			this.SetProgress(progress);
		}
	}
	
	public void SetProgress (float progress) {
		Vector2 sizeDelta = Filler.sizeDelta;
		sizeDelta.x = this._fillerInitialSize / 100 * progress;
		Filler.sizeDelta = sizeDelta;
	}

	public void TweenValueChange(float changedValue)
	{
		this.StartCoroutine(_TweenValueChange(changedValue));
	}

	private IEnumerator _TweenValueChange(float changedValue)
	{
		int oldValue = (int)this.value;

		if (changedValue > oldValue)
		{
			for (int i = oldValue; i < (int)changedValue; i++)
			{
				yield return null;
				this.value ++;
			}
		}
		else
		{
			for (int i = oldValue; i > (int)changedValue; i--)
            {
                yield return null;
                this.value --;
            }
		}

		this.value = changedValue;
	}
}
