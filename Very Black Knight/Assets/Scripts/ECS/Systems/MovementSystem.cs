using UnityEngine;
using Unity.Entities;
using Unity.Transforms;

public class MovementSystem : ComponentSystem
{
    Vector3 currentPosition;
    public static bool playerIsMoving { get; private set; }

    float tolerance = 0.1f;
    float tileSpacing = 1;

    private bool playerIsSet;

    protected override void OnUpdate()
    {
        setPlayerInStartTile();


        if (Input.GetKeyDown(KeyCode.A))
        {
            //Run over all entities in order to find enough proximity between the desired destination position and tiles positions
            Entities.ForEach((ref TileComponent tc) =>
            {
                Debug.Log("Moving");
                if (tileExists(currentPosition.x + tileSpacing, currentPosition.z, ref tc))
                {
                    Debug.Log("Found");
                    currentPosition = new Vector3(currentPosition.x + tileSpacing, currentPosition.y, currentPosition.z);
                    ECSBrain.playerCanMakeMovement = false;
                    movePlayer();
                    return;
                }
            });
        }




    }

    private void movePlayer()
    {
        Entities.ForEach((ref PlayerComponent pc, ref Translation translation) =>
        {
            Debug.Log("PlayerFound");
            translation.Value = currentPosition;

        });
    }



    private bool tileExists(float x, float z, ref TileComponent tc)
    {
        //Only a certain distance is accepted. Tolerance is ned due to float accuaracy limits, so too much distance in X or Z isn't accepted
        if (Mathf.Abs(x - tc.position.x) < tolerance)
        {
            if (Mathf.Abs(z - tc.position.y) < tolerance)
            {
                return true;
            }
        }


        return false;
    }

    private void setPlayerInStartTile()
    {

        if (!playerIsSet)
        {
            playerIsSet = true;

            Entities.ForEach((ref TileComponent tc) =>
            {

                if (tc.startingTile == true)
                {

                    currentPosition.x = tc.position.x;
                    currentPosition.z = tc.position.y;

                    movePlayer();
                    return;
                }

            });
            Debug.LogError("No Starting Tile!");
        }


    }
}
