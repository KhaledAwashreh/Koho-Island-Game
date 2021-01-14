using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoItemCtrl : MonoBehaviour {
	/* object references */
	public Text Property;
	public Text Value;

	public void SetData(string property, string value)
	{
		this.Property.text = property;
		this.Value.text = value;
	}
}
