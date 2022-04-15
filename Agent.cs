using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Agent 
{
    protected Vector3Int pos;
    protected AgentManager parent;
    protected int distanceTraveled;
    public Agent(Vector3Int pos, AgentManager parent)
    {
        this.pos = pos;
        this.parent = parent;
    }

    // Update is called once per frame
    virtual public void Step()
    {
        
    }

    protected bool InBounds(int x, int z)
    {
        if (0 <= x && x < parent.v.GetSize())
        {
            if (0 <= z && z < parent.v.GetSize())
            {
                return true;
            }
        }


        return false;
    }
}
