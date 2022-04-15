using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightMaker 
{
    List<GameObject> lights;
 
  

    public LightMaker()
    {
        lights = new List<GameObject>();
    }

    public void MakeLight(Chunk c, float voxelSize, int tx, int ty, int tz)
    {
        float x = tx * voxelSize +(c.pos.x * c.chunkSize * voxelSize) + (voxelSize / 2);
        float y = ty * voxelSize + (c.pos.y * c.chunkSize * voxelSize) + (voxelSize / 2);
        float z = tz * voxelSize + (c.pos.z * c.chunkSize * voxelSize) + (voxelSize / 2);

        GameObject lightGameObject = new GameObject("Light_" + lights.Count);

        // Add the light component
        Light lightComp = lightGameObject.AddComponent<Light>();

        // Set color and position
        lightComp.color = Color.yellow;
        lightComp.type = LightType.Point;
        lightComp.range = 20;
        lightComp.intensity = 5;
        
        lightComp.shadows = LightShadows.Soft;
        lightComp.shadowResolution = UnityEngine.Rendering.LightShadowResolution.Low;

        // Set the position (or any transform property)
        lightGameObject.transform.position = new Vector3(x, y, z);

        lights.Add(lightGameObject);
    }

    // Update is called once per frame
    public void Update()
    {
       /*
        foreach (GameObject light in lights)
            {
           
                float offset = Random.Range(-5.0f, 5.0f);

            //light.GetComponent<Light>().intensity = 5 + offset;
            light.GetComponent<Light>().range = 20 + offset;
            }
            */
    }

}
