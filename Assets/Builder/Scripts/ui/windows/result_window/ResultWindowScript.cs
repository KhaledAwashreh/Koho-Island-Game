using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultWindowScript : WindowScript {
	public static ResultWindowScript instance;
    
	/* object references */
	public Text ResultText;

	public Image StarLeft;
	public Image StarCentre;
	public Image StarRight;

	void Awake(){
		instance = this;
	}
    
	public Text SwordManCounter;
	public Text ArcherCounter;
   
	public void OnClickReturnHomeButton(){
		SceneManager.instance.EnterNormalMode ();
	}

	public void SetData(bool victory, int swordManExpended, int archerExpended)
	{
		if (victory)
			this.ResultText.text = "<color=#FFF800FF>Victory</color>";
		else
			this.ResultText.text = "<color=#FF0000FF>Defeat</color>";

		this.SwordManCounter.text = swordManExpended.ToString();
		this.ArcherCounter.text = archerExpended.ToString();

		if(!victory)
		{
			StarLeft.color = Color.black;
			StarCentre.color = Color.black;
			StarRight.color = Color.black;
		}
	}

}
