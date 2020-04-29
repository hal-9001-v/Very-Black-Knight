using System.Collections;
using UnityEngine;

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

        //Load player stats from previous scene or new stats
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

    //Restart scene when dead
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

    //Reduce health points
    public void hurt(float dmg)
    {
        health -= dmg;

        
        if (health <= 0)
        {//Dead
            currentState = (int)State.dead;
            myAnimator.SetTrigger("dead");

            //Update Health Bar
            myPlayerGUI.setHealth(0);
        }
        else
        {//Alive
            myAnimator.SetTrigger("hurt");   
            //Update Health Bar
            myPlayerGUI.setHealth(health);

        }

    }

    //Increase upgrade points
    public void levelUp(int ups)
    {
        //Stop player input
        playerActive = false;

        upgrades = ups;
        playerLevel += ups;

        myPlayerGUI.setCurrentLevel(playerLevel);

        //Show level up screen
        displayLevelUpScreen();
    }

    public void activePlayer(bool b)
    {
        playerActive = b;
    }

    //Spend upgrade Points on health
    public void upgradeHealth()
    {
        
        if (upgrades > 0)
        {
            upgrades--;

            healthLevel++;
            //Apply changes
            loadHealthLevel();
            //Update
            displayLevelUpScreen();
        }
    }

    //Spend upgrade Points on Movement
    public void upgradeMovement()
    {
        if (upgrades > 0)
        {
            upgrades--;

            movementLevel++;
            //Apply changes
            loadMovementLevel();
            //Update
            displayLevelUpScreen();
        }
    }

    //Set movement speed according to current movement level
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

    //Set movement speed according to current health level
    private void loadHealthLevel()
    {
        if (healthLevel == 1)
        {
            MAXHEALTH = 5;
            health = MAXHEALTH;

            //Update GUI values
            myPlayerGUI.setMaxHealth(MAXHEALTH);

        }
        else if (healthLevel == 2)
        {
            MAXHEALTH = 7;
            health = MAXHEALTH;

            //Update GUI values
            myPlayerGUI.setMaxHealth(MAXHEALTH);

        }
        else if (healthLevel == 3)
        {
            MAXHEALTH = 9;
            health = MAXHEALTH;

            //Update GUI values
            myPlayerGUI.setMaxHealth(MAXHEALTH);

        }
    }

    public void displayLevelUpScreen()
    {
        //Inhibit player input
        playerActive = false;

        myPlayerGUI.setLevelUpIndicators(movementLevel, healthLevel, upgrades);

    }

    public void hideLevelUpScreen()
    {
        playerActive = true;

        myPlayerGUI.hideLevelUpIndicators();
    }


    //Load JSON data from previous scene in disk
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

    //Set default stats
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

    //Write on disk current stats
    public void saveData()
    {
        PlayerData pd = new PlayerData();
        pd.maxHealth = MAXHEALTH;
        pd.playerLevel = playerLevel;
        pd.upgradesLeft = upgrades;
        pd.movementLevel = movementLevel;
        pd.healthLevel = healthLevel;
        pd.score = movementScript.inputCount;

        Debug.Log("Data Saved");

        pd.saveScoreJSON();
    }

    //Load new Scene
    public void nextLevel()
    {
        hideLevelUpScreen();

        //Save data for next scene
        saveData();

        //Change Scene
        int nextScene = UnityEngine.SceneManagement.SceneManager.sceneCount + 1;
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex + 1);
    }
}
