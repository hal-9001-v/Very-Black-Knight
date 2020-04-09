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

    public override void _startTurn()
    {
        if (readyForNextTurn)
        {
            if (hurt())
            {
                readyForNextTurn = false;
                currentState = (int)State.idle;
                resetTurnIn(2f);
                return;
            }

            switch (currentState)
            {
                //Idle
                case 0:

                    if (Vector3.Distance(transform.position, player.transform.position) < 20)
                    {
                        timeToReach = baseTimeToReach;
                    }
                    else
                    {
                        timeToReach = baseTimeToReach * 2;
                    }

                    movementList.Clear();

                    //Decide movement
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
    public override void _endOfMovementActions()
    {
        readyForNextTurn = true;
        currentState = (int)State.idle;
        myAnimator.SetBool("walking", false);

        setAttackTiles();

    }

    private bool runMoveList()
    {
        //Run over all possible movement tiles 
        foreach (Vector2 movement in movementList)
        {
            if (_canMakeMovement(movement.x, movement.y))
            {
                if (movement.x > 0) transform.eulerAngles = new Vector3(0, 90, 0);
                else if (movement.x < 0) transform.eulerAngles = new Vector3(0, -90, 0);

                else if (movement.y > 0) transform.eulerAngles = new Vector3(0, 0, 0);
                else if (movement.y < 0) transform.eulerAngles = new Vector3(0, 180, 0);

                return true;
            }
        }

        return false;
    }

    private void setAttackTiles()
    {

        MeshRenderer mr;

        foreach (GameObject tile in attackTiles)
        {
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
        delayFunction();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _makeMovement();
    }

    //In t milliseconds, readyForNextTurn will be trye
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
