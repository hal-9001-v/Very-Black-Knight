using UnityEngine;

//Author: Vic
public class PlayerMovement : MonoBehaviour
{
    private GameObject gameContainerObject;
    private Game game;

    //Width of cells
    private float cellSize;

    //Time to finish movement
    public float MAXTIMETOREACH { get; private set; }
    public float timeToReach { get; private set; }
    private float timeCounter;

    [HideInInspector]
    public bool doingMovement = false;
    public bool inputEnable = true;

    //Starting and ending position are used of interpolation
    private Vector3 newPosition;
    private Vector3 startingPosition;

    Vector3 direction;

    public GridTilePro currentTile;

    public int inputCount { get; set; }

    //Player Movement Vectors
    Vector3 FWVector = new Vector3(1,0,0);
    Vector3 BWVector = new Vector3(-1,0,0);
    Vector3 RVector = new Vector3(0,0,-1);
    Vector3 LVector = new Vector3(0,0,1);

    bool newDirections = false;
    Quaternion newRotation;

    void Start()
    {
        gameContainerObject = GameObject.Find("GameController");
        game = gameContainerObject.GetComponent<Game>();
        cellSize = game.cellSize;

        MAXTIMETOREACH = 0.5f;
        timeToReach = MAXTIMETOREACH;

    }

    // Update is called once per frame
    public bool checkInput()
    {

        //Movement Input is only possible if the object is not moving
        if (!doingMovement && inputEnable)
        {
            bool keyPressed = false;


            //Forward
            if (Input.GetKey(KeyCode.W) | Input.GetKeyDown(KeyCode.UpArrow))
            {
                keyPressed = true;
                if (canMakeMovement(FWVector.x, FWVector.z))
                {
                    //Transform angles to face direction
                    Vector3 newRotation = Quaternion.LookRotation(FWVector).eulerAngles;
                    
                    transform.eulerAngles = newRotation;

                    inputCount++;
                    return true;
                }

            }

            //BackWard
            if (Input.GetKey(KeyCode.S) | Input.GetKeyDown(KeyCode.DownArrow))
            {
                keyPressed = true;
                if (canMakeMovement(BWVector.x, BWVector.z))
                {
                    //Transform angles to face direction
                    Vector3 newRotation = Quaternion.LookRotation(BWVector).eulerAngles;
                    
                    transform.eulerAngles = newRotation;

                    inputCount++;
                    return true;
                }

            }

            //Right
            if (Input.GetKey(KeyCode.D) | Input.GetKeyDown(KeyCode.RightArrow))
            {
                keyPressed = true;
                if (canMakeMovement(RVector.x, RVector.z))
                {
                    //Transform angles to face direction
                    Vector3 newRotation = Quaternion.LookRotation(RVector).eulerAngles;
                  
                    transform.eulerAngles = newRotation;

                    inputCount++;
                    return true;
                }

            }

            //Left
            if (Input.GetKey(KeyCode.A) | Input.GetKeyDown(KeyCode.LeftArrow))
            {
                keyPressed = true;
                if (canMakeMovement(LVector.x, LVector.z))
                {
                    //Transform angles to face direction
                    Vector3 newRotation = Quaternion.LookRotation(LVector).eulerAngles;
                    
                    transform.eulerAngles = newRotation;

                    inputCount++;
                    return true;
                }
            }

            //New redirection is only applied when player is not pressing any key
            if (!keyPressed)
                rotateDirections();

        }
        return false;
    }

    //Physics must be done on FixedUpdate as it is always executed
    void FixedUpdate()
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
                endOfMovementActions();
            }

        }
    }

    //Every Time a new gridTile is stepped, this function must be called
    private void endOfMovementActions()
    {
        currentTile = game.getTile(transform.position).GetComponent<GridTilePro>();

        //Push is an interpolation to a certain grid tile
        if (currentTile.pushTile)
        {
            startMovement(currentTile.pushTileObject.transform.position);
            return;
        }

        //Hurt reduces health
        if (currentTile.hurtTile)
        {
            gameObject.GetComponent<Player>().hurt(currentTile.damage);
            return;
        }

        //Teleportation is a sudden change of position
        if (currentTile.teleportationTile)
        {
            Vector3 auxiliarVector = currentTile.teleportationTileObject.transform.position;
            auxiliarVector.y = gameObject.transform.position.y;

            gameObject.transform.position = auxiliarVector;

            currentTile = currentTile.teleportationTileObject.GetComponent<GridTilePro>();

            //Considering this is not a "movememnt" which calls endOfMovementFunction, it shall be called here so it is applied the new tile effect
            if (currentTile.tileEffectAfterTeleportation)
            {
                endOfMovementActions();
            }
            return;
        }

    }

    public bool canMakeMovement(float xMove, float zMove)
    {
        Vector3 movementVector = new Vector3();

        //Transform desired destination into grid coordinates
        movementVector.x = Mathf.Round((transform.position.x + xMove * cellSize) / cellSize) * cellSize;
        movementVector.z = Mathf.Round((transform.position.z + zMove * cellSize) / cellSize) * cellSize;

        //Start movement
        if (game.playerCanMakeMovement(movementVector.x, movementVector.z))
        {
            startMovement(movementVector);
            return true;
        }
        return false;
    }

    public void startMovement(Vector3 movementVector)
    {
        //It is necesarry to store point B for Interpolation
        startingPosition = transform.position;


        //It is necesarry to store point B for Interpolation
        newPosition = movementVector;

        //We make sure the player's height is not the tile's height
        newPosition.y = transform.position.y;

        doingMovement = true;
        timeCounter = 0;

        direction = newPosition - startingPosition;

        direction = Vector3.Normalize(direction);

    }

    public void setTimeToReach(float f)
    {
        if (f < 0 ) return;

        timeToReach = f;
    }

    //Apply rotation matrix into movement vectors
    private void rotateDirections() {

        if (newDirections)
        {
            FWVector = newRotation * FWVector;
            BWVector = newRotation * BWVector;
            RVector = newRotation * RVector;
            LVector = newRotation * LVector;
            newDirections = false;
        }
    }

    //When camera is rotated, movement vectors must be updated with a rotation matrix
    public void setNewDirections(Quaternion rotate) {
        newDirections = true;
        newRotation = rotate;
    }
}
