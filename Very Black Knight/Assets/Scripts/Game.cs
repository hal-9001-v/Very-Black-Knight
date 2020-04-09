using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//Author: Vic
//Game class is made to link elements within the game. Thus, it can  be used to modify many elements.

public class Game : MonoBehaviour
{
    //Tiletag is the tag name which floor tiles have
    public string tileTag { get; private set; }

    //Size in the grid
    [SerializeField]
    public float cellSize { get; private set; }

    public GameObject playerObject;
    Player myPlayerScript;

    //Enemies list
    List<Enemy> enemiesList;

    //List of floor tiles
    GridTilePro[] tiles;

    bool enemyTurn = true;

    public UnityEvent atEndTile;

    //This function is called to check wether floor tiles are next to the given coordinates, thus they are accessble
    public bool playerCanMakeMovement(float x, float y)
    {
        //We have to check every floor tile we have
        foreach (GridTilePro gt in tiles)
        {


            if (gt.movable(x, y))
            {
                if (gt.endingTile)
                {
                    Debug.Log("End of Scene");
                    atEndTile.Invoke();
                }

                return true;

            }
        }

        return false;
    }

    public bool enemyCanMakeMovement(float x, float y)
    {
        //We have to check every floor tile we have
        foreach (GridTilePro gt in tiles)
        {
            /*if (gt.movable(x, y))
            {
                if (gt.endingTile)
                {
                    Debug.Log("End of Scene");
                    atEndTile.Invoke();
                    return true;
                }

                return gt.tileEffects();

            }*/

            if (gt.movable(x, y)) {
                return true;
            }

        }

        return false;
    }


    void Awake()
    {
        tileTag = "tile";
        cellSize = 1;

        myPlayerScript = playerObject.GetComponent<Player>();

        enemiesList = new List<Enemy>();

        //Find enemies in scene
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            enemiesList.Add(enemy.GetComponent<Enemy>());
        }

        //Tiles will be gotten from every gameObject whose tag is "tileTag"
        GameObject[] auxiliarGO = GameObject.FindGameObjectsWithTag(tileTag);
        tiles = new GridTilePro[auxiliarGO.Length];

        for (int i = 0; i < tiles.Length; i++)
        {
            tiles[i] = auxiliarGO[i].GetComponent<GridTilePro>();

            if (tiles[i] == null) {
                Debug.LogError("NULL: "+auxiliarGO[i].name);
            }
        }

        Debug.Log("Current Tiles in the scene: " + tiles.Length);

        //There's only a starting point in our scene, we have to count how many of them we have
        int startingTileCounter = 0;

        //This loop can be used to configure the scene
        foreach (GridTilePro gt in tiles)
        {
                //Moving player to the starting point
                if (gt.startingTile)
                {
                    startingTileCounter++;

                    Vector3 aux = gt.transform.position;
                    aux.y = playerObject.transform.position.y;

                    playerObject.transform.position = aux;

                }

        }

        //There is only one Ending tile per Scene
        if (startingTileCounter > 1)
        {
            Debug.LogError("There are " + startingTileCounter + " starting Tiles");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyTurn)
        {
            if (myPlayerScript.hasFinishedTurn())
            {
                enemyTurn = false;
                StartCoroutine(EnemyMoves());

            }

        }
    }


    IEnumerator EnemyMoves()
    {
        foreach (Enemy enemyScript in enemiesList)
        {
            enemyScript.startTurn();

            yield return 0;
        }

        enemyTurn = true;
    }

    public GameObject getTile(Vector3 positionVector)
    {
        foreach (GridTilePro gt in tiles)
        {
            if (gt.movable(positionVector.x, positionVector.z))
            {
                return gt.gameObject;
            }
        }

        return null;
    }

}
