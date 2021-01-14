using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowScript : MonoBehaviour {
    
	public virtual void Close(){
		if (this.gameObject != null)
		{
			Destroy(this.gameObject);
		}
	}
}
