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

    void Start() {
        field = inputFieldObject.GetComponent<InputField>();
        score = PlayerPrefs.GetInt("inputCount");

        scoreText = scoreTextObject.GetComponent<Text>();

        scoreText.text = "Score: " + score;

        pd = PlayerData.loadPlayerDataJSON();
        pd.readyForScore = true;

        scoreText.text = ""+pd.score;
    }

    public void addPlayer() {
        pd.name = field.text;

        pd.saveScoreJSON();
    }


}
