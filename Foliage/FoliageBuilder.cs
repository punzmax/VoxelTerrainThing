using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoliageBuilder 
{
    float voxelSize;
    GrassMaker grassMaker;
    public Material mat;

    public FoliageBuilder(float voxelSize, Material mat)
    {
        this.voxelSize = voxelSize;
        this.mat = mat;

        grassMaker = new GrassMaker(voxelSize, mat);
    }



    public void MakeFoliage(Chunk c)
    {
        for (int x = 0; x < c.chunkSize; x++)
        {
            for (int y = 0; y < c.chunkSize; y++)
            {
                for (int z = 0; z < c.chunkSize; z++)
                {
                    if(c.GetBlockType(x, y, z) == 14)
                    {
                        grassMaker.MakeGrass(c, x, y, z);
                        grassMaker.MakeGrass(c, x, y, z);
                        grassMaker.MakeGrass(c, x, y, z);
                        grassMaker.MakeGrass(c, x, y, z);

                        
                    }


                }
            }
        }
    }
}
