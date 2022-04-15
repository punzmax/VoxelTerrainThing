using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleFaceBuilder : FaceBuilder
{


    public SimpleFaceBuilder()
    {

    }

    

    public new void Build(Chunk c, Vector3Int pos, List<int> triangles, List<Vector3> verts, float voxelSize)
    {
        List<Vector3> used_Verts = new List<Vector3>();
        List<int> t_triangles = new List<int>();

        Vector3[] cube_verts = new Vector3[8];

        float x = pos.x * voxelSize + (c.pos.x * c.chunkSize * voxelSize);
        float y = pos.y * voxelSize + (c.pos.y * c.chunkSize * voxelSize);
        float z = pos.z * voxelSize + (c.pos.z * c.chunkSize * voxelSize);

        cube_verts[0] = new Vector3(x, y, z);
        cube_verts[1] = new Vector3(x + voxelSize, y, z);
        cube_verts[2] = new Vector3(x + voxelSize, y + voxelSize, z);
        cube_verts[3] = new Vector3(x, y + voxelSize, z);

        cube_verts[4] = new Vector3(x, y + voxelSize, z + voxelSize);
        cube_verts[5] = new Vector3(x + voxelSize, y + voxelSize, z + voxelSize);
        cube_verts[6] = new Vector3(x + voxelSize, y, z + voxelSize);
        cube_verts[7] = new Vector3(x, y, z + voxelSize);

        
        
        if (!c.HasFrontNeighbor(pos))
        {
            AddVertIndices(t_triangles, new int[] { 0, 2, 1, 0, 3, 2});
        }
        if (!c.HasBackNeighbor(pos))
        {
            AddVertIndices(t_triangles, new int[] { 5, 4, 7, 5, 7, 6 });
        }
        if (!c.HasLeftNeighbor(pos))
        {
            AddVertIndices(t_triangles, new int[] { 0, 7, 4, 0, 4, 3 });
        }
        if (!c.HasRightNeighbor(pos))
        {
            AddVertIndices(t_triangles, new int[] { 1, 2, 5, 1, 5, 6 });
        }
        if (!c.HasTopNeighbor(pos))
        {
            AddVertIndices(t_triangles, new int[] { 2, 3, 4, 2, 4, 5 });
        }
        if (!c.HasBottomNeighbor(pos))
        {
            AddVertIndices(t_triangles, new int[] {0, 6, 7, 0, 1, 6});
        }

        

        List<int> usedVertIndices = CalculateUsedVerts(t_triangles, cube_verts, used_Verts,  verts);

       

        triangles.AddRange(usedVertIndices);
        verts.AddRange(used_Verts);
    }

    

}
