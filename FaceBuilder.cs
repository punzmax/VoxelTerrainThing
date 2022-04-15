using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FaceBuilder 
{
    public void Build(Chunk c, Vector3Int pos, List<int> triangles, List<Vector3> verts, float voxelSize) { }

    protected void AddVertIndices(List<int> list, int[] verts)
    {
        for (int i = 0; i < verts.Length; i++)
        {
            list.Add(verts[i]);
        }
    }



    protected List<int> CalculateUsedVerts(List<int> t_triangles, Vector3[] cube_Verts, List<Vector3> used_Verts, List<Vector3> all_verts)
    {

        int offset = all_verts.Count;

        for (int i = 0; i < cube_Verts.Length; i++)
        {

            if (t_triangles.Contains(i) && !all_verts.Contains(cube_Verts[i]))
            {

                used_Verts.Add(cube_Verts[i]);

            }

        }



        for (int i = 0; i < t_triangles.Count; i++)
        {
            if (used_Verts.Contains(cube_Verts[t_triangles[i]]))
            {
                t_triangles[i] = used_Verts.IndexOf(cube_Verts[t_triangles[i]]) + offset;
            }
            else
            {
                if (all_verts.Contains(cube_Verts[t_triangles[i]]))
                {
                    t_triangles[i] = all_verts.IndexOf(cube_Verts[t_triangles[i]]);
                }
                else
                {
                    Debug.Log("Something went hottibly wrong!");
                }
            }

        }

        return t_triangles;

    }
}
