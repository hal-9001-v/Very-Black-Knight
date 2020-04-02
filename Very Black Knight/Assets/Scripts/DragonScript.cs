using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Priority_Queue;
public class DragonScript : Enemy
{
    private int currentState = 0;
    GameObject player;
    bool readyForNextTurn = true;

    List<GameObject> attackTiles;
    List<Vector2> movementList;

    bool delay = false;
    float waitTime;
    float currentTime;

    float baseTimeToReach;

    enum State
    {
        idle = 0,
        walking = 1
    }

    public override void startTurn()
    {
        if (readyForNextTurn)
        {
            readyForNextTurn = false;

            if (hurt())
            {
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
                    else {
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




                        if (move())
                        {
                            currentState = (int)State.walking;
                            myAnimator.SetBool("walking", true);

                        }
                        else
                        {
                            readyForNextTurn = true;
                        }

                    
                    break;

                //Walking
                case 1:
                    // Look at endOfMovement() due to synchronization 

                    break;
            }
        }
    }
    public override void endOfMovementActions()
    {
        readyForNextTurn = true;
        currentState = (int)State.idle;
        myAnimator.SetBool("walking", false);

        setAttackTiles();

    }

    private bool move()
    {
        foreach (Vector2 movement in movementList)
        {
            if (canMakeMovement(movement.x, movement.y))
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

            mr.material.color -= attackColor;
            
        }

        attackTiles.Clear();
        Vector3 auxiliarVector;

        auxiliarVector = transform.position;
        attackTiles.Add(game.getTile(auxiliarVector));


        if (tileExists(1, 0))
        {
            auxiliarVector = transform.position;
            auxiliarVector.x += cellSize;
            attackTiles.Add(game.getTile(auxiliarVector));
        }

        if (tileExists(-1, 0))
        {
            auxiliarVector = transform.position;
            auxiliarVector.x -= cellSize;
            attackTiles.Add(game.getTile(auxiliarVector));
        }

        if (tileExists(0, 1))
        {
            auxiliarVector = transform.position;
            auxiliarVector.z += cellSize;
            attackTiles.Add(game.getTile(auxiliarVector));
        }

        if (tileExists(0, -1))
        {
            auxiliarVector = transform.position;
            auxiliarVector.z -= cellSize;
            attackTiles.Add(game.getTile(auxiliarVector));
        }

        if (tileExists(1, 1))
        {
            auxiliarVector = transform.position;
            auxiliarVector.x += cellSize;
            auxiliarVector.z += cellSize;
            attackTiles.Add(game.getTile(auxiliarVector));
        }

        if (tileExists(-1, 1))
        {
            auxiliarVector = transform.position;
            auxiliarVector.x -= cellSize;
            auxiliarVector.z += cellSize;
            attackTiles.Add(game.getTile(auxiliarVector));
        }

        if (tileExists(1, -1))
        {
            auxiliarVector = transform.position;
            auxiliarVector.x += cellSize;
            auxiliarVector.z -= cellSize;
            attackTiles.Add(game.getTile(auxiliarVector));
        }

        if (tileExists(-1, -1))
        {
            auxiliarVector = transform.position;
            auxiliarVector.x -= cellSize;
            auxiliarVector.z -= cellSize;
            attackTiles.Add(game.getTile(auxiliarVector));
        }

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


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("player");
        movementList = new List<Vector2>();

        baseTimeToReach = timeToReach;

        initialize();

        attackTiles = new List<GameObject>();
    }

    void Update()
    {
        delayFunction();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        makeMovement();
    }

    void resetTurnIn(float t)
    {
        waitTime = t;
        currentTime = 0;

        delay = true;
    }

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

            if (tile.GetComponent<GridTile>().movable(player.transform.position.x, player.transform.position.z))
            {
                player.GetComponent<Player>().hurt(2);

                return true;

            }

        }
        return false;

    }

}
