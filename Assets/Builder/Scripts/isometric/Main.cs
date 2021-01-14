using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
	public static Main instance;

	/* object refs */

	/* public vars */

	/* private vars */

	void Awake()
	{
		instance = this;
		Items.LoadItems();
		Sprites.LoadSprites();
	}
}
