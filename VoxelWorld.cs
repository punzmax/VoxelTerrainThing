using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoxelWorld : MonoBehaviour
{
    //this is the global size of the world meaning 512x512x512
    //works up to 512, cS = 32 need to be optimized more to work better!


    int count = 0;
    const int size = 512;
    const int chunkSize = 32;

    //int numErosionCycles = 10000;
    int numErosionCycles = 10000;
    const int numChunksY = 8;

    int numChunks;
    public float voxelSize = 0.01f;

    
    Chunk[] chunks;
    [SerializeField, HideInInspector] 
    public MeshFilter[] meshFilters;
    MeshBuilder meshBuilder;
    FoliageBuilder foliageBuilder;
    [HideInInspector]
    public TerrainGenerator terrainGenerator;
    public Material tempMat;
    public Material grassMat;
    public Texture tex;
    VoxelUpdater updater;
    StreetMaker streetMaker;
    ErosionMaker erosionMaker;

    public bool GenerateErosion = true;
    public bool GenerateStreets = true;

    public ComputeShader ErosionComputeShader;

    bool collidersUpdated = false; 

    List<Vector3Int> chunkUpdateQueue;

    // Start is called before the first frame update
    void Start()
    {
        numChunks = size / chunkSize;
        
 
        terrainGenerator = new TerrainGenerator(chunkSize, size);
        
        

        updater = gameObject.AddComponent(typeof(VoxelUpdater)) as VoxelUpdater;

        MakeChunks();

        if(meshFilters == null || meshFilters.Length != numChunks * numChunks * numChunksY)
        {
            meshFilters = new MeshFilter[numChunks * numChunks * numChunksY];
        }
       
        
        meshBuilder = new MeshBuilder(voxelSize);
        foliageBuilder = new FoliageBuilder(voxelSize, grassMat);

        for (int x = 0; x < numChunks; x++)
        {
            for (int y = 0; y < numChunksY; y++)
            {
                for (int z = 0; z < numChunks; z++)
                {
                    UpdateChunk(x, y, z);
                }
            }
        }

        chunkUpdateQueue = new List<Vector3Int>();

        erosionMaker = new ErosionMaker(100, this);
        
    }

    void Update()
    {


        if(GenerateErosion && count < numErosionCycles)
        {
            erosionMaker.Step();
            erosionMaker.UpdateHeights();


        } else if (count == numErosionCycles)
        {
            TurnDirtToGrass();
            //erosionMaker.ClearBuffers();
            streetMaker = new StreetMaker(25, this);
        }
        else
        {
            

            if (GenerateStreets)
            {
                if (streetMaker == null)
                    streetMaker = new StreetMaker(25, this);

                streetMaker.Step();


            }
                
           
            if((!GenerateStreets || streetMaker.Done()) && !collidersUpdated )
            {
                UpdateColliders();
                collidersUpdated = true;
            }

           
        }

       
         
        
        if(count < numErosionCycles)
        {
            if(count % 1000 == 0) // update chunks only every 1000 frames while eroding
            {
                if (chunkUpdateQueue.Count != 0)
                {
                    for (int i = 0; i < chunkUpdateQueue.Count; i++)
                    {
                        UpdateChunk(chunkUpdateQueue[i].x, chunkUpdateQueue[i].y, chunkUpdateQueue[i].z);
                        
                    }

                    chunkUpdateQueue.Clear();
                }
            }
            
        } else
        {
            if (chunkUpdateQueue.Count != 0)
            {
                for (int i = 0; i < chunkUpdateQueue.Count; i++)
                {
                    UpdateChunk(chunkUpdateQueue[i].x, chunkUpdateQueue[i].y, chunkUpdateQueue[i].z);
                    
                }

                chunkUpdateQueue.Clear();
            }
        }
        

        count++;



    }

    void UpdateColliders()
    {
        for(int i = 0; i < meshFilters.Length; i++)
        {

            meshFilters[i].GetComponent<MeshCollider>().sharedMesh = meshFilters[i].mesh;
            
        }
    }



    void OnApplicationQuit()
    {
        
        //erosionMaker.ClearBuffers();
    }


    public void TurnDirtToGrass()
    {
        for(int i = 0; i < terrainGenerator.highestVoxels.Length; i++)
        {
            if(GetVoxelType(terrainGenerator.highestVoxels[i]) == 1)
            {
                SetVoxel(terrainGenerator.highestVoxels[i], 2);
            }
        }
    }
    public void UpdateChunk(int x, int y, int z)
    {
       
        if (meshFilters[I.Get(x, y, z, numChunks)] == null)
        {
            
            GameObject meshObj = new GameObject("mesh" + chunks[I.Get(x, y, z, numChunks)].GetTag());
            meshObj.transform.parent = transform;

            meshObj.AddComponent<MeshRenderer>();

            meshFilters[I.Get(x, y, z, numChunks)] = meshObj.AddComponent<MeshFilter>();
            meshFilters[I.Get(x, y, z, numChunks)].sharedMesh = new Mesh();

            meshFilters[I.Get(x, y, z, numChunks)].gameObject.AddComponent<MeshCollider>();
        }

        meshFilters[I.Get(x, y, z, numChunks)].GetComponent<MeshRenderer>().sharedMaterial = tempMat;

        //meshBuilder.BuildChunkMesh(chunks[I.Get(x, y, z, numChunks)], meshFilters[I.Get(x, y, z, numChunks)].sharedMesh);
        meshBuilder.BuildGreedyChunkMesh(chunks[I.Get(x, y, z, numChunks)], meshFilters[I.Get(x, y, z, numChunks)].sharedMesh, updater);


        meshFilters[I.Get(x, y, z, numChunks)].gameObject.SetActive(true);

        //foliageBuilder.MakeFoliage(chunks[I.Get(x, y, z, numChunks)]);
        

    }


    void MakeChunks()
    {
        chunks = new Chunk[numChunks * numChunks * numChunksY];
        

        for (int x = 0; x < numChunks; x++)
        {
            for (int y = 0; y < numChunksY; y++)
            {
                for (int z = 0; z < numChunks; z++)
                {
                    chunks[I.Get(x, y, z, numChunks)] = FillChunk(x, y, z);
                    
                }
            }
        }
    }

    public Chunk GetChunk(int x, int y, int z)
    {
        if(x < 0 || y < 0 || z < 0 || x >= numChunks || y >= numChunksY || z >= numChunks)
        {
            return null;
        }
        return chunks[I.Get(x, y, z, numChunks)];
    }


    Chunk FillChunk(int chunk_x, int chunk_y, int chunk_z)
    {
        Chunk c = new Chunk(chunkSize, new Vector3Int(chunk_x, chunk_y, chunk_z), this);
        int[] terrainData = terrainGenerator.GetTerrainData(new Vector3Int(chunk_x, chunk_y, chunk_z));
        //int[] terrainData = terrainGenerator.GetDebugTerrain();

        int modX = chunkSize * chunk_x;
        int modY = chunkSize * chunk_y;
        int modZ = chunkSize * chunk_z;

        for (int x = 0; x < chunkSize; x++)
        {
            for (int y = 0; y < chunkSize; y++)
            {
                for (int z = 0; z < chunkSize; z++)
                {
                    Vector3Int voxelPos = new Vector3Int(modX + x, modY + y, modZ + z);

                    c.InsertVoxel(new Voxel(terrainData[I.Get(x, y, z, chunkSize)], voxelPos), new Vector3Int(x, y, z));
                }
            }
        }

        return c;
    }

   
    public void SetVoxel(Vector3Int pos, int type)
    {
        Chunk c = GetChunkByWorldCoordinate(pos.x, pos.y, pos.z);

        int chunk_x = pos.x - (c.pos.x * chunkSize);
        int chunk_y = pos.y - (c.pos.y * chunkSize);
        int chunk_z = pos.z - (c.pos.z * chunkSize);

        Vector3Int c_pos = new Vector3Int(chunk_x, chunk_y, chunk_z);


        //Debug.Log(chunk_x + " " + chunk_y + " " + chunk_z);
        c.InsertVoxel(new Voxel(type, pos), c_pos);


        chunks[I.Get(c.pos.x, c.pos.y, c.pos.z, numChunks)] = c;
        Vector3Int v = new Vector3Int(c.pos.x, c.pos.y, c.pos.z);

        if (!chunkUpdateQueue.Contains(v))
        {
            chunkUpdateQueue.Add(v);
        }
        
    }

    public int GetVoxelType(Vector3Int pos)
    {
        Chunk c = GetChunkByWorldCoordinate(pos.x, pos.y, pos.z);

        int chunk_x = pos.x - (c.pos.x * chunkSize);
        int chunk_y = pos.y - (c.pos.y * chunkSize);
        int chunk_z = pos.z - (c.pos.z * chunkSize);

       

        return c.GetBlockType(chunk_x, chunk_y, chunk_z);

    }
    public Voxel GetVoxel(Vector3Int pos)
    {
        Chunk c = GetChunkByWorldCoordinate(pos.x, pos.y, pos.z);

        int chunk_x = pos.x - (c.pos.x * chunkSize);
        int chunk_y = pos.y - (c.pos.y * chunkSize);
        int chunk_z = pos.z - (c.pos.z * chunkSize);



        return c.GetVoxel(chunk_x, chunk_y, chunk_z);

    }

    



    Chunk GetChunkByWorldCoordinate(int x, int y, int z)
    {
        int posX = Mathf.FloorToInt(x / chunkSize);
        int posY = Mathf.FloorToInt(y / chunkSize);
        int posZ = Mathf.FloorToInt(z / chunkSize);

        return chunks[I.Get(posX, posY, posZ, numChunks)];
    }
   
    public Vector3Int GetPositionOfHighestVoxel(int x, int z)
    {

        
        return terrainGenerator.GetHighestVoxelPosition(x, z);

    }

    public void SetHighestPosition(Vector3Int pos)
    {
         terrainGenerator.SetHighestVoxelPosition(pos.x, pos.z, pos);
    }

    public int GetSize()
    {
        return size;
    }
    
}
