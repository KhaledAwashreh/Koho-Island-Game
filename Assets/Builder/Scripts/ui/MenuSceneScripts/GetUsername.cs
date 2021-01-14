using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Mime;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GetUsername : MonoBehaviour
{
    public Button startBtn;
    public InputField username;

    public static int flag = -1;
    public static string userDataName;

    
    
    
    
    
    
    public Text redText;
    // Start is called before the first frame update
    void Start()
    {
        startBtn.onClick.AddListener(OnClickGetUsername);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void OnClickGetUsername()
    {
        
        if (username.text == "")
        {
            redText.text = "Please enter your username!";
        }
        else if (username.text.Contains(" "))
        {
            redText.text = "No spaces please!";
        }
        else
        {
            userDataName = username.text;
            flag = 1;
            EditorSceneManager.LoadSceneAsyncInPlayMode("Assets/Builder/Scenes/StoryScene.unity",
                new LoadSceneParameters(LoadSceneMode.Single, LocalPhysicsMode.None));

        }
    }
}

