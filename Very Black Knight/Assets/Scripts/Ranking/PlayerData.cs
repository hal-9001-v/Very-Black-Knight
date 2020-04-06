using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerData
{
    //Path for data
    public static string sceneData = "/Data/sceneData.json";


    //Adding player to ranking board is up to this variable
    public bool readyForScore;

    //Player name
    public string name;
    //Amount of movements
    public int score;

    public float maxHealth;
    public float health;
    
    public int playerLevel;
    public int upgradesLeft;
    public int movementLevel;
    public int healthLevel;

    //Save in file all data from player for ranking system and scene changes
    public void saveScoreJSON()
    {
        string JSONData = JsonUtility.ToJson(this);
        File.WriteAllText(Application.dataPath + sceneData, JSONData);
    }
    //Load player data from previous scenes
    public static PlayerData loadPlayerDataJSON()
    {
        return JsonUtility.FromJson<PlayerData>(File.ReadAllText(Application.dataPath + sceneData));
    }
}

