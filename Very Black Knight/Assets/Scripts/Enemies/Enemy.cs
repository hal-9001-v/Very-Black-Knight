using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    protected float cellSize;
    protected Game game;

    public float timeToReach;
    float timeCounter;
    protected bool doingMovement;

    protected GameObject currentTile;

    Color floorColor;
    protected Color attackColor;

    Vector3 startingPosition;
    Vector3 newPosition;
    Vector3 direction;

    protected List<GameObject> attackTiles;

    public abstract void _startTurn();
    public abstract void _endOfMovementActions();

    protected void _initialize()
    {
        game = GameObject.Find("GameController").GetComponent<Game>();
        attackTiles = new List<GameObject>();

        cellSize = game.cellSize;

        floorColor = new Color(1, 0, 0);
        attackColor = new Color(0.5f, 0, 0);
        currentTile = game.getTile(transform.position);

        MeshRenderer mr = currentTile.GetComponent<MeshRenderer>();

        if (mr == null)
        {
            mr = currentTile.GetComponentInChildren<MeshRenderer>();
        }

        mr.material.color += floorColor;

    }

    protected void _addTileToAttackList(int x, int z)
    {
        if (_tileExistsLocal(x, z))
        {
            Vector3 auxiliarVector;

            auxiliarVector = transform.position;
            auxiliarVector.x += cellSize * x;
            auxiliarVector.z += cellSize * z;
    
            attackTiles.Add(game.getTile(auxiliarVector));
        }

    }

    protected bool _canMakeMovement(float xMove, float zMove)
    {
        if (!doingMovement)
        {
            Vector3 movementVector = new Vector3();

            //Transform desired destination into grid coordinates
            movementVector.x = Mathf.Round((transform.position.x + xMove * cellSize) / cellSize) * cellSize;
            movementVector.z = Mathf.Round((transform.position.z + zMove * cellSize) / cellSize) * cellSize;

            //Start movement
            if (game.enemyCanMakeMovement(movementVector.x, movementVector.z))
            {
                //It is necesarry to store point B for Interpolation
                startingPosition = transform.position;


                //It is necesarry to store point B for Interpolation
                newPosition = movementVector;

                //We make sure the player's height is not the tile's height
                newPosition.y = transform.position.y;

                _applyColorToFloor(newPosition);


                doingMovement = true;
                timeCounter = 0;

                direction = newPosition - startingPosition;

                direction = Vector3.Normalize(direction);

                return true;
            }
        }
        return false;

    }

    //Fixed Update
    protected void _makeMovement()
    {
        //Movement on the player are done if dointMovement is true
        if (doingMovement)
        {
            //Divide by timeToReach implies it will take such time until arrival
            timeCounter += Time.deltaTime / timeToReach;
            transform.position = Vector3.Lerp(startingPosition, newPosition, timeCounter);

            //Aproximating to the point
            if (Vector3.Distance(transform.position, newPosition) < 0.05)
            {
                transform.position = newPosition;
                doingMovement = false;

                _endOfMovementActions();

            }
        }
    }

    //Checks wether a tile exists in current position + (x,y)*cellSize on grid
    protected bool _tileExistsLocal(float x, float y)
    {
        Vector2 auxiliarVector = new Vector2();

        //Transform desired destination into grid coordinates
        auxiliarVector.x = Mathf.Round((transform.position.x + x * cellSize) / cellSize) * cellSize;
        auxiliarVector.y = Mathf.Round((transform.position.z + y * cellSize) / cellSize) * cellSize;

        return game.enemyCanMakeMovement(auxiliarVector.x, auxiliarVector.y);

    }

    void _applyColorToFloor(Vector3 positionVector)
    {
        MeshRenderer mr;

        mr = currentTile.GetComponent<MeshRenderer>();

        if (mr == null)
        {
            mr = currentTile.GetComponentInChildren<MeshRenderer>();
        }
        mr.material.color -= floorColor;

        currentTile = game.getTile(positionVector);

        mr = currentTile.GetComponent<MeshRenderer>();

        if (mr == null)
        {
            mr = currentTile.GetComponentInChildren<MeshRenderer>();
        }
        mr.material.color += floorColor;


    }
}