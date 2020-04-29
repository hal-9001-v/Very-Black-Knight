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

    [Range(10, 200)]
    public float range = 20;

    public bool followDebug;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("player");
        myAnimator = gameObject.GetComponent<Animator>();

        movementList = new List<Vector2Int>();

        _initialize();
    }

    public override void _startTurn()
    {

        //If player is away from guard, no action is taken
        if (Vector3.Distance(transform.position, player.transform.position) > range)
        {
            return;
        }
        else
        {
            //Debugging on console if guard is following player
            if (followDebug)
            {
                Debug.Log("Guard: " + gameObject.name + " is in follow range");
            }
        }

        if (readyForNextTurn)
        {
            readyForNextTurn = false;

            //Attack resets turn
            bool attackIsDone = attack();
            if (attackIsDone) return;

            calculateMovements();
            tryToMakeMovement();

        }
    }

    //Turn is reset when movement ends
    public override void _endOfMovementActions()
    {
        readyForNextTurn = true;
        myAnimator.SetBool("walking", false);

    }

    private void calculateMovements()
    {
        setMovementList(Random.Range(0, 3));

    }

    void setMovementList(int i)
    {

        movementList.Clear();

        //Several order on chosing new movement directions. With this, moves can be more unpredictable
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
                //Rotations according to taken movement
                if (movement.x > 0) transform.eulerAngles = new Vector3(0, 90, 0);
                else if (movement.x < 0) transform.eulerAngles = new Vector3(0, -90, 0);

                else if (movement.y > 0) transform.eulerAngles = new Vector3(0, 0, 0);
                else if (movement.y < 0) transform.eulerAngles = new Vector3(0, 180, 0);

                myAnimator.SetBool("walking", true);

                //Add tiles under attack to list
                setAttackTiles(movement);

                return true;
            }
        }

        readyForNextTurn = true;

        return false;
    }

    private bool attack()
    {
        //If player has been attacked
        if (hurt())
        {
            myAnimator.SetTrigger("hurt");

            //After 2 seconds turn will be ready
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
            //Paint new current tile
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
        //Check if player is on any of the under attack tiles
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
