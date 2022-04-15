using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class I 
{
    public I()
    {

    }

    //Allows you to access 1d arrays 3d
    public static int Get(int x, int y, int z, int size)
    {
        int v_x = x;
        int v_y = size * size * y;
        int v_z = size * z;


        return v_x + v_y + v_z;
    }

    //4 D array
    public static int Get4D(int a, int b, int c, int d, int size)
    {
        int v_a = a;
        int v_b = size * b;
        int v_c = size * size * c;
        int v_d = size * size * size * d;

        return v_a + v_b + v_c + v_d;
    }

    public static int Get(int x, int y,  int size)
    {
        int v_x = x;
        int v_y = size * y;


        return v_x + v_y;
    }

    //Allows you to access 1d arrays 3d, Where sizeX != sizeY
    public static int Get(int x, int y, int z, int sizeX, int sizeZ)
    {
        int v_x = x;
        int v_y = sizeX * sizeZ * y;
        int v_z = sizeZ * z;


        return v_x + v_y + v_z;
    }


    public static int IndexAs2D(int staticValue, int p, int q, int size, string staticAxis)
    {
        if(staticAxis == "x")
        {
            return I.Get(staticValue, p, q, size);
        } else if(staticAxis == "y")
        {
            return I.Get(p, staticValue, q, size);
        } else if(staticAxis == "z")
        {         
           return I.Get(p, q, staticValue, size);
        }

        Debug.Log("Shouldnt happen! Please enter either x, y, z for staticAxis");
        return int.MaxValue;
    }

    public static int ConvertToPQ(Vector3Int vec, string axis, string staticAxis){
        if (staticAxis == "x")
        {
            if(axis == "p")
            {
                return vec.y;
            }
            if (axis == "q")
            {
                return vec.z;
            }
            if (axis == "o")
            {
                return vec.x;
            }

        }
        else if (staticAxis == "y")
        {
            if (axis == "p")
            {
                return vec.x;
            }
            if (axis == "q")
            {
                return vec.z;
            }
            if (axis == "o")
            {
                return vec.y;
            }
        }
        else if (staticAxis == "z")
        {
            if (axis == "p")
            {
                return vec.x;
            }
            if (axis == "q")
            {
                return vec.y;
            }
            if (axis == "o")
            {
                return vec.z;
            }
        }


        return int.MaxValue;
    }


    public static Vector3Int MakeVectorFromSlice(int staticVal, int p, int q, string axis)
    {
        Vector3Int v = new Vector3Int();

        if (axis == "x")
        {
            v = new Vector3Int(staticVal, p, q);
        } else if(axis == "y")
        {
            v = new Vector3Int(p, staticVal, q);
        } else if (axis == "z")
        {
            v = new Vector3Int(p, q, staticVal);
        } 

        return v;
    }

    public static int ConvertToI(Vector3Int start, string axis)
    {
        if (axis == "x")
            return start.z;
        if (axis == "y")
            return start.z;
        if (axis == "z")
            return start.y;

        
        return int.MaxValue;

    }

    public static Vector3Int SetVec(Vector3Int start, int val, string axis)
    {
        Vector3Int v = new Vector3Int();

        if (axis == "x")
        {
            v.x = start.x;
            v.y = start.y;
            v.z = val;
        }
            
        if (axis == "y")
        {
            v.x = start.x;
            v.y = start.y;
            v.z = val;
        }
            
        if (axis == "z")
        {

            v.x = start.x;
            v.y = val;
            v.z = start.z;
        }

        return v;

    }
}

