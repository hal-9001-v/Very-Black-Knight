using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankingAdder : MonoBehaviour
{
    public GameObject inputFieldObject;
    InputField field;

    public GameObject scoreTextObject;
    private Text scoreText;

    int score;

    void Start() {
        field = inputFieldObject.GetComponent<InputField>();
        score = PlayerPrefs.GetInt("inputCount");

        scoreText = scoreTextObject.GetComponent<Text>();

        scoreText.text = "Score: " + score;
    }

    public void addPlayer() {

        PlayerPrefs.SetString("playerName", field.text);

        Debug.LogWarning("Player: "+field.text+" Score: "+score);

        PlayerPrefs.Save();

    }
}
