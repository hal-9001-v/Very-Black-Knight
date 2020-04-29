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
        //Get Player name from input
        field = inputFieldObject.GetComponent<InputField>();

        //Read Data
        pd = PlayerData.loadPlayerDataJSON();
        score = pd.score;

        pd.readyForScore = true;

        //Text update
        scoreText = scoreTextObject.GetComponent<Text>();
        scoreText.text = "Score: " + score;
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

}
