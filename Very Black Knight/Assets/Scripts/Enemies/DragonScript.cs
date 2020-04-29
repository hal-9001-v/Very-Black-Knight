using System.Collections.Generic;
using UnityEngine;
public class DragonScript : Enemy
{
    //Objects
    GameObject player;
    List<Vector2> movementList;
    Animator myAnimator;

    //Booleans
    bool readyForNextTurn = true;
    bool delay = false;

    //Floats
    float waitTime;
    float currentTime;
    float baseTimeToReach;

    //Integers
    private int currentState = 0;
    
    enum State
    {
        idle = 0,
        walking = 1
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("player");
        myAnimator = gameObject.GetComponent<Animator>();

        movementList = new List<Vector2>();

        baseTimeToReach = timeToReach;

        _initialize();
    }

    //StartTurn is called on an Enemy List on Game Object, dragon's actions are started with this function
    public override void _startTurn()
    {
        if (readyForNextTurn)
        {
            //Check wether player is on an attack Tile
            if (hurt())
            {
                //Turn is over if an attack is done
                readyForNextTurn = false;
                currentState = (int)State.idle;

                //2 Seconds until dragon can move or attack again
                resetTurnIn(2f);
                return;
            }

            switch (currentState)
            {
                //Idle
                case 0:
                    //Spped boost in case player is far away
                    if (Vector3.Distance(transform.position, player.transform.position) < 20)
                    {
                        timeToReach = baseTimeToReach;
                    }
                    else
                    {
                        timeToReach = baseTimeToReach * 2;
                    }

                    movementList.Clear();

                    //Decide possible movement directions
                    if (player.transform.position.x > transform.position.x)
                    {
                        movementList.Add(new Vector2(1, 0));
                    }
                    if (player.transform.position.z > transform.position.z)
                    {
                        movementList.Add(new Vector2(0, 1));

                    }
                    if (player.transform.position.x < transform.position.x)
                    {
                        movementList.Add(new Vector2(-1, 0));

                    }
                    if (player.transform.position.z < transform.position.z)
                    {
                        movementList.Add(new Vector2(0, -1));
                    }

                    //Check possible movements
                    if (runMoveList())
                    {
                        currentState = (int)State.walking;
                        myAnimator.SetBool("walking", true);
                        readyForNextTurn = false;

                    }

                    break;

                //Walking
                case 1:
                    // Look at endOfMovement() due to synchronization 

                    break;
            }
        }
    }

    //Restart turn when movement is done
    public override void _endOfMovementActions()
    {
        readyForNextTurn = true;
        currentState = (int)State.idle;
        myAnimator.SetBool("walking", false);

        //Add tiles under attack to list
        setAttackTiles();

    }

    private bool runMoveList()
    {
        //Run over all possible movement directions
        foreach (Vector2 movement in movementList)
        {
            //Choose a tile on direction
            if (_canMakeMovement(movement.x, movement.y))
            {
                //Rotations according to chosen movement
                if (movement.x > 0) transform.eulerAngles = new Vector3(0, 90, 0);
                else if (movement.x < 0) transform.eulerAngles = new Vector3(0, -90, 0);

                else if (movement.y > 0) transform.eulerAngles = new Vector3(0, 0, 0);
                else if (movement.y < 0) transform.eulerAngles = new Vector3(0, 180, 0);

                //A movement must be done
                return true;
            }
        }

        //No possible movement
        return false;
    }

    private void setAttackTiles()
    {

        MeshRenderer mr;

        foreach (GameObject tile in attackTiles)
        {

            //Get Mesh renderer from current tile
            if (tile.GetComponent<MeshRenderer>() != null)
                mr = tile.GetComponent<MeshRenderer>();
            else
            {
                mr = tile.GetComponentInChildren<MeshRenderer>();

            }

            //Set last under attack tiles back to original colour 
            mr.material.color -= attackColor;

        }

        attackTiles.Clear();
        
        
        //Check if tiles in default position exists and if so, add them to the tiles under attack list
        _addTileToAttackList(0, 0);
        _addTileToAttackList(1, 0);
        _addTileToAttackList(-1, 0);
        _addTileToAttackList(0, 1);
        _addTileToAttackList(0, -1);
        _addTileToAttackList(1, 1);
        _addTileToAttackList(-1, 1);
        _addTileToAttackList(1, -1);
        _addTileToAttackList(-1, -1);

        //Paint in red all tiles under Attack
        foreach (GameObject tile in attackTiles)
        {
            mr = tile.GetComponent<MeshRenderer>();

            if (mr == null)
            {
                mr = tile.GetComponentInChildren<MeshRenderer>();

            }

            mr.material.color += attackColor;
        }

    }

    void Update()
    {
        //Turn Reset function timer
        delayFunction();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _makeMovement();
    }

    //After t milliseconds, readyForNextTurn will be trye
    void resetTurnIn(float t)
    {
        waitTime = t;
        currentTime = 0;

        delay = true;
    }

    //Reset readyForNexTUrn after certain time specified in resetTurnIn(). This function is called in Update()
    void delayFunction()
    {
        if (delay)
        {
            if (currentTime > waitTime)
            {
                readyForNextTurn = true;
                delay = false;
            }
            currentTime += Time.deltaTime;
        }
    }

    bool hurt()
    {
        //Check if player is on any of listed attack tiles
        foreach (GameObject tile in attackTiles)
        {
            if (tile.GetComponent<GridTilePro>().movable(player.transform.position.x, player.transform.position.z))
            {
                player.GetComponent<Player>().hurt(2);

                return true;
            }

        }
        return false;

    }
}
