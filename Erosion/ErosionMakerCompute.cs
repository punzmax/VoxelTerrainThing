using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ErosionMakerCompute : AgentManager
{

    List<Vector3Int> heightUpdateQueue;

    const int numThreads = 64;

    [SerializeField]
    ComputeBuffer positionsBuffer;
    ComputeBuffer highestPositionsBuffer;

    Vector3Int[] neighbors =  {
        new Vector3Int(-1, 0, -1),
        new Vector3Int(0, 0, -1),
        new Vector3Int(1, 0, -1),

        new Vector3Int(-1, 0, 0),
        new Vector3Int(1, 0, 0),

        new Vector3Int(-1, 0, 1),
        new Vector3Int(0, 0, 1),
        new Vector3Int(1, 0, 1)
    };

    Vector3Int[] startPositions;
    

    ComputeBuffer neighborsBuffer;

    ComputeBuffer outPositionsBuffer;
    Vector3Int[] newPositions;

    ComputeBuffer outHighestPositionsBuffer;
    Vector3Int[] newHighestPositions;

    ComputeBuffer resetBuffer;
    int[] resetPositions;

    ComputeBuffer depositedBuffer;
    int[] lastStepDeposited;

    ComputeBuffer distanceBuffer;
    int[] distance;

    

    int kernel;

    public ComputeShader ErosionAgents;

    static readonly int
        positionsId = Shader.PropertyToID("_Positions"),
        highestPositionsId = Shader.PropertyToID("_HighestPositions"),
        outPositionsId = Shader.PropertyToID("_NewPositions"),
        neighborsId = Shader.PropertyToID("_Neighbors"),
        outHighestPositionsId = Shader.PropertyToID("_NewHighestPositions"),
        depositedId = Shader.PropertyToID("_Deposited"),
        distanceId = Shader.PropertyToID("_DistanceTraveled"),
        resetsId = Shader.PropertyToID("_Resets");

    public ErosionMakerCompute(int numAgents, VoxelWorld v) : base(numAgents, v)
    {

     
        ErosionAgents = v.ErosionComputeShader;

        startPositions = new Vector3Int[numThreads];

        for(int i = 0; i < numThreads; i++)
        {
            startPositions[i] = GetValidStartPos();
        }

        


        heightUpdateQueue = new List<Vector3Int>();



        kernel = ErosionAgents.FindKernel("CSMain");

        
        highestPositionsBuffer = new ComputeBuffer(v.GetSize() * v.GetSize(), 3 * 4);       
        outHighestPositionsBuffer = new ComputeBuffer(v.GetSize() * v.GetSize(), 3 * 4);
        positionsBuffer = new ComputeBuffer(numThreads, 3 * 4);
        outPositionsBuffer = new ComputeBuffer(numThreads, 3 * 4);
        neighborsBuffer = new ComputeBuffer(numThreads, 3 * 4);



        neighborsBuffer.SetData(neighbors);
        ErosionAgents.SetBuffer(kernel, neighborsId, neighborsBuffer);



        resetPositions = new int[numThreads];
        distance = new int[numThreads];
        lastStepDeposited = new int[numThreads];
        newPositions = new Vector3Int[numThreads];
        newHighestPositions = new Vector3Int[v.GetSize() * v.GetSize()];

        resetBuffer = new ComputeBuffer(numThreads, 4);
        resetBuffer.SetData(resetPositions);
        depositedBuffer = new ComputeBuffer(numThreads, 4);

        depositedBuffer.SetData(lastStepDeposited);
        distanceBuffer = new ComputeBuffer(numThreads, 4);
        distanceBuffer.SetData(distance);

        positionsBuffer.SetData(startPositions);
        highestPositionsBuffer.SetData(v.terrainGenerator.highestVoxels);

        ErosionAgents.SetBuffer(kernel, resetsId, resetBuffer);
    }

    override public Vector3Int GetValidStartPos()
    {
        bool foundPos = false;

        int range = v.terrainGenerator.max - v.terrainGenerator.min;
        int min = v.terrainGenerator.min;
        Vector3Int pos = new Vector3Int();



        while (foundPos == false)
        {

            int x = Random.Range(0, v.GetSize() - 1);
            int z = Random.Range(0, v.GetSize() - 1);

            pos = v.GetPositionOfHighestVoxel(x, z);


            if (pos.y > min + Mathf.RoundToInt(range * 0.3f))
            {
                foundPos = true;
            }


        }



        return pos;
    }


    public void ClearBuffers()
    {


        neighborsBuffer.Release();
       
        highestPositionsBuffer.Release();
        outHighestPositionsBuffer.Release();

        positionsBuffer.Release();
        outPositionsBuffer.Release();

        depositedBuffer.Release();
        resetBuffer.Release();
        distanceBuffer.Release();

    }

    public void UpdateHeights()
    {
        for(int i = 0; i < heightUpdateQueue.Count; i++)
        {
            v.SetHighestPosition(heightUpdateQueue[i]);

        }


        heightUpdateQueue.Clear();
    }

    public new void Step()
    {
        UpdateShader();
    }

    public void UpdateShader()
    {


       
        //set buffers
        ErosionAgents.SetBuffer(kernel, positionsId, positionsBuffer);
        ErosionAgents.SetBuffer(kernel, highestPositionsId, highestPositionsBuffer);
        
        ErosionAgents.SetBuffer(kernel, outPositionsId, outPositionsBuffer);
        ErosionAgents.SetBuffer(kernel, outHighestPositionsId, outHighestPositionsBuffer);
        ErosionAgents.SetBuffer(kernel, depositedId, depositedBuffer);
        ErosionAgents.SetBuffer(kernel, distanceId, distanceBuffer);

        //dispatch
        ErosionAgents.Dispatch(kernel, numThreads, 1, 1);




        //output of shader
        outPositionsBuffer.GetData(newPositions);
        outHighestPositionsBuffer.GetData(newHighestPositions);
        
        resetBuffer.GetData(resetPositions);

        //reset
        Vector3Int[] t = new Vector3Int[64];
        positionsBuffer.GetData(t);

        CheckResets();

        positionsBuffer.SetData(newPositions);

        CheckHeights();

        
        









        
        

        //Debug.Log(t[1]);
        
    }

    void CheckHeights()
    {
        for(int i = 0; i < newHighestPositions.Length; i++)
        {
            if (newHighestPositions[i] != null)
            {
                Vector3Int p = new Vector3Int(newHighestPositions[i].x, newHighestPositions[i].y, newHighestPositions[i].z);

                if(p.y > v.terrainGenerator.highestVoxels[p.x + (p.z * 512)].y)
                {
                    v.SetVoxel(p, 1);
                    
                    v.terrainGenerator.highestVoxels[p.x + (p.z * 512)] = p;

                } else if (p.y < v.terrainGenerator.highestVoxels[p.x + (p.z * 512)].y)
                {
                    v.SetVoxel(v.terrainGenerator.highestVoxels[p.x + (p.z * 512)], 0);
                    
                    v.terrainGenerator.highestVoxels[p.x + (p.z * 512)] = p;
                }


            }



        }

        newHighestPositions = new Vector3Int[64];
    }

    

    void CheckResets()
    {
        for(int i = 0; i < resetPositions.Length; i++)
        {
            if(resetPositions[i] == 1)
            {
                newPositions[i] = GetValidStartPos();

                
            }
            
        }


        resetPositions = new int[numThreads];
    }

    public void AddToHeightQueue(Vector3Int newPos)
    {
        heightUpdateQueue.Add(newPos);
    }

}
