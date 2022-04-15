using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Mathematics.math;

public class HeightMapGenerator { 
    int height;
    int[] values;

    // height specifies the maximum height of the noisemap 
    public HeightMapGenerator()
    {

    }
    public int GetHeightMap(int x, int z, int height)
    {

        //i could randomise those offset values, but i want to give the user the option to use a seed
        
        float val = GetPerlinNoise(x, z, 0.03f / 3, 0.7f);


        
        val += GetPerlinNoise(x, z, 0.1f / 3, 0.2f);


        
        val += GetPerlinNoise(x, z, 0.5f / 3, 0.07f);

        
        val += GetPerlinNoise(x, z, 0.9f / 3, 0.03f);



        return Mathf.RoundToInt(val * height);

    }

    //uses Domain warping once
    public int GetHeightMapWarped(int x, int z, int height)
    {

        //i could randomise those offset values, but i want to give the user the option to use a seed
        float[] offset = new float[] {3.0f, 4.0f, 1.2f, 5.1f};
        float val = WarpedNoise1(x, z, 0.03f /  3 / 2, 0.7f, offset);


        offset = new float[] { 1.5f, 3.1f, 1.5f, 2.3f };
        val += WarpedNoise1(x, z, 0.1f / 3 / 2, 0.2f, offset);


        offset = new float[] { 0.2f, 1.0f, 2.5f, 3.1f };
        val += WarpedNoise1(x, z, 0.5f / 3 / 2, 0.07f, offset);

        offset = new float[] { 2.1f, 2.4f, 3.7f, 2.8f };
        val += WarpedNoise1(x, z, 0.9f / 3 / 2, 0.03f,  offset);


        
        return Mathf.RoundToInt(val * height);

    }
    public int GetMaskedHeightMap(int x, int z, int height, int seed)
    {

        int smoothTerrain = GetHeightMapWarped2(x, z, height / 8, seed);

        //float terrainMask = GetPerlinNoise(x + seed, z + seed, 0.03f / 6, 1.0f) * 6 - 2;
        float[] offset = new float[] { 5.0f, 2.0f, 8.5f, 9.4f, 2.5f, 3.2f, 3.5f, 3.0f, 4.3f };
        float terrainMask = WarpedNoise2(x + seed, z + seed, 0.03f / 7, 1.0f, offset) * 5 - 2;

        if (terrainMask > 1.0f)
        {
            terrainMask = 1.0f;
        } else if(terrainMask < 0)
        {
            terrainMask = 0.0f;
        }
        int mountainousTerrain = Mathf.RoundToInt(GetHeightMapWarped2(x, z, height / 4, seed + 123) * terrainMask);


        return Mathf.RoundToInt((smoothTerrain + mountainousTerrain) / 1.125f);
    }


    
    public int GetHeightMapWarped2(int x, int z, int height, int seed)
    {

        //i could randomise those offset values, but i want to give the user the option to use a seed
        float[] offset = new float[] { 3.0f, 4.0f, 1.2f, 5.1f, 2.4f, 2.1f, 1.2f, 4.0f, 0.9f};
        float val = WarpedNoise2(x + seed, z + seed, 0.03f / 3, 0.7f, offset);


        offset = new float[] { 1.5f, 3.1f, 1.5f, 2.3f, 2.1f, 1.5f, 3.1f, 2.3f};
        val += WarpedNoise2(x + seed, z + seed, 0.1f / 3, 0.2f, offset);


        offset = new float[] { 0.2f, 1.0f, 2.5f, 3.1f, 0.9f, 5.1f, 0.2f, 5.2f };
        val += WarpedNoise2(x + seed, z + seed, 0.5f / 3, 0.07f, offset);

        offset = new float[] { 2.1f, 2.4f, 3.7f, 2.8f, 7.1f, 2.2f, 4.2f, 1.2f};
        val += WarpedNoise2(x + seed, z + seed, 0.9f / 3, 0.03f, offset);



        return Mathf.RoundToInt(val * height);

    }



    float GetPerlinNoise(int x, int z, float frequency, float amplitude)
    {

        return Mathf.PerlinNoise(x * frequency , z * frequency ) * amplitude;
        
    }

    float GetPerlinNoise(float x, float z, float frequency, float amplitude)
    {

        return Mathf.PerlinNoise(x * frequency, z * frequency ) * amplitude;
    }


    float WarpedNoise1(float x, float z, float frequency, float amplitude, float[] offset)
    {
        
        float vX = GetPerlinNoise(x + offset[0], z + offset[1], frequency, amplitude);
        float vZ = GetPerlinNoise(x + offset[2], z + offset[3], frequency, amplitude);



        return GetPerlinNoise(vX * 4.0f + x, vZ * 4.0f + z, frequency, amplitude);
    }

    float WarpedNoise2(int x, int z, float frequency, float amplitude, float[] offset)
    {

        float vX = WarpedNoise1(x + offset[4], z + offset[5], frequency, amplitude, offset);
        float vZ = WarpedNoise1(x + offset[6], z + offset[7], frequency, amplitude, offset);



        return GetPerlinNoise(vX * 4.0f + x, vZ *4.0f + z, frequency, amplitude);
    }
}
