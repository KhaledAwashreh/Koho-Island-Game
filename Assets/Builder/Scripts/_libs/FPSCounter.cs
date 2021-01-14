/* **************************************************************************
 * FPS COUNTER
 * **************************************************************************
 * Written by: Coppra Games
 * Created: June 2017
 * *************************************************************************/

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FPSCounter : MonoBehaviour {

	/* obejct refs */
	public Text FPS;

	/* Public Variables */
	internal float frequency = 0.5f;
	internal int framesPerSec;

	private void Start() {
		StartCoroutine(updateFPS());
	}
	
	/*
	 * EVENT: FPS
	 */
	private IEnumerator updateFPS() {
		for(;;){
			
			// Capture frame-per-second
			int lastFrameCount = Time.frameCount;
			float lastTime = Time.realtimeSinceStartup;
			yield return new WaitForSeconds(frequency);
			float timeSpan = Time.realtimeSinceStartup - lastTime;
			int frameCount = Time.frameCount - lastFrameCount;
			
			// Display it
			this.framesPerSec = Mathf.RoundToInt(frameCount / timeSpan);
			this.FPS.text = this.framesPerSec.ToString ();

		}
	}

}