using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoxelUpdater : MonoBehaviour
{
    public static LightMaker lightMaker;
    //float updateCount = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        if (lightMaker == null)
        {
            lightMaker = new LightMaker();
        }
    }

    // Update is called once per frame
    void Update()
    {

        /*
        updateCount += 1.0f * Time.deltaTime;

       

        if (updateCount >= 0.1f)
        {
            
            lightMaker.Update();
            updateCount = 0f;
        }

        */
    }

    public void MakeLight(Chunk c, float voxelSize, int x, int y, int z)
    {
        if(lightMaker == null)
        {
            lightMaker = new LightMaker();
        }
        lightMaker.MakeLight(c, voxelSize, x, y, z);
    }
}
