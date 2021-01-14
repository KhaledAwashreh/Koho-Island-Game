using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadScenes : MonoBehaviour
{
    public Button New;

    public Button Continue;
    // Start is called before the first frame update
    void Start()
    {
        New.onClick.AddListener(OnClickNew);
        Continue.onClick.AddListener(OnClickContinue);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickNew()
    {
        EditorSceneManager.LoadSceneAsyncInPlayMode("Assets/Builder/Scenes/NameScene.unity",  new LoadSceneParameters (LoadSceneMode.Single, LocalPhysicsMode.None));
    }

    public void OnClickContinue()
    {
        EditorSceneManager.LoadSceneAsyncInPlayMode("Assets/Builder/Scenes/MainScene.unity",  new LoadSceneParameters (LoadSceneMode.Single, LocalPhysicsMode.None));
    }
}
