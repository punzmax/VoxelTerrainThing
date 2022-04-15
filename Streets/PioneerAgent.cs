using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PioneerAgent : Agent
{
   
    Vector2Int direction;
    Vector3Int offset;
    Vector3Int nextPositon;

    float randomSeed;
    float randomChance = 0.2f;
    int maximumHeightDifference = 1;
    public Vector3Int startPos;
    public int numResets = 0;


    public PioneerAgent(Vector3Int pos, Vector2Int dir,  StreetMaker s) : base(pos, s)
    {
    
        this.startPos = pos;
        this.direction = dir;

        parent.v.SetVoxel(pos, 7);
  
        randomSeed = Random.Range(0.0f, 1.0f);

    }

    override public void Step()
    {
        CalculateOffset();

        int x = pos.x + direction.x + offset.x;
        int z = pos.z + direction.y + offset.z;

        if (InBounds(x, z) )
        {

            nextPositon = parent.v.GetPositionOfHighestVoxel(x, z);

            if (ValidHeight() )
            {
                if (ValidVoxel())
                {
                    pos = nextPositon;




                    parent.v.SetVoxel(pos, 7);

                    distanceTraveled += 1;
                    
              
                    if (distanceTraveled >= 32 && Random.Range(0.0f, 1.0f) > 0.5)
                    {
                        distanceTraveled = 0;
                        ((StreetMaker)parent).SpawnPioneer(pos, direction);
                    }
                } else
                {
                    parent.Terminate(this);
                }
                
            } else
            {
                Reset();
            }
            
        }
        else
        {
            Reset();
        }


    }

    

   bool ValidHeight()
    {
        if(pos.y - nextPositon.y > maximumHeightDifference || pos.y - nextPositon.y < -maximumHeightDifference)
        {
            if(pos.y - nextPositon.y  == maximumHeightDifference + 1)
            {
                nextPositon.y += 1;
            } else if (pos.y - nextPositon.y < -maximumHeightDifference - 1)
            {
                nextPositon.y -= 1;
            }

            return false;

        }
       


        return true;
    }
    
    bool ValidVoxel()
    {
        if(parent.v.GetVoxelType(nextPositon) == 7)
        {
            


            

            return false;
        }

        return true;
    }

    void CalculateOffset()
    {
        
        

        if(Random.Range(0.0f, 1.0f) < randomChance)
        {
            float rand = Mathf.PerlinNoise(pos.x + randomSeed, pos.z + randomSeed);
            

            if (rand >= 0.5)
            {
                offset.x = 1;
                offset.z = 1;
            }
            else
            {
                offset.x = -1;
                offset.z = -1;
            }


        } else
        {
            offset.x = 0;
            offset.z = 0;
        }


        if(direction.x != 0)
        {
            offset.x = 0;
        }

        if(direction.y != 0)
        {
            offset.z = 0;
        }

        
    }


  
    void Reset()
    {
        if (numResets == 0)
        {
           
            numResets = 1;
            pos = startPos;
            direction = new Vector2Int(direction.x * -1, direction.y  * -1);
        } else
        {
            
            parent.Terminate(this);
        }
    }
}