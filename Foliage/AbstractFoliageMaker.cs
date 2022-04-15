using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractFoliageMaker 
{
    List<int> triangles;
    List<Vector3> verts;
    List<Vector2> uvs;
    List<Vector2> typeIndicies;
    float voxelSize;
    Material mat;
    UVMaker uvMaker;

    protected AbstractFoliageMaker(float voxelSize, Material mat)
    {
        this.voxelSize = voxelSize;
        this.mat = mat;
        

        uvMaker = new UVMaker(voxelSize);
    }

    protected void Build(Vector3 pos, float width, float height)
    {


        GameObject meshObj = new GameObject("Grass Foliage");
        meshObj.AddComponent<MeshRenderer>();

        MeshFilter mF = meshObj.AddComponent<MeshFilter>();
        mF.sharedMesh = new Mesh();


        mF.GetComponent<MeshRenderer>().sharedMaterial = mat;

        
        BuildMesh(width, height, mF.sharedMesh);


        mF.gameObject.SetActive(true);

        int randRot = Random.Range(0, 90);
        Quaternion q = Quaternion.Euler(0, randRot, 0);

        mF.transform.SetPositionAndRotation(pos, q);
       

    }

    private void BuildMesh(float width, float height, Mesh m)
    {
        triangles = new List<int>();
        uvs = new List<Vector2>();
        typeIndicies = new List<Vector2>();
        verts = new List<Vector3>();
        float x = - width / 2;

        //FRONT FACE
        verts.Add(new Vector3(x, 0, 0));
        verts.Add(new Vector3(x, 0 + (height * voxelSize), 0));
        verts.Add(new Vector3(x + (width * voxelSize), 0 + (height * voxelSize), 0));
        verts.Add(new Vector3(x + (width * voxelSize), 0, 0));

        triangles.Add(0);
        triangles.Add(1);
        triangles.Add(2);
        triangles.Add(2);
        triangles.Add(3);
        triangles.Add(0);

        
        //BACKFACE
        verts.Add(new Vector3(x, 0, 0 ));
        verts.Add(new Vector3(x + (width * voxelSize), 0, 0));
        verts.Add(new Vector3(x+ (width * voxelSize), 0+ (height * voxelSize), 0));
        verts.Add(new Vector3(x, 0 + (height * voxelSize), 0));
            
        
        triangles.Add(4);
        triangles.Add(5);
        triangles.Add(6);
        triangles.Add(6);
        triangles.Add(7);
        triangles.Add(4);
       

        uvMaker.Make(uvs, GreedyFaceBuilder.Direction.Front, 1, 1);
        uvMaker.Make(uvs, GreedyFaceBuilder.Direction.Back, -1, 1);

        m.Clear();
        m.vertices = verts.ToArray();
        m.triangles = triangles.ToArray();
        m.SetUVs(0, uvs.ToArray());

        
        //this is used to get the propper texture for the block
        int t = Random.Range(0, 8);

        foreach (Vector3 vert in verts)
        {
            typeIndicies.Add(new Vector2(t, 0));
        }

        m.SetUVs(3, typeIndicies.ToArray());
        

        m.Optimize();
        m.RecalculateNormals();

    }






}
