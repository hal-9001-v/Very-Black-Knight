using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class RankingAdder : MonoBehaviour
{
    public GameObject inputFieldObject;
    InputField field;

    public GameObject scoreTextObject;
    private Text scoreText;

    int score;

    PlayerData pd;

    void Start()
    {
        loadData();
        field = inputFieldObject.GetComponent<InputField>();


        scoreText = scoreTextObject.GetComponent<Text>();

        scoreText.text = "Score: " + score;

        pd = PlayerData.loadPlayerDataJSON();
        pd.readyForScore = true;

        scoreText.text = "" + pd.score;
    }

    public void addPlayer()
    {
        pd.name = field.text;

        pd.saveScoreJSON();


        if (score > 500)
        {
            Debug.Log("Closing Game");
            Application.Quit(0);
        }

    }

    private void loadData()
    {
        PlayerData pd = PlayerData.loadPlayerDataJSON();

        score = pd.score;

    }


}
