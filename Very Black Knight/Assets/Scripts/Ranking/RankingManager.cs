using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.IO;

public class RankingManager : MonoBehaviour
{
    //Scene Elements
    public Text[] scoreTexts;
    public Text[] nameTexts;

    public GameObject boardImageObject;
    private RawImage boardImage;

    public UnityEvent afterAddPlayer;

    //STRINGS
    public static string fileName = "/Data/gamedata.json";

    //Display variable
    bool showed = false;

    ScoreBoard board;


    // Start is called before the first frame update
    void Start()
    {
        boardImage = boardImageObject.GetComponent<RawImage>();

        board = loadRankingJSON();

        //Load last player data saved
        PlayerData pd = PlayerData.loadPlayerDataJSON();

        //If PlayerData should be added to ranking
        if (pd.readyForScore)
        {
            Debug.Log("New PLayer on Board");
            addPlayer();

            //PlayerData should be added only once. Addition variable turned to 0 and updated in save file
            pd.readyForScore = false;

            pd.saveScoreJSON();
        }

        //This lines are used to reset board. Do not uncomment unless needed
        //board.setCheckValues();
        //saveRankingJSON(board);

        //Sort board to keep board updated
        board = sortBoard(board);

        hideRanking();

    }

    ScoreBoard sortBoard(ScoreBoard sc)
    {
        //Bubble sort technic
        for (int i = 0; i < sc.names.Length; i++)
        {
            for (int j = 0; j < sc.names.Length - i - 1; j++)
            {
                //Lower values mean higher positions
                if (sc.scores[j] > sc.scores[j + 1])
                {
                    //Auxiliar variables
                    string auxiliarString = sc.names[j];
                    int auxiliarInt = sc.scores[j];

                    //Set better values on higher position
                    sc.scores[j] = sc.scores[j + 1];
                    sc.names[j] = sc.names[j + 1];

                    //Set auxiliar values on lower position
                    sc.names[j + 1] = auxiliarString;
                    sc.scores[j + 1] = auxiliarInt;

                }
            }
        }

        return sc;
    }

    //SAVE AND LOAD
    void saveRankingJSON(ScoreBoard myBoard)
    {
        //JSON data technic
        if (myBoard != null)
        {
            string JSONData = JsonUtility.ToJson(myBoard);

            //Such file will is used for load and save
            File.WriteAllText(Application.dataPath + fileName, JSONData);

            Debug.Log("Score Board Succesfully Saved");
        }
    }
    ScoreBoard loadRankingJSON()
    {
        ScoreBoard myBoard;

        //Board is extracted from JSON data file
        myBoard = JsonUtility.FromJson<ScoreBoard>(File.ReadAllText(Application.dataPath + fileName));

        return myBoard;
    }
    public void addPlayer()
    {
        if (board == null)
        {
            board = loadRankingJSON();
        }

        PlayerData pd = new PlayerData();
        pd = PlayerData.loadPlayerDataJSON();

        board.addPlayer(pd);

        saveRankingJSON(board);
    }

    //INTERFACES
    public void displayRanking()
    {
        if (boardImage != null)
        {
            boardImage.enabled = true;

            for (int i = 0; i < 5; i++)
            {

                //Turn image and text objects visible
                scoreTexts[i].enabled = true;
                nameTexts[i].enabled = true;

                //Update score texts
                scoreTexts[i].text = "" + board.scores[i];
                nameTexts[i].text = board.names[i];

            }
        }
    }
    public void hideRanking()
    {
        //Turning invisible all ranking objects
        boardImage.enabled = false;

        for (int i = 0; i < 5; i++)
        {
            scoreTexts[i].enabled = false;
            nameTexts[i].enabled = false;
        }
    }
    public void callRanking()
    {
        if (showed)
        {
            hideRanking();
            showed = false;
        }
        else
        {
            displayRanking();
            showed = true;
        }
    }

}

class ScoreBoard
{
    public string[] names;
    public int[] scores;

    public void print()
    {

        for (int i = 0; i < names.Length; i++)
        {
            Debug.Log("Names: " + names[i] + " Scores: " + scores[i]);
        }

    }

    public void setCheckValues()
    {
        scores = new int[5];
        names = new string[5];

        scores[0] = 100000;
        names[0] = "Bot";

        scores[1] = 100000;
        names[1] = "Bot";

        scores[2] = 100000;
        names[2] = "Bot";

        scores[3] = 100000;
        names[3] = "Bot";

        scores[4] = 100000;
        names[4] = "Bot";

    }

    public void addPlayer(PlayerData pd)
    {
        string[] auxNames = new string[names.Length + 1];
        int[] auxScores = new int[names.Length + 1];

        //Copy of arrays into a bigger one
        for (int i = 0; i < names.Length; i++)
        {
            auxNames[i] = names[i];
            auxScores[i] = scores[i];

        }

        //Add new player to bigger array
        auxNames[names.Length] = pd.name;
        auxScores[names.Length] = pd.score;

        //Update pointers to new array
        names = auxNames;
        scores = auxScores;


    }

}


