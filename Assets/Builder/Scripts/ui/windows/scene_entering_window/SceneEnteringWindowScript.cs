using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class SceneEnteringWindowScript : WindowScript {

	public Action OnIntermediate;

	public void IntermediateAction(){
		if (OnIntermediate != null) {
			OnIntermediate.Invoke ();
		}
	}
}
