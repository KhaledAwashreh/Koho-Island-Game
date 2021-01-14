using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StoryManager : MonoBehaviour
{
    public Button nextBtn;
    public Image storyBook;

    public Sprite[] s = new Sprite[9];

    public int pge = 0;

    // Start is called before the first frame update
    void Start()
    {
        pge = 0;
        storyBook.sprite = s[pge];
        storyBook = GetComponent<Image>();
        nextBtn.onClick.AddListener(nextOnClick);

        Debug.Log(pge);
    }

    // Update is called once per frame
        void Update()
        {
            
            
            
            
        }

        void nextOnClick()
        {
                pge++;

            if (pge < 8)
            {
                storyBook.sprite = s[pge];
            }
            else
            {
                EditorSceneManager.LoadSceneAsyncInPlayMode("Assets/Builder/Scenes/MainScene.unity",
                    new LoadSceneParameters(LoadSceneMode.Single, LocalPhysicsMode.None));
            }

            
        }
    }

