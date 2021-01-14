using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]

public class playerScore : MonoBehaviour
{  public string name;
    public int score;

    public playerScore(String name, int score)
    {
        this.name = name;
        this.score = score;
    }

  
}
