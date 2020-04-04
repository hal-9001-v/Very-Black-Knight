using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;


public class ECSBrain : MonoBehaviour
{
    EntityManager entityManager;

    public static Vector3 playerPosition { get; private set; }
    
    [SerializeField]
    public float tileSpacing { get; private set; }

    public static bool playerCanMakeMovement;

    // Start is called before the first frame update
    void Start()
    {
        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        playerCanMakeMovement = true;

        
    }

    public static void setPlayerPosition(float x, float z) {
        playerPosition = new Vector3(x,playerPosition.y,z);
    }

}
