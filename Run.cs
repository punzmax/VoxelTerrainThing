using System.Collections.Generic;
using UnityEngine;

public class Run 
{
    float voxelSize;
    List<Vector3Int> tempVisit;
    int indexAxis;
    int type;
    List<Vector2> uvs;
    UVMaker uvMaker;

    public Run(float voxelSize)
    {
        tempVisit = new List<Vector3Int>();
        this.voxelSize = voxelSize;
        
    }


    public void MakeRun(Chunk c, List<int> triangles, List<Vector3> verts, List<Vector2> uv, List<Vector2> typeUV, Vector3Int start,  bool[] visited, int type, GreedyFaceBuilder.Direction d)
    {
        tempVisit = new List<Vector3Int>();

        uvMaker = new UVMaker(voxelSize);

        int width = 1;
        int runLength = 1;
        this.type = type;

        string indexAxis = "";
        if (d == GreedyFaceBuilder.Direction.Left || d == GreedyFaceBuilder.Direction.Right)
        {
            indexAxis = "x";
        }
        else if (d == GreedyFaceBuilder.Direction.Top || d == GreedyFaceBuilder.Direction.Bottom)
        {
            indexAxis = "y";
        }
        else if (d == GreedyFaceBuilder.Direction.Front|| d == GreedyFaceBuilder.Direction.Back)
        {
            indexAxis = "z";
        }

        //Debug.Log(runLength + " " + width + " " + d);
        

        int startP = I.ConvertToPQ(start, "p", indexAxis);
        int startQ = I.ConvertToPQ(start, "q", indexAxis);
       
        int startO = I.ConvertToPQ(start, "o", indexAxis);

        int p = startP;
        
        int q  = startQ + 1;

        bool pRun = true;

            
        while(q < c.chunkSize && pRun)
        {
            //form vec(start, p, q);
            Vector3Int v = I.MakeVectorFromSlice(startO, p, q, indexAxis);

            if (!c.HasNeighbor(v, d) && c.IsOpaque(v.x, v.y, v.z) && c.GetBlockType(v.x, v.y, v.z) == type)
            {
                runLength++;
                visited[I.Get(v.x, v.y, v.z, c.chunkSize)] = true;
            } else
            {
                pRun = false;
            }

            
            //visited[I.Get(startO, p, q, c.chunkSize)] = true;
            
            q++;
        }

        bool qRun = true;
        p++;

  
        q = startQ;

       
        while (p < c.chunkSize && qRun)
        {
            if (TryRun(I.MakeVectorFromSlice(startO, p, q, indexAxis), indexAxis, c, runLength, d))
            {
                width++;
                
                AddVisited(visited, c.chunkSize);
                
               

            } else
            {
                qRun = false; 
            }
            
            
            p++;
        }


        

        DrawRun(c, triangles, verts, uv, start, width, runLength, d);
        uvMaker.Make(uv, d, width, runLength);

        AssignTexture(typeUV,type, d);
        
    }
   

    bool TryRun(Vector3Int startPos, string axis, Chunk c, int height, GreedyFaceBuilder.Direction d)
    {

        int t_height = 0;
        bool doRun = true;
        tempVisit = new List<Vector3Int>();

        Vector3Int v = startPos;
        

        int i = I.ConvertToI(startPos, axis);

        while (i < c.chunkSize && doRun == true)
        {

            

            if (!c.HasNeighbor(new Vector3Int(v.x, v.y, v.z), d) && c.IsOpaque(v.x, v.y, v.z) && c.GetBlockType(v.x, v.y, v.z) == type)
            {

                t_height++;
                tempVisit.Add(new Vector3Int(v.x, v.y, v.z));


            } else
            {
                doRun = false;
            }
            i++;
           
            
            v = I.SetVec(v, i, axis);
            
            
        }

       

        return t_height == height;

    }


    public void DrawTopRun(Vector3[] vertPos, float x, float y, float z, int width, int height)
    {
        vertPos[0] = new Vector3(x, y + voxelSize, z);
        vertPos[1] = new Vector3(x, y + voxelSize, z + (height * voxelSize)); 
        vertPos[2] = new Vector3(x + (width * voxelSize), y + voxelSize, z + (height * voxelSize));
        vertPos[3] = new Vector3(x + (width * voxelSize), y + voxelSize, z);

    }
    public void DrawBottomRun(Vector3[] vertPos, float x, float y, float z, int width, int height)
    {
        
        vertPos[0] = new Vector3(x, y, z);
        vertPos[1] = new Vector3(x + (width * voxelSize), y, z);
        vertPos[2] = new Vector3(x + (width * voxelSize), y , z + (height * voxelSize));
        vertPos[3] = new Vector3(x, y, z + (height * voxelSize));
    }


