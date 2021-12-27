using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Leaderboard
{
    
    public string name;
    public int score;

    Leaderboard() { }

    public Leaderboard(string name, int score)
    {
        this.name = name;
        this.score = score;
    }
}
