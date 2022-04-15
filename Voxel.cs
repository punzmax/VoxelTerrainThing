using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Voxel
{
    public Vector3Int position;
    public int type;
    enum Faces {Top, Bottom, Left, Right, Front, Back};
    List<Faces> visibleFaces;

    public Voxel(int type, Vector3Int position)
    {
        this.type = type;
        this.position = position;
      
    }
   



    public void checkVisibleFaces()
    {

    }

    

    public bool IsOpaque()
    {
        if(type == 0 || type > 13)
        {
            return false;
        }

        return true;
    }
    
   
}