    public void DrawLeftRun(Vector3[] vertPos, float x, float y, float z, int width, int height)
    {
        vertPos[0] = new Vector3(x, y, z);
        vertPos[1] = new Vector3(x , y, z + (height * voxelSize));
        vertPos[2] = new Vector3(x, y + (width * voxelSize), z + (height * voxelSize));
        vertPos[3] = new Vector3(x, y + (width * voxelSize), z);
    }

    public void DrawRightRun(Vector3[] vertPos, float x, float y, float z, int width, int height)
    {
        vertPos[0] = new Vector3(x + voxelSize, y, z);
        vertPos[1] = new Vector3(x + voxelSize, y + (width * voxelSize), z);
        vertPos[2] = new Vector3(x + voxelSize, y + (width * voxelSize), z + (height * voxelSize));
        vertPos[3] = new Vector3(x + voxelSize, y, z + (height * voxelSize));
    }

    public void DrawFrontRun(Vector3[] vertPos, float x, float y, float z, int width, int height)
    {
        vertPos[0] = new Vector3(x, y, z);
        vertPos[1] = new Vector3(x, y + (height * voxelSize), z);
        vertPos[2] = new Vector3(x + (width * voxelSize), y + (height * voxelSize), z);
        vertPos[3] = new Vector3(x + (width * voxelSize), y, z);
    }

    public void DrawBackRun(Vector3[] vertPos, float x, float y, float z, int width, int height)
    {
        vertPos[0] = new Vector3(x, y, z + voxelSize);
        vertPos[1] = new Vector3(x + (width * voxelSize), y, z + voxelSize);
        vertPos[2] = new Vector3(x + (width * voxelSize), y + (height * voxelSize), z + voxelSize);
        vertPos[3] = new Vector3(x, y + (height * voxelSize), z + voxelSize);
    }



    public void DrawRun(Chunk c, List<int> triangles, List<Vector3> verts, List<Vector2> uv, Vector3Int start, int width, int height, GreedyFaceBuilder.Direction d)
    {
        Vector3[] vertPos = new Vector3[4];

        float x = start.x * voxelSize + (c.pos.x * c.chunkSize * voxelSize);
        float y = start.y * voxelSize + (c.pos.y * c.chunkSize * voxelSize);
        float z = start.z * voxelSize + (c.pos.z * c.chunkSize * voxelSize);

        switch (d)
        {
            case (GreedyFaceBuilder.Direction.Top):
                DrawTopRun(vertPos, x, y, z, width, height);
                break;
            case (GreedyFaceBuilder.Direction.Bottom):
                DrawBottomRun(vertPos, x, y, z, width, height);
                break;
            case (GreedyFaceBuilder.Direction.Left):
                DrawLeftRun(vertPos, x, y, z, width, height);
                break;
            case (GreedyFaceBuilder.Direction.Right):
                DrawRightRun(vertPos, x, y, z, width, height);
                break;
            case (GreedyFaceBuilder.Direction.Front):
                DrawFrontRun(vertPos, x, y, z, width, height);
                break;
            case (GreedyFaceBuilder.Direction.Back):
                DrawBackRun(vertPos, x, y, z, width, height);
                break;
        }

        triangles.Add(0 + verts.Count);
        triangles.Add(1 + verts.Count);
        triangles.Add(2 + verts.Count);

        triangles.Add(2 + verts.Count);
        triangles.Add(3 + verts.Count);
        triangles.Add(0 + verts.Count);

        verts.Add(vertPos[0]);
        verts.Add(vertPos[1]);
        verts.Add(vertPos[2]);
        verts.Add(vertPos[3]);

        


    }

    public void AddVisited(bool[] visited, int size)
    {

        foreach (Vector3Int entry in tempVisit)
        {

            visited[I.Get(entry.x, entry.y, entry.z, size)] = true;
        
        }

    }

    public void AssignTexture(List<Vector2> typeUV, int type, GreedyFaceBuilder.Direction d)
    {
        switch (type)
        {
            case 2: //Grass Block
                if(d != GreedyFaceBuilder.Direction.Top){
                    type = 3;
                } else if (d == GreedyFaceBuilder.Direction.Bottom)
                {
                    type = 1;
                }
                break;
        }

        typeUV.Add(new Vector2((float)type, 0));
        typeUV.Add(new Vector2((float)type, 0));
        typeUV.Add(new Vector2((float)type, 0));
        typeUV.Add(new Vector2((float)type, 0));
    }
}
