using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassMaker : AbstractFoliageMaker
{
    float voxelSize;

    public GrassMaker(float voxelSize, Material mat) : base(voxelSize, mat)
    {
        this.voxelSize = voxelSize;
    }

    public void MakeGrass(Chunk c, int x, int y, int z)
    {

        float pos_x = ((c.pos.x * c.chunkSize) + x) * voxelSize + (voxelSize / 16 * Random.Range(0, 15));
        float pos_y = ((c.pos.y * c.chunkSize) + y);
        float pos_z = ((c.pos.z * c.chunkSize) + z) * voxelSize + (voxelSize / 16 * Random.Range(0, 15)) ;

       

        Build(new Vector3(pos_x, pos_y, pos_z), voxelSize, voxelSize);
    }


}
