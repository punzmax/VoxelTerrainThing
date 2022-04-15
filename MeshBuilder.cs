using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshBuilder
{
    float voxelSize;

    SimpleFaceBuilder faceBuilder;
    GreedyFaceBuilder greedyFaceBuilder;

    public MeshBuilder(float voxelSize)
    {
        this.voxelSize = voxelSize;
        faceBuilder = new SimpleFaceBuilder();
        greedyFaceBuilder = new GreedyFaceBuilder(voxelSize);
    }

    
   

    public void BuildChunkMesh(Chunk c, Mesh m)
    {
        
        List<int> triangles = new List<int>();
        List<Vector3> vertices = new List<Vector3>();
        

        for (int x = 0; x < c.chunkSize; x++)
        {
            for (int y = 0; y < c.chunkSize; y++)
            {
                for (int z = 0; z < c.chunkSize; z++)
                {
                    if (c.IsOpaque(x, y, z))
                    {
                        faceBuilder.Build(c, new Vector3Int(x, y, z), triangles, vertices, voxelSize);
                    }
                }
            }
        }

       
        vertices = RemoveDuplicateVertices(vertices, triangles);
        
        
        m.Clear();
        m.vertices = vertices.ToArray();
        m.triangles = triangles.ToArray();
        //m.Optimize();
        m.RecalculateNormals();

    }




    public void BuildGreedyChunkMesh(Chunk c, Mesh m, VoxelUpdater updater)
    {
        List<int> triangles = new List<int>();
        List<Vector3> vertices = new List<Vector3>();
        List<Vector2> uv = new List<Vector2>();
        List<Vector2> typeUVs = new List<Vector2>();


        greedyFaceBuilder.Build(c, new Vector3Int(0, 0, 0), triangles, vertices, uv, typeUVs, updater);
        //greedyFaceBuilder.BuildTop(c, new Vector3Int(0, 0, 0), triangles, vertices, GreedyFaceBuilder.Direction.Bottom);




        
        //Debug.Log(vertices.Count);

        m.Clear();
        m.vertices = vertices.ToArray();
        m.triangles = triangles.ToArray();
        m.SetUVs(0, uv.ToArray());

        //this is used to get the propper texture for the block
        m.SetUVs(3, typeUVs.ToArray());

        m.Optimize();
        m.RecalculateNormals();
    }


    public List<Vector3> RemoveDuplicateVertices(List<Vector3> verts, List<int> tri)
    {
        List<Vector3> new_verts = new List<Vector3>();


        for (int i = 0; i < verts.Count; i++)
        {
            if (!new_verts.Contains(verts[i]))
            {
                new_verts.Add(verts[i]);
            }
            else
            {
                ChangeAllIndices(tri, i, new_verts.IndexOf(verts[i]));

            }
        }

        

        return new_verts;
    }



    public void ChangeAllIndices(List<int> list, int index, int new_index)
    {
        for(int i = 0; i < list.Count; i++)
        {
            if(list[i].Equals(index))
            {
                list[i] = new_index;
            }
        }

        
    }

    //draws a voxel, regardless of its visibility
    //inspo : http://ilkinulas.github.io/development/unity/2016/04/30/cube-mesh-in-unity3d.html
    Mesh DrawVoxel(int pos_x, int pos_y, int pos_z)
    {

        Mesh m = new Mesh();
        Vector3[] verticies = new Vector3[8];
        
        float x = pos_x * voxelSize;
        float y = pos_y * voxelSize;
        float z = pos_z * voxelSize;


        verticies[0] = new Vector3(x, y, z);
        verticies[1] = new Vector3(x + voxelSize, y, z);
        verticies[2] = new Vector3(x + voxelSize, y + voxelSize, z);
        verticies[3] = new Vector3(x, y + voxelSize, z);

        verticies[4] = new Vector3(x , y + voxelSize, z + voxelSize);
        verticies[5] = new Vector3(x + voxelSize, y + voxelSize, z + voxelSize);
        verticies[6] = new Vector3(x + voxelSize, y, z + voxelSize);
        verticies[7] = new Vector3(x, y, z + voxelSize);


        int[] triangles = {
            0, 2, 1, //face front
	        0, 3, 2,
            2, 3, 4, //face top
	        2, 4, 5,
            1, 2, 5, //face right
	        1, 5, 6,
            0, 7, 4, //face left
	        0, 4, 3,
            5, 4, 7, //face back
	        5, 7, 6,
            0, 6, 7, //face bottom
	        0, 1, 6
        };

        m.Clear();
        m.vertices = verticies;
        m.triangles = triangles;
        m.Optimize();
        m.RecalculateNormals();

        return m;
    }

}
