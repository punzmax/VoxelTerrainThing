using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UVMaker 
{

    float voxelSize;
    public UVMaker(float voxelSize)
    {
        this.voxelSize = voxelSize; 
    }

    public void Make(List<Vector2> uv, GreedyFaceBuilder.Direction d, int length, int width)
    {
        float factor = length * width * voxelSize / 0.75f  * 0.5f;


        switch (d)
        {
            case GreedyFaceBuilder.Direction.Bottom:
                
                
                uv.Add(new Vector2(0.0f / (width), 15.0f / (length)) * factor);
                uv.Add(new Vector2(15.0f / (width), 15.0f / (length)) * factor);
                uv.Add(new Vector2(15.0f / (width), 0.0f / (length)) * factor);
                uv.Add(new Vector2(0.0f / (width), 0.0f / (length)) * factor);
                break;
            case GreedyFaceBuilder.Direction.Top:
                uv.Add(new Vector2(0.0f / (width), 0.0f / (length)) * factor);
                uv.Add(new Vector2(0.0f / (width), 15.0f / (length)) * factor);
                uv.Add(new Vector2(15.0f / (width), 15.0f / (length)) * factor);
                uv.Add(new Vector2(15.0f / (width), 0.0f / (length)) * factor);
                break;
            case GreedyFaceBuilder.Direction.Left:
                uv.Add(new Vector2(15.0f / (length), 0.0f / (width)) * factor);
                uv.Add(new Vector2(0.0f / (length), 0.0f / (width)) * factor);
                uv.Add(new Vector2(0.0f / (length), 15.0f / (width)) * factor);
                uv.Add(new Vector2(15.0f / (length), 15.0f / (width)) * factor);
                break;
            case GreedyFaceBuilder.Direction.Right:

                uv.Add(new Vector2((0.0f / length) , (0.0f / width) ) * factor);
                uv.Add(new Vector2((0.0f / length) , (15.0f / width)) * factor );
                uv.Add(new Vector2((15.0f / length), (15.0f / width)) * factor );
                uv.Add(new Vector2((15.0f / length) , (0.0f / width)) * factor);

                break;
            case GreedyFaceBuilder.Direction.Front:

                uv.Add(new Vector2(0.0f / (width), 0.0f / (length)) * factor);
                uv.Add(new Vector2(0.0f / (width), 15.0f / (length)) * factor);
                uv.Add(new Vector2(15.0f / (width), 15.0f / (length)) * factor);
                uv.Add(new Vector2(15.0f / (width), 0.0f / (length)) * factor);
                break;
            case GreedyFaceBuilder.Direction.Back:
                uv.Add(new Vector2(15.0f / (width), 0.0f / (length)) * factor);
                uv.Add(new Vector2(0.0f / (width), 0.0f / (length)) * factor);
                uv.Add(new Vector2(0.0f / (width), 15.0f / (length)) * factor);
                uv.Add(new Vector2(15.0f / (width), 15.0f / (length)) * factor);
                break;
        }


        
        

    }
}
