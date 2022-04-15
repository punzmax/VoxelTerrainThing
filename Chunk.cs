using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk
{

    Voxel[] voxels;
    public Vector3Int pos;

    public int chunkSize;
    public VoxelWorld world;

    public Chunk(int chunkSize, Vector3Int pos, VoxelWorld world)
    {
        this.world = world;
        this.chunkSize = chunkSize;
        voxels = new Voxel[chunkSize * chunkSize * chunkSize];
        this.pos = pos;
    }

    public bool HasNeighbor(Vector3Int voxelPos, GreedyFaceBuilder.Direction d)
    {
        switch (d)
        {
            case GreedyFaceBuilder.Direction.Top:
                return HasTopNeighbor(voxelPos);
            case GreedyFaceBuilder.Direction.Bottom:
                return HasBottomNeighbor(voxelPos);
            case GreedyFaceBuilder.Direction.Left:
                return HasLeftNeighbor(voxelPos);
            case GreedyFaceBuilder.Direction.Right:
                return HasRightNeighbor(voxelPos);
            case GreedyFaceBuilder.Direction.Front:
                return HasFrontNeighbor(voxelPos);
            case GreedyFaceBuilder.Direction.Back:
                return HasBackNeighbor(voxelPos);

        }
        return false;
    }

    public void InsertVoxel(Voxel v, Vector3Int pos)
    {
        voxels[I.Get(pos.x, pos.y, pos.z, chunkSize)] = v;
        
    }

    public void CalculateVertexPositions()
    {

    }
    public bool IsOpaque(int x,int y,int z)
    {
       
        return voxels[I.Get(x, y, z, chunkSize)].IsOpaque();
    }

    public int GetBlockType(int x, int y, int z)
    {
        return voxels[I.Get(x, y, z, chunkSize)].type;
    }

    public Voxel GetVoxel(int x, int y, int z)
    {
        return voxels[I.Get(x, y, z, chunkSize)];
    }

    public string GetTag()
    {
        return "_" + pos.x + "_" + pos.y + "_" + pos.z;
    }

    public bool HasTopNeighbor(Vector3Int voxelPos)
    {

        if (voxelPos.y + 1 >= chunkSize || !voxels[I.Get(voxelPos.x, voxelPos.y + 1, voxelPos.z, chunkSize)].IsOpaque()) {
            if (voxelPos.y + 1 >= chunkSize && world.GetChunk(pos.x, pos.y + 1, pos.z) != null)
            {
                return world.GetChunk(pos.x, pos.y + 1, pos.z).IsOpaque(voxelPos.x, 0, voxelPos.z);
            }
            return false;
        }

        return true;
       
    }

    public bool HasBottomNeighbor(Vector3Int voxelPos)
    {

        if ( voxelPos.y - 1 < 0 || !voxels[I.Get(voxelPos.x, voxelPos.y - 1, voxelPos.z, chunkSize)].IsOpaque() )
        {
            if(voxelPos.y - 1 < 0 && world.GetChunk(pos.x, pos.y-1, pos.z) != null)
            {
                return world.GetChunk(pos.x, pos.y - 1, pos.z).IsOpaque(voxelPos.x, chunkSize - 1, voxelPos.z);
            }
            return false;
        }

        return true;
    }

    public bool HasLeftNeighbor(Vector3Int voxelPos)
    {

        if (voxelPos.x - 1 < 0 || !voxels[I.Get(voxelPos.x - 1, voxelPos.y, voxelPos.z, chunkSize)].IsOpaque())
        {
            if (voxelPos.x - 1 < 0 && world.GetChunk(pos.x - 1, pos.y, pos.z) != null)
            {
                return world.GetChunk(pos.x - 1, pos.y, pos.z).IsOpaque(chunkSize - 1, voxelPos.y, voxelPos.z);
            }

            return false;
        }

        return true;
    }

    public bool HasRightNeighbor(Vector3Int voxelPos)
    {

        if (voxelPos.x + 1 >= chunkSize || !voxels[I.Get(voxelPos.x + 1, voxelPos.y, voxelPos.z, chunkSize)].IsOpaque())
        {
            if (voxelPos.x + 1 >= chunkSize && world.GetChunk(pos.x + 1, pos.y, pos.z) != null)
            {
                return world.GetChunk(pos.x + 1, pos.y, pos.z).IsOpaque(0, voxelPos.y, voxelPos.z);
            }
            return false;
        }

        return true;
    }

    public bool HasFrontNeighbor(Vector3Int voxelPos)
    {

        if (voxelPos.z - 1 < 0 || !voxels[I.Get(voxelPos.x, voxelPos.y, voxelPos.z - 1, chunkSize)].IsOpaque())
        {
            if (voxelPos.z - 1 < 0 && world.GetChunk(pos.x, pos.y, pos.z - 1) != null)
            {
                return world.GetChunk(pos.x, pos.y, pos.z - 1).IsOpaque(voxelPos.x, voxelPos.y, chunkSize - 1);
            }
            return false;
        }

        return true;
    }

    public bool HasBackNeighbor(Vector3Int voxelPos)
    {

        if (voxelPos.z + 1 >= chunkSize || !voxels[I.Get(voxelPos.x, voxelPos.y, voxelPos.z + 1, chunkSize)].IsOpaque())
        {
            if (voxelPos.z + 1 >= chunkSize && world.GetChunk(pos.x, pos.y, pos.z + 1) != null)
            {
                return world.GetChunk(pos.x, pos.y, pos.z + 1).IsOpaque(voxelPos.x, voxelPos.y, 0);
            }
            return false;
        }

        return true;
    }


    //convert the raw voxel data into something the marching cubes LUT can read
    public int GetBinaryConfiguration(Vector3Int voxelPos)
    {
        
        int[] data = new int[8];

        if (HasBackNeighbor(voxelPos))
        {
            data[0] = 1;
            data[1] = 1;
            data[2] = 1;
            data[3] = 1;
        }
        if (HasFrontNeighbor(voxelPos))
        {
            data[4] = 1;
            data[5] = 1;
            data[6] = 1;
            data[7] = 1;
        }
        if (HasLeftNeighbor(voxelPos))
        {
            data[0] = 1;
            data[3] = 1;
            data[4] = 1;
            data[7] = 1;
        }
        if (HasRightNeighbor(voxelPos))
        {
            data[1] = 1;
            data[2] = 1;
            data[5] = 1;
            data[6] = 1;
        }
        if (HasTopNeighbor(voxelPos))
        {
            data[2] = 1;
            data[3] = 1;
            data[6] = 1;
            data[7] = 1;
        }
        if (HasBottomNeighbor(voxelPos))
        {
            data[0] = 1;
            data[1] = 1;
            data[4] = 1;
            data[5] = 1;
        }


        return RearrangeBitArray(data);

    }

    private int RearrangeBitArray(int[] data)
    {
        int mult = 1;
        int val = 0;

        for (int i = 0; i < data.Length; i++)
        {
            val += data[i] * mult;

            mult = mult * 10;
        }


        return val;
    }


}

