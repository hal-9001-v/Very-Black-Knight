using System.Collections.Generic;
using UnityEngine;
public class Guard : Enemy
{
    //Objects
    GameObject player;
    List<Vector2Int> movementList;
    Animator myAnimator;

    //Booleans
    bool readyForNextTurn = true;
    bool delay = false;

    //Floats
    float waitTime;
    float currentTime;
    float baseTimeToReach;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("player");
        myAnimator = gameObject.GetComponent<Animator>();

        movementList = new List<Vector2Int>();

        baseTimeToReach = timeToReach;

        _initialize();
    }

    public override void _startTurn()
    {
        if (readyForNextTurn)
        {
            readyForNextTurn = false;

            if (Vector3.Distance(transform.position, player.transform.position) < 20)
            {
                timeToReach = baseTimeToReach;
            }
            else
            {
                timeToReach = baseTimeToReach * 2;
            }

            bool attackIsDone = attack();
            if (attackIsDone) return;

            calculateMovements();
            tryToMakeMovement();

        }
    }

    public override void _endOfMovementActions()
    {
        readyForNextTurn = true;
        Debug.Log("READY: " + readyForNextTurn);
        myAnimator.SetBool("walking", false);

    }

    private void calculateMovements()
    {
        setMovementList(Random.Range(0,3));

        Debug.Log(movementList.Count);


    }

    void setMovementList(int i) {

        movementList.Clear();

        if (i == 0)
        {
            

            //Decide movement
            if (player.transform.position.x > transform.position.x)
            {
                movementList.Add(new Vector2Int(1, 0));
            }

            if (player.transform.position.z > transform.position.z)
            {
                movementList.Add(new Vector2Int(0, 1));
            }

            if (player.transform.position.x < transform.position.x)
            {
                movementList.Add(new Vector2Int(-1, 0));
            }

            if (player.transform.position.z < transform.position.z)
            {
                movementList.Add(new Vector2Int(0, -1));
            }
        }


        if (i == 1)
        {
            //Decide movement
            if (player.transform.position.z > transform.position.z)
            {
                movementList.Add(new Vector2Int(0, 1));
            }

            if (player.transform.position.x > transform.position.x)
            {
                movementList.Add(new Vector2Int(1, 0));
            }

            
            if (player.transform.position.x < transform.position.x)
            {
                movementList.Add(new Vector2Int(-1, 0));
            }

            if (player.transform.position.z < transform.position.z)
            {
                movementList.Add(new Vector2Int(0, -1));
            }

        }

        if (i == 2)
        {
            //Decide movement
            if (player.transform.position.z < transform.position.z)
            {
                movementList.Add(new Vector2Int(0, -1));
            }


            if (player.transform.position.z > transform.position.z)
            {
                movementList.Add(new Vector2Int(0, 1));
            }

            
            if (player.transform.position.x > transform.position.x)
            {
                movementList.Add(new Vector2Int(1, 0));
            }


            if (player.transform.position.x < transform.position.x)
            {
                movementList.Add(new Vector2Int(-1, 0));
            }


        }

        if (i == 3)
        {
            //Decide movement
            if (player.transform.position.x < transform.position.x)
            {
                movementList.Add(new Vector2Int(-1, 0));
            }


            if (player.transform.position.z < transform.position.z)
            {
                movementList.Add(new Vector2Int(0, -1));
            }


            if (player.transform.position.z > transform.position.z)
            {
                movementList.Add(new Vector2Int(0, 1));
            }

            
            if (player.transform.position.x > transform.position.x)
            {
                movementList.Add(new Vector2Int(1, 0));
            }

        }

    }

    private bool tryToMakeMovement()
    {
        //Run over all possible movement tiles 
        foreach (Vector2Int movement in movementList)
        { 
            if (_canMakeMovement(movement.x, movement.y))
            {
                if (movement.x > 0) transform.eulerAngles = new Vector3(0, 90, 0);
                else if (movement.x < 0) transform.eulerAngles = new Vector3(0, -90, 0);

                else if (movement.y > 0) transform.eulerAngles = new Vector3(0, 0, 0);
                else if (movement.y < 0) transform.eulerAngles = new Vector3(0, 180, 0);

                myAnimator.SetBool("walking", true);
                setAttackTiles(movement);

                return true;
            }
        }

        readyForNextTurn = true;

        return false;
    }

    private bool attack()
    {
        if (hurt())
        {
            myAnimator.SetTrigger("hurt");
            resetTurnIn(2f);
            return true;
        }


        return false;
    }

    private void setAttackTiles(Vector2Int movement)
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
        _addTileToAttackList(movement.x, movement.y);
        _addTileToAttackList(movement.x * 2, movement.y * 2);


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
                player.GetComponent<Player>().hurt(1);

                return true;

            }

        }
        return false;

    }
}
