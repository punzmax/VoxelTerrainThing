using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StreetMaker : AgentManager
{
  
   

    public StreetMaker(int numAgents, VoxelWorld v) : base(numAgents, v)
    {
        for(int i = 0; i < numAgents; i++)
        {
            Vector2Int direction = new Vector2Int(0, 1);

            agents.Add(new PioneerAgent(GetValidStartPos(), direction, this));
  
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


            if (pos.y < min + Mathf.RoundToInt(range * 0.5f))
            {
                foundPos = true;
            }


        }
        
        

        return pos;
    }



    public bool Done()
    {
        if(agents.Count == 0)
        {
            return true;
        }

        return false;
    }
    public void SpawnPioneer(Vector3Int pos, Vector2Int direction)
    {
        float rand = Random.Range(0.0f, 1.0f);
        int flip = 1;

        if(rand >= 0.5f)
        {
            flip = -1;
        }

        Vector2Int newDir = new Vector2Int(direction.y * flip, direction.x * flip);


        agents.Add(new PioneerAgent(pos, newDir,  this)) ;
    }


    

   
}
