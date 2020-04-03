using UnityEngine;
using Unity.Entities;

public class MovementSystem : ComponentSystem
{
    Vector3 currentPosition;
    public static bool playerIsMoving { get; private set; }

    float tolerance;

    protected override void OnUpdate()
    {
        if (ECSBrain.playerCanMakeMovement)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                //Run over all entities in order to find enough proximity between the desired destination position and tiles positions
                Entities.ForEach((ref TileComponent tc) =>
                {
                    if (tileExists(currentPosition.x + ECSBrain.tileSpacing, currentPosition.z + ECSBrain.tileSpacing, ref tc))
                    {
                        currentPosition = new Vector3(currentPosition.x + ECSBrain.tileSpacing, currentPosition.y, currentPosition.z + ECSBrain.tileSpacing);
                        ECSBrain.playerCanMakeMovement = false;
                        return;
                    }
                });
            }
        }

    }

    private bool tileExists(float x, float z, ref TileComponent tc)
    {
        //Only a certain distance is accepted. Tolerance is ned due to float accuaracy limits, so too much distance in X or Z isn't accepted
        if (Mathf.Abs(x - tc.xPosition) < tolerance)
        {
            if (Mathf.Abs(y - tc.zPosition) < tolerance)
            {
                return true;
            }
        }


        return false;
    }
}
