using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public GameObject playerGuiObject;
    private PlayerGUI myPlayerGUI;

    public bool loadFromDisk;

    PlayerMovement movementScript;
    Animator myAnimator;

    float MAXHEALTH;
    float health;

    private int playerLevel;
    private int upgrades;

    public int movementLevel { get; private set; }
    public int healthLevel { get; private set; }

    private int currentState;
    bool finishedTurn = false;
    bool playerActive = true;

    enum State
    {
        idle = 0,
        walking = 1,
        dead = 2
    }

    // Start is called before the first frame update
    void Start()
    {
        //Find Scripts
        movementScript = gameObject.GetComponent<PlayerMovement>();

        myAnimator = gameObject.GetComponent<Animator>();

        myPlayerGUI = playerGuiObject.GetComponent<PlayerGUI>();

        if (movementScript == null)
        {
            Debug.LogError("Movement Script is missing!");

        }

        if (myAnimator == null)
        {
            Debug.LogError("Animator Component is missing!");

        }

        currentState = (int)State.idle;

        if (loadFromDisk)
        {
            loadData();
        }
        else
        {
            setData();
        }

        Debug.Log("HP: " + health);
        Debug.Log("Movement: " + movementScript.timeToReach);
        Debug.Log("HP LVL: " + healthLevel);
        Debug.Log("MOVEMENT LVL: " + movementLevel);
        Debug.Log("Input Count: "+movementScript.inputCount);
    }

    // Update is called once per frame
    void Update()
    {
        if (playerActive)
        {
            switch (currentState)
            {
                //Idle
                case 0:

                    finishedTurn = false;

                    if (movementScript.checkInput())
                    {
                        currentState = (int)State.walking;

                    }
                    break;

                //Walking
                case 1:
                    if (movementScript.doingMovement)
                    {
                        myAnimator.SetBool("Walking", true);
                    }
                    else
                    {
                        myAnimator.SetBool("Walking", false);
                        currentState = (int)State.idle;

                        finishedTurn = true;
                    }

                    break;
                //dead
                case 2:
                    playerActive = false;
                    StartCoroutine(restartScene());
                    break;

                //Hurt
                case 3:

                    break;

            }
        }

    }

    IEnumerator restartScene()
    {
        yield return new WaitForSeconds(2);

        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);

        yield return 0;
    }

    public bool hasFinishedTurn()
    {

        return finishedTurn;
    }

    public void hurt(float dmg)
    {
        health -= dmg;

        if (health <= 0)
        {
            currentState = (int)State.dead;
            myAnimator.SetTrigger("dead");
            myPlayerGUI.setHealth(0);
        }
        else
        {
            myAnimator.SetTrigger("hurt");
            myPlayerGUI.setHealth(health);

        }

    }

    public void levelUp(int ups)
    {
        playerActive = false;

        upgrades = ups;
        playerLevel += ups;
        myPlayerGUI.setCurrentLevel(playerLevel);

        displayLevelUpScreen();
    }

    public void activePlayer(bool b)
    {
        playerActive = b;
    }

    public void upgradeHealth()
    {
        if (upgrades > 0)
        {
            upgrades--;

            healthLevel++;
            loadHealthLevel();
            displayLevelUpScreen();
        }
    }
    public void upgradeMovement()
    {
        if (upgrades > 0)
        {
            upgrades--;

            movementLevel++;

            loadMovementLevel();
            displayLevelUpScreen();
        }


    }
    public void upgradeAttack()
    {
        if (upgrades > 0)
        {
            upgrades--;

            movementLevel++;
            displayLevelUpScreen();
        }

    }

    private void loadMovementLevel()
    {
        if (movementLevel == 1) {
            movementScript.setTimeToReach(0.5f);
        }
        else
        if (movementLevel == 2)
        {
            movementScript.setTimeToReach(0.4f);

        }
        else if (movementLevel == 3)
        {
            movementScript.setTimeToReach(0.3f);

        }
    }
    private void loadHealthLevel()
    {
        if (healthLevel == 1)
        {
            MAXHEALTH = 5;
            health = MAXHEALTH;

            myPlayerGUI.setMaxHealth(MAXHEALTH);

        }
        else if (healthLevel == 2)
        {
            MAXHEALTH = 7;
            health = MAXHEALTH;

            myPlayerGUI.setMaxHealth(MAXHEALTH);

        }
        else if (healthLevel == 3)
        {
            MAXHEALTH = 9;
            health = MAXHEALTH;

            myPlayerGUI.setMaxHealth(MAXHEALTH);

        }
    }

    public void displayLevelUpScreen()
    {
        playerActive = false;

        myPlayerGUI.setLevelUpIndicators(movementLevel, healthLevel, upgrades);

    }

    public void hideLevelUpScreen()
    {
        playerActive = true;

        myPlayerGUI.hideLevelUpIndicators();
    }

    private void loadData()
    {
        PlayerData pd = PlayerData.loadPlayerDataJSON();

        //Floats
        MAXHEALTH = pd.maxHealth;
        health = pd.health;

        //Integers
        playerLevel = pd.playerLevel;
        upgrades = pd.upgradesLeft;
        movementLevel = pd.movementLevel;
        healthLevel = pd.healthLevel;
        movementScript.inputCount = pd.score;

        loadHealthLevel();
        loadMovementLevel();

        myPlayerGUI.setMaxHealth(MAXHEALTH);
        myPlayerGUI.setCurrentLevel(playerLevel);

    }

    private void setData()
    {
        //FOATS
        MAXHEALTH = 5;
        health = MAXHEALTH;

        //INTEGERS
        playerLevel = 1;
        upgrades = 0;
        movementLevel = 1;
        healthLevel = 1;
        movementScript.inputCount = 0;


        myPlayerGUI.setMaxHealth(MAXHEALTH);
        myPlayerGUI.setCurrentLevel(playerLevel);
    }

    private void saveData()
    {
        PlayerData pd = new PlayerData();
        pd.maxHealth = MAXHEALTH;
        pd.playerLevel = playerLevel;
        pd.upgradesLeft = upgrades;
        pd.movementLevel = movementLevel;
        pd.healthLevel = healthLevel;
        pd.score = movementScript.inputCount;

        pd.saveScoreJSON();
    }

    public void nextLevel()
    {
        hideLevelUpScreen();
        saveData();
        int nextScene = UnityEngine.SceneManagement.SceneManager.sceneCount + 1;
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex + 1);
    }
}
