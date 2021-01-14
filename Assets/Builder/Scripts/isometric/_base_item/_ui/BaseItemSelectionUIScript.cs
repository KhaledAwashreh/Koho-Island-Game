using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseItemSelectionUIScript : MonoBehaviour
{

	/* object references */
	public Transform ArrowRight;
	public Transform ArrowLeft;
	public Transform ArrowTop;
	public Transform ArrowBottom;

	public Transform ItemInfoContainer;

	public TextMesh NameLabel;
	public TextMesh NameLabelShadow;

	public TextMesh LevelLabel;
	public TextMesh LevelLabelShadow;

	public SpriteRenderer Grid;
	public Sprite GridGreen;
	public Sprite GridRed;

	void Start()
	{
		BaseItemScript baseItem = this.GetComponentInParent<BaseItemScript>();
		Vector3 baseSize = baseItem.GetSize();

		/* resize each elements for same physical size */
		this.ArrowRight.localScale = this.ArrowRight.localScale / baseSize.x;
		this.ArrowLeft.localScale = this.ArrowLeft.localScale / baseSize.x;
		this.ArrowTop.localScale = this.ArrowTop.localScale / baseSize.x;
		this.ArrowBottom.localScale = this.ArrowBottom.localScale / baseSize.x;

		this.ItemInfoContainer.localScale = this.ItemInfoContainer.localScale / baseSize.x;

		/* update item info details */
		this.NameLabel.text = this.NameLabelShadow.text = baseItem.itemData.name;
		this.LevelLabel.text = this.LevelLabelShadow.text = "Level 1";

		this.Grid.transform.localScale = this.Grid.transform.localScale / baseSize.x;
		this.Grid.size = new Vector2(baseSize.x, baseSize.z);
		this.ShowGrid(false);
	}

	public void ShowGrid(bool isTrue)
	{
		this.Grid.gameObject.SetActive(isTrue);
	}

	public void SetGridColor(Color color)
	{
		if (color == Color.green)
		{
			this.Grid.sprite = this.GridGreen;

		}
		else if (color == Color.red)
		{
			this.Grid.sprite = this.GridRed;
		}
	}
}
