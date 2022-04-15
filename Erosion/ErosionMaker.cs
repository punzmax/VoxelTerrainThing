using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ErosionMaker : AgentManager
{

    List<Vector3Int> heightUpdateQueue;

  

   

    public ErosionMaker(int numAgents, VoxelWorld v) : base(numAgents, v)
    {

        heightUpdateQueue = new List<Vector3Int>();

        for (int i = 0; i < numAgents; i++)
        {
            agents.Add(new RainDrop(GetValidStartPos(), this));
        }

        


    }

    override public Vector3Int GetValidStartPos()
    {
        bool foundPos = false;

        int range = v.terrainGenerator.max - v.terrainGenerator.min;
        int min = v.terrainGenerator.min;
        Vector3Int pos = new Vector3Int();



        while (foundPos == false)
        {

            int x = Random.Range(0, v.GetSize() - 1);
            int z = Random.Range(0, v.GetSize() - 1);

            pos = v.GetPositionOfHighestVoxel(x, z);


            if (pos.y > min + Mathf.RoundToInt(range * 0.3f))
            {
                foundPos = true;
            }


        }



        return pos;
    }


    public void UpdateHeights()
    {
        for(int i = 0; i < heightUpdateQueue.Count; i++)
        {
            v.SetHighestPosition(heightUpdateQueue[i]);

        }


        heightUpdateQueue.Clear();
    }

    

    public void AddToHeightQueue(Vector3Int newPos)
    {
        heightUpdateQueue.Add(newPos);
    }

}
