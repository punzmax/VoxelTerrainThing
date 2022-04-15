using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaveGenerator 
{
    int width;
    int height;
    int depth;
    int numSteps;

    int[] values;

    Vector3Int currentPos;

    public CaveGenerator(int size, int height, int numSteps)
    {
        this.width = size;
        this.height = height;
        this.depth = size;
        this.numSteps = numSteps;

        currentPos = new Vector3Int();

        currentPos.x = Random.Range(0, size);
        currentPos.y = Random.Range(0, height);
        currentPos.z = Random.Range(0, size);


        values = MakeCave();
    }


    public int[] MakeCave()
    {
        values = new int[width * height * depth];

        for (int i = 0; i < values.Length; i++)
        {
            values[i] = 0;
        }

        
        for (int i = 0; i < numSteps; i++)
        {
            currentPos = Step(i, currentPos);
            values[I.Get(currentPos.x, currentPos.y, currentPos.z, width)] = 1;
        }
        

        return values;
    }

    private Vector3Int Step(int i, Vector3Int pos)
    {
        pos.x = CheckBounds(MoveInDirection(i / numSteps, 0.323f, pos.x), width);
        pos.y = CheckBounds(MoveInDirection(i / numSteps, 0.1289f, pos.y), height);
        pos.z = CheckBounds(MoveInDirection(i / numSteps, 0.65467f, pos.z), width);

        return pos;
    } 


    private int MoveInDirection(float i, float offset, int pos)
    {
        float n1 = Mathf.PerlinNoise(i, offset) * 2;
        Debug.Log(n1);

        if (n1 < 1 / 3)
        {
            pos--;
        } else if (n1 > 2 / 3)
        {
            pos++;
        }
        

        return pos;
    }

    private int CheckBounds(int val, int max)
    {
        if(val < 0)
        {
            val = max - 1;
        }  

        if(val >= max)
        {
            val = 0;
        }
         

        return val;
    }


    public int[] GetDebugData(Vector3Int chunk, int chunkSize)
    {
        
        int[] val = new int[chunkSize * chunkSize * chunkSize];

        for (int x = 0; x < chunkSize; x++)
        {
            for (int y = 0; y < chunkSize; y++)
            {
                for (int z = 0; z < chunkSize; z++)
                {
                    val[I.Get(x, y, z, chunkSize)] = values[I.Get(x + (chunk.x * chunkSize), y + (chunk.y * chunkSize), z + (chunk.z * chunkSize), width)];
                    
                }
            }
        }


        

        return val;
    }

    

}
