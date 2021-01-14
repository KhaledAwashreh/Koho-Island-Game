using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SceneViewUtil : MonoBehaviour {

	[MenuItem("Tools/Align View to Isometric")]
	static void AlignViewToIsometric(){
		Transform camera = GameObject.Find ("Main Camera").transform;
		SceneView.lastActiveSceneView.pivot = new Vector3 (20, 35, 25);
		SceneView.lastActiveSceneView.size = 80;
		SceneView.lastActiveSceneView.orthographic = true;
		SceneView.lastActiveSceneView.rotation = camera.rotation;
		SceneView.lastActiveSceneView.Repaint ();
	}

	[MenuItem("Tools/Align View to UI")]
	static void AlignViewToUI(){
		Transform uiroot = GameObject.Find ("UIRoot").transform;
		SceneView.lastActiveSceneView.pivot = new Vector3 (0, 0, 0);
		SceneView.lastActiveSceneView.size = 500;
		SceneView.lastActiveSceneView.orthographic = true;
		SceneView.lastActiveSceneView.rotation = uiroot.rotation;
		SceneView.lastActiveSceneView.Repaint ();
	}
}
