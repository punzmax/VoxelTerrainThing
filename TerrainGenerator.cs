using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class TerrainGenerator 
{
    private int chunkSize;
    int size;
    int[] terrainData;
    HeightMapGenerator h;
    int[] heightmap;
    public Vector3Int[] highestVoxels;
    public int min;
    public int max;

    public TerrainGenerator(int chunkSize, int size)
    {
        this.chunkSize = chunkSize;
        this.size = size;

        terrainData = new int[chunkSize * chunkSize *chunkSize];
        h = new HeightMapGenerator();

        min = 0;
        max = 0;

        heightmap = new int[size * size];
        highestVoxels = new Vector3Int[size * size];



        //generate heightmap first, so we only have to do it once for each y direction, this comes in handy when dealing with warped noise
        //this is stored as a 4D array to speed it up
        
        for (int x = 0; x < size; x++)
        {
            for (int z = 0; z < size; z++)
            {
                    
                heightmap[x + (z * size)] = h.GetMaskedHeightMap(x, z, 512, 0);


                highestVoxels[x + (z * size)] = new Vector3Int(x, heightmap[x + (z * size)], z);

                UpdateMinMax(heightmap[x + (z * size)]);
            }
        }
     }


    void DebugTerrain()
    {
        terrainData = new int[chunkSize * chunkSize * chunkSize];

        for(int x = 0; x < chunkSize; x++)
        {
            for (int y = 0; y < chunkSize; y++)
            {
                for (int z = 0; z < chunkSize; z++)
                {

                    if(y <= 1)
                    {
                        terrainData[I.Get(x, y, z, chunkSize)] = 1; //dirt
                    } else if(y == 2)
                    {
                        terrainData[I.Get(x, y, z, chunkSize)] = 2; // grass
                    }
                }
            }
        }
    }

    //Generates the terrain and fills the terrainData array with the number of the voxelType
    //for debug reasons im filling it with dirt up to the half of the size
    void GenerateTerrain(Vector3Int pos)
    { 
        terrainData = new int[chunkSize * chunkSize * chunkSize] ; 
        for (int x = 0; x < chunkSize; x++)
        {
            for (int y = 0; y < chunkSize; y++)
            {
                for (int z = 0; z < chunkSize; z++)
                {

                   
                    
                   if (y + (pos.y * chunkSize) + 1 < heightmap[((pos.x * chunkSize) + x + ((pos.z * chunkSize) + z ) * size)])
                   {

                        terrainData[I.Get(x, y, z, chunkSize)] = 4; //Stone

                   }
                    else if (y + (pos.y * chunkSize) + 1 == heightmap[(pos.x * chunkSize) + x + (((pos.z * chunkSize) + z) * size)])
                    {
                        terrainData[I.Get(x, y, z, chunkSize)] = 1; //Dirt
                    }
                    else if (y + (pos.y * chunkSize) == heightmap[(pos.x * chunkSize) + x + (((pos.z * chunkSize) + z) * size)])
                    {
                        terrainData[I.Get(x, y, z, chunkSize)] = 2; //GRass

                        
                           
                    }
                   



                }
            }
        }
    }

    public void UpdateMinMax(int y)
    {
        if(y < min)
        {
            min = y;
        }

        if(y > max)
        {
            max = y;
        }
    }

    public int[] GetTerrainData(Vector3Int pos)
    {
        GenerateTerrain(pos);
        

        return terrainData;
    }


    public int[] GetDebugTerrain()
    {
        DebugTerrain();
        AddDebugFoliage();

        return terrainData;
    }



    public void AddDebugFoliage()
    {
        for (int x = 0; x < chunkSize; x++)
        {
            for (int y = 0; y < chunkSize; y++)
            {
                for (int z = 0; z < chunkSize; z++)
                {
                    if(terrainData[I.Get(x, y, z, chunkSize)] == 2)
                    {
                        terrainData[I.Get(x, y + 1, z, chunkSize)] = 14; //GrassFoliage
                    }
                }
            }
        }
    }


    public Vector3Int GetHighestVoxelPosition(int x, int z)
    {
        return highestVoxels[x + (z * size)];
    }

    public void SetHighestVoxelPosition(int x, int z, Vector3Int pos)
    {
         highestVoxels[x + (z * size)] = pos;
    }



}
