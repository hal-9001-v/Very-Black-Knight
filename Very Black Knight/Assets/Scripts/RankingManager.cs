using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class RankingManager : MonoBehaviour
{
    int[] scores;
    bool showed = false;

    public GameObject resetButtonObject;

    string[] names;

    public Text[] scoreTexts;
    public Text[] nameTexts;

    public GameObject boardImageObject;
    private RawImage boardImage;

    public UnityEvent afterAddPlayer;

    // Start is called before the first frame update
    void Start()
    {
        boardImage = boardImageObject.GetComponent<RawImage>();
        scores = new int[5];
        names = new string[5];

        hideRanking();

        sortRanking();

        //ATTENTION!!! Used for first saving
        //resetRanking();

        /*
        foreach (int a in scores)
        {
            Debug.Log(a);
        }
        */

        //displayRanking();

    }

    public void resetRanking()
    {

        PlayerPrefs.DeleteAll();

        scores[0] = 9999;
        scores[1] = 9999;
        scores[2] = 9999;
        scores[3] = 9999;
        scores[4] = 9999;

        names[0] = "Bot";
        names[1] = "Bot";
        names[2] = "Bot";
        names[3] = "Bot";
        names[4] = "Bot";

        saveRanking();

        loadRanking();

    }

    void loadRanking()
    {
        scores[0] = PlayerPrefs.GetInt("score1");
        scores[1] = PlayerPrefs.GetInt("score2");
        scores[2] = PlayerPrefs.GetInt("score3");
        scores[3] = PlayerPrefs.GetInt("score4");
        scores[4] = PlayerPrefs.GetInt("score5");

        names[0] = PlayerPrefs.GetString("name1");
        names[1] = PlayerPrefs.GetString("name2");
        names[2] = PlayerPrefs.GetString("name3");
        names[3] = PlayerPrefs.GetString("name4");
        names[4] = PlayerPrefs.GetString("name5");

    }

    void saveRanking()
    {
        PlayerPrefs.SetString("name1", names[0]);
        PlayerPrefs.SetString("name2", names[1]);
        PlayerPrefs.SetString("name3", names[2]);
        PlayerPrefs.SetString("name4", names[3]);
        PlayerPrefs.SetString("name5", names[4]);

        PlayerPrefs.SetInt("score1", scores[0]);
        PlayerPrefs.SetInt("score2", scores[1]);
        PlayerPrefs.SetInt("score3", scores[2]);
        PlayerPrefs.SetInt("score4", scores[3]);
        PlayerPrefs.SetInt("score5", scores[4]);

        PlayerPrefs.Save();
    }

    void sortRanking()
    {

        int playerCount = PlayerPrefs.GetInt("inputCount");
        string playerName = PlayerPrefs.GetString("playerName");
        PlayerPrefs.SetInt("inputCount", 999999);
        PlayerPrefs.SetString("playerName", "Bot");

        Person[] people = new Person[6];

        string[] newNames = new string[6];
        int[] newScores = new int[6];
        bool[] set = new bool[6];

        loadRanking();

        for (int i = 0; i < 5; i++)
        {
            people[i] = new Person(names[i], scores[i]);

            newNames[i] = names[i];
            newScores[i] = scores[i];
            set[i] = false;
        }

        people[5] = new Person(playerName, playerCount);

        newNames[5] = people[5].name;
        newScores[5] = people[5].score;
        set[5] = false;

        System.Array.Sort(newScores);

        foreach (Person person in people)
        {

            for (int i = 0; i < 6; i++)
            {
                if (!set[i] && newScores[i] == person.score)
                {
                    newNames[i] = person.name;
                    set[i] = true;

                    break;
                }
            }
        }
        
        for (int i = 0; i < 5; i++)
        {

            names[i] = newNames[i];
            scores[i] = newScores[i];
        }
        
        saveRanking();


    }

    public void addPlayer()
    {

        afterAddPlayer.Invoke();

    }

    public void displayRanking()
    {
        if (boardImage != null)
        {
            boardImage.enabled = true;


            for (int i = 0; i < 5; i++)
            {
                scoreTexts[i].enabled = true;
                nameTexts[i].enabled = true;

            }

            resetButtonObject.SetActive(true);

            loadRanking();

            for (int i = 0; i < 5; i++)
            {
                scoreTexts[i].text = "" + scores[i];
                nameTexts[i].text = names[i];

            }
        }
    }

    public void hideRanking()
    {
        boardImage.enabled = false;

        for (int i = 0; i < 5; i++)
        {
            scoreTexts[i].enabled = false;
            nameTexts[i].enabled = false;

        }
        resetButtonObject.SetActive(false);
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



    // Update is called once per frame
    void Update()
    {

    }

    class Person
    {
        public string name { get; private set; }
        public int score { get; private set; }

        public Person(string name, int score)
        {
            this.name = name;
            this.score = score;
        }
    }
}
