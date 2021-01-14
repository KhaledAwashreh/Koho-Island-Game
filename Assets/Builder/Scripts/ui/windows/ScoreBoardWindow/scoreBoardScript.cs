using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityScript.Scripting.Pipeline;

public class scoreBoardScript : WindowScript
{
    private float previousCheck;
    private scoreBoardScript instance;
    public Transform entryContainer;
    public Transform entryTemplate;
    private List<playerScore> _highScoreEntries;
    private System.Collections.Generic.List<Transform> highScoreEntriesTransformList;
     void Start()
     {
         previousCheck = Time.time;
        instance = this;
        entryTemplate.gameObject.SetActive(false);
        _highScoreEntries = new List<playerScore>();
        
        fillData();

        for (int i = 0; i < _highScoreEntries.Count; i++)
        {
            for (int j = i + 1; j < _highScoreEntries.Count; j++)
            {
                if (_highScoreEntries[j].score > _highScoreEntries[i].score)
                {
                    playerScore temp = _highScoreEntries[i];
                    _highScoreEntries[i] = _highScoreEntries[j];
                    _highScoreEntries[j] = temp;
                }
            }
        }





        highScoreEntriesTransformList=new System.Collections.Generic.List<Transform>();
        foreach (playerScore entry in _highScoreEntries)
        {
            createHighScore(entry,entryContainer,highScoreEntriesTransformList);
        }
        

        }

    // Update is called once per frame
    private void Update()
    {
      
    }

    public void createHighScore(playerScore data,Transform container, System.Collections.Generic.List<Transform> TramsformList)
    {
        float templateHeight = 20f;

        Transform entryTransform = Instantiate(entryTemplate, container);
        RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
        entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * TramsformList.Count);
        entryTransform.gameObject.SetActive(true);


        int rank = TramsformList.Count+ 1;
        string rankString;
        switch (rank)
        {
            default:
                rankString = rank + "Th";
                break;
            case 1:
                rankString = "1ST";
                break;
            case 2:
                rankString = "2ND";
                break;
            case 3:
                rankString = "3RD";
                break;
        }

        entryTransform.Find("Position").GetComponent<Text>().text = rankString;
        int score = data.score;
        entryTransform.Find("Enviromental score").GetComponent<Text>().text = score.ToString();
        string name = data.name;
        entryTransform.Find("PlayerName").GetComponent<Text>().text = name;
        TramsformList.Add(entryTransform);
    }

    public void fillData()
    {
        List<playerScore> temp = fireBaseHandler.instance.Scores;
        for (int i = 0; i < temp.Count; i++)
        {
            Debug.Log(fireBaseHandler.instance.Scores.Count+"count of entries");
            playerScore newEntry = new playerScore(temp[i].name, temp[i].score);
            _highScoreEntries.Add(newEntry);
           // Debug.Log(temp[i].name);

        }
    }

   

}
