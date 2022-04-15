using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainDrop : Agent
{
    bool lastStepDeposited = false;

    Vector3Int[] neighbors = {
        new Vector3Int(-1, 0, -1),
        new Vector3Int(0, 0, -1),
        new Vector3Int(1, 0, -1),

        new Vector3Int(-1, 0, 0),
        new Vector3Int(1, 0, 0),

        new Vector3Int(-1, 0, 1),
        new Vector3Int(0, 0, 1),
        new Vector3Int(1, 0, 1)
    };

    List<Vector3Int> validNeighbors;

    public RainDrop(Vector3Int pos, ErosionMaker e) : base(pos, e)
    {
        validNeighbors = new List<Vector3Int>();
    }


    override public void Step()
    {
        Vector3Int lowestPosition = new Vector3Int(0, int.MaxValue, 0);

        for(int i = 0; i < neighbors.Length; i++)
        {
            if(InBounds(pos.x + neighbors[i].x, pos.z + neighbors[i].z))
            {
                

                validNeighbors.Add(parent.v.GetPositionOfHighestVoxel(pos.x + neighbors[i].x, pos.z + neighbors[i].z));
            }
        }


        for(int i = 0; i < validNeighbors.Count; i++)
        {
            if(lowestPosition == null)
            {
                lowestPosition = validNeighbors[i];

            } else if (validNeighbors[i].y < lowestPosition.y)
            {
                lowestPosition = validNeighbors[i];
            }
        }


        if (lowestPosition.y < pos.y)
        {
            if(lastStepDeposited == false)
            {
                Errode();

            }

            pos = lowestPosition;
        } else
        {
            
            if(distanceTraveled > 0)
            {
                Deposit();
               

            } else
            {
                lastStepDeposited = false;
                pos = parent.GetValidStartPos();
                distanceTraveled = 0;

            } 

        }

        validNeighbors.Clear();

    }


    void Errode()
    {
        //Debug.Log("Erroded");

        lastStepDeposited = false;

        
        parent.v.SetVoxel(pos, 0);

        distanceTraveled += 1;

        Vector3Int t = new Vector3Int(pos.x, pos.y-1, pos.z);

        ((ErosionMaker) parent).AddToHeightQueue(t);

    }

    void Deposit()
    {
        //Debug.Log("Deposited");

        this.pos.y += 1;
        lastStepDeposited = true;
        //distanceTraveled -= (int) Random.Range(1, 3);
        distanceTraveled -= 1;
        parent.v.SetVoxel(pos, 1);

        Vector3Int t = new Vector3Int(pos.x, pos.y, pos.z);
        ((ErosionMaker)parent).AddToHeightQueue(t);



    }

    
}
