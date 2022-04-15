using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//this is still WIP
public class GreedyFaceBuilder : FaceBuilder 
{
    public enum Direction {Top, Bottom, Left, Right, Front, Back};
    bool[] visited_Top;
    bool[] visited_Bottom;
    bool[] visited_Left;
    bool[] visited_Right;
    bool[] visited_Front;
    bool[] visited_Back;

    float voxelSize;
    List<Vector3> used_Verts;
    List<int> t_triangles;
    
    Run run;
    

    public GreedyFaceBuilder(float voxelSize)
    {
        this.voxelSize = voxelSize;
        run = new Run(voxelSize);
        
    }



    public void Build(Chunk c, Vector3Int pos, List<int> triangles, List<Vector3> verts, List<Vector2> uv, List<Vector2> typeUV, VoxelUpdater updater)
    {
        visited_Top = new bool[c.chunkSize * c.chunkSize * c.chunkSize];
        visited_Bottom = new bool[c.chunkSize * c.chunkSize * c.chunkSize];
        visited_Left = new bool[c.chunkSize * c.chunkSize * c.chunkSize];
        visited_Right = new bool[c.chunkSize * c.chunkSize * c.chunkSize];
        visited_Front = new bool[c.chunkSize * c.chunkSize * c.chunkSize];
        visited_Back = new bool[c.chunkSize * c.chunkSize * c.chunkSize];





        for (int x = 0; x < c.chunkSize; x++)
        {
            for (int y = 0; y < c.chunkSize; y++)
            {
                for(int z = 0; z < c.chunkSize; z++)
                {

                    if(c.GetBlockType(x, y, z) < 10 && c.GetBlockType(x, y, z) != 0)
                    {
                        //Make Top face Run
                        if (!visited_Top[I.Get(x, y, z, c.chunkSize)] && c.IsOpaque(x, y, z) && !c.HasTopNeighbor(new Vector3Int(x, y, z)))
                        {

                            run.MakeRun(c, triangles, verts, uv, typeUV, new Vector3Int(x, y, z), visited_Top, c.GetBlockType(x, y, z), Direction.Top);
                            visited_Top[I.Get(x, y, z, c.chunkSize)] = true;
                        }

                        //Make Bottomface run
                        if (!visited_Bottom[I.Get(x, y, z, c.chunkSize)] && c.IsOpaque(x, y, z) && !c.HasBottomNeighbor(new Vector3Int(x, y, z)))
                        {
                            run.MakeRun(c, triangles, verts, uv, typeUV, new Vector3Int(x, y, z), visited_Bottom, c.GetBlockType(x, y, z), Direction.Bottom);
                            visited_Bottom[I.Get(x, y, z, c.chunkSize)] = true;
                        }


                        //Make Left run
                        if (!visited_Left[I.Get(x, y, z, c.chunkSize)] && c.IsOpaque(x, y, z) && !c.HasLeftNeighbor(new Vector3Int(x, y, z)))
                        {
                            run.MakeRun(c, triangles, verts, uv, typeUV, new Vector3Int(x, y, z), visited_Left, c.GetBlockType(x, y, z), Direction.Left);
                            visited_Left[I.Get(x, y, z, c.chunkSize)] = true;
                        }

                        //Make Right run
                        if (!visited_Right[I.Get(x, y, z, c.chunkSize)] && c.IsOpaque(x, y, z) && !c.HasRightNeighbor(new Vector3Int(x, y, z)))
                        {
                            run.MakeRun(c, triangles, verts, uv, typeUV, new Vector3Int(x, y, z), visited_Right, c.GetBlockType(x, y, z), Direction.Right);
                            visited_Right[I.Get(x, y, z, c.chunkSize)] = true;
                        }


                        //Make Front run
                        if (!visited_Front[I.Get(x, y, z, c.chunkSize)] && c.IsOpaque(x, y, z) && !c.HasFrontNeighbor(new Vector3Int(x, y, z)))
                        {
                            run.MakeRun(c, triangles, verts, uv, typeUV, new Vector3Int(x, y, z), visited_Front, c.GetBlockType(x, y, z), Direction.Front);
                            visited_Front[I.Get(x, y, z, c.chunkSize)] = true;
                        }



                        //Make back run
                        if (!visited_Back[I.Get(x, y, z, c.chunkSize)] && c.IsOpaque(x, y, z) && !c.HasBackNeighbor(new Vector3Int(x, y, z)))
                        {
                            run.MakeRun(c, triangles, verts, uv, typeUV, new Vector3Int(x, y, z), visited_Back, c.GetBlockType(x, y, z), Direction.Back);
                            visited_Back[I.Get(x, y, z, c.chunkSize)] = true;
                        }
                    }
                    else
                    {
                        if (c.GetBlockType(x, y, z) == 13)
                        {
                            updater.MakeLight(c, voxelSize, x, y, z);
                            
                        }
                    }




                }
            }
        }

    }

}
