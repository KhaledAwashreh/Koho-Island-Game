using System;
using System.Collections.Generic;
using UnityEngine;

public class fireBaseHandler : MonoBehaviour
{
    private float previousCheck;
    public static fireBaseHandler instance;
    private  string playerName;
    private  int score;
    public  List<playerScore> Scores=new List<playerScore>();
    private void Start()
    {
        instance = this;
        playerName = DataBaseManager.instance._gameData.sceneData.getUserInfo().name;
        score= (int)(1000 - 5*ConsequencesSystem.instance.airPollution-100*(ConsequencesSystem.instance.Co2Level)+10*(ConsequencesSystem.instance.O2Level)+10*ConsequencesSystem.instance.treeCount);
    
     
        loadData();
    }

    private void Update()
    {
        if (Time.time - previousCheck > 3)

        {
            previousCheck = Time.time;
            fireBaseHandler.instance.uploadData();
            fireBaseHandler.instance.loadData();
            fireBaseHandler.instance.calculateScore();
        }
    }

    public  void loadData()
    {
      List<playerScore> temp=new List<playerScore>();

      DatabaseHandler.GetUsers(users =>
        {
            
            foreach (var user in users)
            {
                playerScore newEntry=new playerScore(user.Value.name,user.Value.score);
                temp.Add(newEntry);

            }
        });
      Scores = temp;
    }
    

    public  void uploadData()
    {
        var user2 = new playerScore(playerName,score);
        DatabaseHandler.PostUser(user2, user2.name, () =>
        {
            Debug.Log("Added Successfully");
        });
    }
    public void calculateScore() {
        score = (int)(1000 - 5*ConsequencesSystem.instance.airPollution-100*(ConsequencesSystem.instance.Co2Level)+10*(ConsequencesSystem.instance.O2Level)+10*ConsequencesSystem.instance.treeCount);
    }

}